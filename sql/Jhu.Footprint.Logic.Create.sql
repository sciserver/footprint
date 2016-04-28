CREATE SCHEMA [fps]
GO

CREATE PROC [fps].[spUpdateCombinedRegion]
(
	@FootprintID int,
	@RegionID int
)
AS
	DECLARE @count int = NULL
	DECLARE @combinedRegionID int = NULL
	DECLARE @combineMethod tinyint = NULL
	DECLARE @combinedRegion [dbo].[Region] = NULL
	
	-- Count single regions
	SELECT @count = COUNT(*)
	FROM Footprint f
	INNER JOIN FootprintRegion r
		ON f.CombinedRegionID = r.ID
	WHERE r.Type = 0

	IF @count = 1 BEGIN
		-- Only a single region exists

		UPDATE Footprint
		SET CombinedRegionID = @RegionID
		WHERE ID = @FootprintID
	END
	ELSE BEGIN
		-- More than one single regions

		-- Load combined region
		SELECT @combinedRegion = r.Region,
		       @combineMethod = f.Method
		FROM Footprint f
		LEFT OUTER JOIN FootprintRegion r
			ON f.CombinedRegionID = r.ID
		WHERE f.ID = @FootprintID
			  AND r.Type = 1	-- combined region

		IF @combinedRegion IS NULL BEGIN
			-- Create new combined region

			IF @combineMethod = 1 BEGIN
				-- UNION

				SELECT @combinedRegion = region.UnionEvery(Region)
				FROM FootprintRegion r
				WHERE r.FootprintID = @FootprintID
				  AND r.Type = 0
			END ELSE IF @combineMethod = 2 BEGIN
				-- INTERSECT

				SELECT @combinedRegion = region.IntersectEvery(Region)
				FROM FootprintRegion r
				WHERE r.FootprintID = @FootprintID
				  AND r.Type = 0
			END ELSE THROW 51000, 'Invalid combination method.', 1;

			INSERT FootprintRegion
				([FootprintID], [Name], [FillFactor], [Type], [Region], [Thumbnail])
			VALUES
				(@FootprintID, 'combined', 1.0, 1, @combinedRegion.ToBinary(), NULL)

			SET @combinedRegionID = @@IDENTITY

			UPDATE Footprint
			SET CombinedRegionID = @combinedRegionID
		END ELSE BEGIN
			-- Update combined region with the very last one

			IF @combineMethod = 1 BEGIN
				-- UNION

				UPDATE FootprintRegion
				SET Region = @combinedRegion.[Union](Region).ToBinary()
				WHERE ID = @combinedRegionID
			END ELSE IF @combineMethod = 2 BEGIN
				-- INTERSECT

				UPDATE FootprintRegion
				SET Region = @combinedRegion.[Intersect](Region).ToBinary()
				WHERE ID = @combinedRegionID
			END ELSE THROW 51000, 'Invalid combination method.', 1;

		END
	END

	-- TODO: recompute HTM index

GO