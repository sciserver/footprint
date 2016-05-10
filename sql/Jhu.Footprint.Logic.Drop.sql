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