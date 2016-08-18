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

CREATE FUNCTION [fps].[FindFootprintRegionEq]
(	
	@ra float,
	@dec float
)
RETURNS TABLE 
AS
RETURN 
(
	WITH q AS
	(
		SELECT RegionID
		FROM FootprintRegionHtm htm
		WHERE htmid.FromEq(@ra, @dec) BETWEEN htmIDStart AND htmIDEnd AND [partial] = 0

		UNION

		SELECT RegionID
		FROM FootprintRegionHtm htm
		INNER JOIN FootprintRegion r ON r.ID = htm.RegionID
		WHERE htmid.FromEq(@ra, @dec) BETWEEN htmIDStart AND htmIDEnd AND [partial] = 1
			AND region.ContainsEq(r.Region, @ra, @dec) = 1
	)
	SELECT RegionID
	FROM q
)

GO

CREATE FUNCTION [fps].[FindFootprintRegionIntersectHtm]
(	
	@region varbinary(max)
)
RETURNS TABLE 
AS
RETURN 
(
	WITH cover AS
	(
		SELECT * FROM htm.CoverAdvanced(@region, 0, 0.9, 2)
	),
	q AS
	(
		SELECT htm.*
		FROM cover
		INNER LOOP JOIN FootprintRegionHtm htm ON
			htm.htmIDStart BETWEEN cover.htmIDStart AND cover.htmIDEnd

		UNION

		SELECT htm.*
		FROM cover
		INNER LOOP JOIN FootprintRegionHtm htm ON
			(htm.htmIDStart = cover.htmIDStart OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFFFFC OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFFFF0 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFFFC0 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFFF00 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFFC00 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFF000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFFC000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFF0000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFFC0000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFF00000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFFC00000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFF000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFFC000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFF0000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFFC0000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFF00000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFFC00000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFF000000000 OR
			htm.htmIDStart = cover.htmIDStart & 0xFFFFFFC000000000)
			AND htm.htmIDEnd >= cover.htmIDStart
	)
	SELECT q.*
	FROM q
)

GO

CREATE FUNCTION [fps].[FindFootprintRegionIDIntersect]
(	
	@region varbinary(max)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT RegionID
	FROM fps.FindFootprintRegionIntersectHtm(@region) htm
)

GO

CREATE FUNCTION [fps].[FindFootprintRegionIntersect]
(	
	@region varbinary(max)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT r.* 
	FROM fps.FindFootprintRegionIDIntersect(@region) q
	INNER JOIN FootprintRegion r
		ON r.ID = q.RegionID
)

GO

CREATE FUNCTION [fps].[FindFootprintRegionIDContain]
(	
	@region varbinary(max)
)
RETURNS @ret TABLE 
(
	RegionID INT NOT NULL
)
AS
BEGIN 

	DECLARE @htmTemp TABLE
	(
		[regionID] [int] NOT NULL,
		[htmIDStart] [bigint] NOT NULL,
		[htmIDEnd] [bigint] NOT NULL,
		[partial] [bit] NOT NULL
	);

	INSERT @htmTemp
	SELECT * FROM [fps].[FindFootprintRegionIntersectHtm](@region);

	WITH __intersecting AS
	(
		SELECT DISTINCT regionID
		FROM @htmTemp
	),
	__contained AS
	(
		SELECT htm.*
		FROM __intersecting
		INNER LOOP JOIN FootprintRegionHtm htm
			ON __intersecting.regionID = htm.RegionID 

		EXCEPT

		SELECT *
		FROM @htmTemp
	)
	INSERT @ret
		(RegionID)
	SELECT DISTINCT RegionID
		FROM __contained

	RETURN;

END;

GO

CREATE FUNCTION [fps].[FindFootprintRegionContain]
(	
	@region varbinary(max)
)
RETURNS TABLE
AS
RETURN 
(
	SELECT r.*
	FROM fps.FindFootprintRegionIDContain(@region) q
	INNER JOIN FootprintRegion r
		ON r.ID = q.RegionID
)

GO