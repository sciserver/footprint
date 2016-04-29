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
	DECLARE @combinationMethod tinyint = NULL
	DECLARE @combinedRegion [dbo].[Region] = NULL
	
	-- Count single regions
	SELECT @count = ISNULL(COUNT(*), 0)
	FROM Footprint f
	INNER JOIN FootprintRegion r
		ON r.FootprintID = f.ID AND r.Type = 0
	WHERE f.ID = @FootprintID 

	IF @count = 1 BEGIN
		-- Only a single region exists

		UPDATE Footprint
		SET CombinedRegionID = @RegionID
		WHERE ID = @FootprintID
	END
	ELSE BEGIN
		-- More than one single regions

		-- Load combined region
		SELECT @combinationMethod = f.CombinationMethod,
		       @combinedRegion = r.Region
		FROM Footprint f
		LEFT OUTER JOIN FootprintRegion r
			ON f.CombinedRegionID = r.ID
			   AND r.Type = 1	-- combined region
		WHERE f.ID = @FootprintID

		IF @combinedRegion IS NULL BEGIN
			-- Create new combined region

			IF @combinationMethod = 1 BEGIN
				-- UNION

				SELECT @combinedRegion = region.UnionEvery(Region)
				FROM FootprintRegion r
				WHERE r.FootprintID = @FootprintID
				  AND r.Type = 0

			END ELSE IF @combinationMethod = 2 BEGIN
				-- INTERSECT

				SELECT @combinedRegion = region.IntersectEvery(Region)
				FROM FootprintRegion r
				WHERE r.FootprintID = @FootprintID
				  AND r.Type = 0

			END ELSE THROW 51000, 'Invalid combination method.', 1;

			-- Save combined region
			INSERT FootprintRegion
				([FootprintID], [Name], [FillFactor], [Type], [Region], [Thumbnail])
			VALUES
				(@FootprintID, 'combined', 1.0, 1, @combinedRegion.ToBinary(), NULL)

			SET @combinedRegionID = @@IDENTITY

			UPDATE Footprint
			SET CombinedRegionID = @combinedRegionID
		END ELSE BEGIN
			-- Update combined region with the very last one

			IF @combinationMethod = 1 BEGIN
				-- UNION

				UPDATE FootprintRegion
				SET Region = @combinedRegion.[Union](Region).ToBinary()
				WHERE ID = @combinedRegionID
			END ELSE IF @combinationMethod = 2 BEGIN
				-- INTERSECT

				UPDATE FootprintRegion
				SET Region = @combinedRegion.[Intersect](Region).ToBinary()
				WHERE ID = @combinedRegionID
			END ELSE THROW 51000, 'Invalid combination method.', 1;

		END
	END

	-- TODO: recompute HTM index

GO

IF OBJECT_ID('[fps].[spRefreshCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spRefreshCombinedRegion]
GO

CREATE PROC [fps].[spRefreshCombinedRegion]
(
	@FootprintID int
)
AS
	DECLARE @count int = NULL
	DECLARE @combinedRegionID int = NULL
	DECLARE @combinationMethod tinyint = NULL
	DECLARE @combinedRegion [dbo].[Region] = NULL

	-- Count single regions
	SELECT @count = ISNULL(COUNT(*), 0)
	FROM Footprint f
	INNER JOIN FootprintRegion r
		ON r.FootprintID = f.ID AND r.Type = 0
	WHERE f.ID = @FootprintID

	IF @count = 0 BEGIN
		-- no remaining region, delete cache if any

		DELETE FootprintRegion
		WHERE FootprintID = @FootprintID
		      AND Type = 1

		-- TODO: delete HTM

		UPDATE Footprint
		SET CombinedRegionID = 0
	END ELSE IF @count = 1 BEGIN
		-- one remaining region, delete cache if any
		-- and use one region as combined region

		DELETE FootprintRegion
		WHERE FootprintID = @FootprintID
		      AND Type = 1

		-- TODO: delete HTM

		UPDATE Footprint
		SET CombinedRegionID = r.ID
		FROM Footprint f
		INNER JOIN FootprintRegion r
			ON r.FootprintID = f.ID
	END ELSE BEGIN
		-- recompute entire combined region

		-- Load combination method
		SELECT @combinationMethod = f.CombinationMethod
		FROM Footprint f
		WHERE f.ID = @FootprintID

		IF @combinationMethod = 1 BEGIN
			-- UNION

			SELECT @combinedRegion = region.UnionEvery(Region)
			FROM FootprintRegion r
			WHERE r.FootprintID = @FootprintID
				AND r.Type = 0

		END ELSE IF @combinationMethod = 2 BEGIN
			-- INTERSECT

			SELECT @combinedRegion = region.IntersectEvery(Region)
			FROM FootprintRegion r
			WHERE r.FootprintID = @FootprintID
				AND r.Type = 0
		END ELSE THROW 51000, 'Invalid combination method.', 1;

		-- Save combined region
		INSERT FootprintRegion
			([FootprintID], [Name], [FillFactor], [Type], [Region], [Thumbnail])
		VALUES
			(@FootprintID, 'combined', 1.0, 1, @combinedRegion.ToBinary(), NULL)

		SET @combinedRegionID = @@IDENTITY

		UPDATE Footprint
		SET CombinedRegionID = @combinedRegionID

		-- TODO: Add HTM indexing
	END

GO