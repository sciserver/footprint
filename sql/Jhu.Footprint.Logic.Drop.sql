IF OBJECT_ID('[fps].[spUpdateCombinedRegion]') IS NOT NULL
DROP PROC [fps].[spUpdateCombinedRegion]

GO

IF SCHEMA_ID('fps') IS NOT NULL
DROP SCHEMA fps

GO