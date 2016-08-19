IF OBJECT_ID('[fps].[FindFootprintRegionContain]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionContain]

GO

IF OBJECT_ID('[fps].[FindFootprintRegionIDContain]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionIDContain]

GO

IF OBJECT_ID('[fps].[FindFootprintRegionIntersect]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionIntersect]

GO

IF OBJECT_ID('[fps].[FindFootprintRegionIDIntersect]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionIDIntersect]

GO

IF OBJECT_ID('[fps].[FindFootprintRegionIntersectHtm]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionIntersectHtm]

GO

IF OBJECT_ID('[fps].[FindFootprintRegionEq]') IS NOT NULL
DROP FUNCTION [fps].[FindFootprintRegionEq]

GO

IF OBJECT_ID('[fps].[spComputeHtmCover]') IS NOT NULL
DROP PROC [fps].[spComputeHtmCover]

GO


IF OBJECT_ID('[fps].[spSaveRegion]') IS NOT NULL
DROP PROC [fps].[spSaveRegion]

GO

IF OBJECT_ID('[fps].[spUpdateCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spUpdateCombinedRegion]

GO

IF OBJECT_ID('[fps].[spRefreshCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spRefreshCombinedRegion]

GO

IF SCHEMA_ID('fps') IS NOT NULL
DROP SCHEMA fps

GO