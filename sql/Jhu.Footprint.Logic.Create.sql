CREATE SCHEMA [fps]
GO

CREATE PROC [fps].[spSaveRegion]
(
	@RegionID int,
	@Region varbinary(max)
)
AS
	-- Check if this is a cache region or individual region
	-- Check if old region is null or not
	DECLARE @existing bit
	DECLARE @footprintID int
	DECLARE @type tinyint
	
	SELECT 
		@footprintID = FootprintID,
		@type = Type,
		@existing =
			CASE
				WHEN region IS NULL THEN 0
				ELSE 1
			END
	FROM FootprintRegion r
	WHERE r.ID = @RegionID;

	-- Save region
	UPDATE FootprintRegion
	SET region = @region
	FROM FootprintRegion r
	WHERE r.ID = @RegionID;

	-- Generate HTM
	EXEC [fps].[spComputeHtmCover] @RegionID

	IF @type = 0 BEGIN
		IF @existing = 0 BEGIN
			-- Update
			EXEC [fps].[spUpdateCombinedRegion] @footprintID, @RegionID
		END ELSE BEGIN
			-- Refresh
			EXEC [fps].[spRefreshCombinedRegion] @footprintID
		END
	END

GO

CREATE PROC [fps].[spUpdateCombinedRegion]
(
	@FootprintID int,
	@RegionID int
)
AS
	PRINT 'Executing [fps].[spUpdateCombinedRegion]...'

	DECLARE @count int = NULL
	DECLARE @combinedRegionID int = NULL
	DECLARE @combinationMethod tinyint = NULL
	DECLARE @combinedRegion [dbo].[Region] = NULL
	
	-- Count single regions
	SELECT @count = ISNULL(COUNT(*), 0)
	FROM Footprint f
	INNER JOIN FootprintRegion r
		ON r.FootprintID = f.ID AND r.Type = 0
	WHERE f.ID = @FootprintID;

	PRINT 'Numer of single regions: ' + CAST(@count AS varchar(5));

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

			PRINT 'Creating new combined region';

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

			-- Generate HTM
			EXEC [fps].[spComputeHtmCover] @combinedRegionID

			UPDATE Footprint
			SET CombinedRegionID = @combinedRegionID
		END ELSE BEGIN
			-- Update combined region with the very last one

			PRINT 'Updating combined region';

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

			-- Generate HTM
			EXEC [fps].[spComputeHtmCover] @combinedRegionID
		END
	END

GO

CREATE PROC [fps].[spRefreshCombinedRegion]
(
	@FootprintID int
)
AS
	PRINT 'Executing [fps].[spRefreshCombinedRegion]...'

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

		PRINT 'No remaining region, deleting cache';

		DELETE FootprintRegion
		WHERE FootprintID = @FootprintID
		      AND Type = 1

		UPDATE Footprint
		SET CombinedRegionID = 0

	END ELSE IF @count = 1 BEGIN
		-- one remaining region, delete cache if any
		-- and use one region as combined region

		PRINT 'One remaining region, deleting cache';

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

		PRINT 'More remaining regions, recomputing cache';

		-- Load combination method
		SELECT @combinationMethod = f.CombinationMethod,
			   @combinedRegionID = f.CombinedRegionID
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
		IF @combinedRegionID > 0 BEGIN

			PRINT 'Updating cache region';

			UPDATE FootprintRegion
			SET Region = @combinedRegion.ToBinary()
			WHERE ID = @combinedRegionID

		END ELSE BEGIN

			PRINT 'Creating new cache region';
		
			INSERT FootprintRegion
				([FootprintID], [Name], [FillFactor], [Type], [Region], [Thumbnail])
			VALUES
				(@FootprintID, 'combined', 1.0, 1, @combinedRegion.ToBinary(), NULL)

			SET @combinedRegionID = @@IDENTITY

			UPDATE Footprint
			SET CombinedRegionID = @combinedRegionID

		END

		-- Generate HTM
		EXEC [fps].[spComputeHtmCover] @combinedRegionID
	END

GO

CREATE PROC [fps].[spComputeHtmCover]
	@RegionID int
AS
	DECLARE @region varbinary(max)

	SELECT @region = region
	FROM FootprintRegion
	WHERE ID = @RegionID

	DELETE FootprintRegionHTM
	WHERE RegionID = @RegionID

	INSERT FootprintRegionHTM
		(RegionID, HtmIDStart, HtmIDEnd, Partial)
	SELECT
		@RegionID, HtmIDStart, HtmIDEnd, Partial
	FROM htm.Cover(@region)

GO