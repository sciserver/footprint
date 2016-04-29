IF OBJECT_ID('[fps].[spUpdateCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spUpdateCombinedRegion]

GO

IF OBJECT_ID('[fps].[spRefreshCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spRefreshCombinedRegion]

GO

IF SCHEMA_ID('fps') IS NOT NULL
DROP SCHEMA fps

GO