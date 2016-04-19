USE [footprint]
GO

/***********************************************************************/
/******                         CREATE TABLES                     ******/
/***********************************************************************/

IF (OBJECT_ID('[dbo].[Footprint]') IS NOT NULL)
DROP TABLE [dbo].[Footprint]
GO

CREATE TABLE [dbo].[Footprint]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FolderId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NOT NULL, 
	[FillFactor] [float] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[RegionBinary] [varbinary](max) NULL,
	[Thumbnail] [varbinary](max) NULL,
	CONSTRAINT [PK_Footprint] PRIMARY KEY CLUSTERED
	(
		[FootprintID] ASC
	)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO

IF (OBJECT_ID('[dbo].[FootprintFolder]') IS NOT NULL)
DROP TABLE [dbo].[FootprintFolder]
GO

CREATE TABLE [dbo].[FootprintFolder]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Owner] [nvarchar](250) NOT NULL,
	[FootprintID] [bigint] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Comments] [ntext] NOT NULL,
	CONSTRAINT [PK_FootprintFolder] PRIMARY KEY CLUSTERED
	(
		[FolderID] ASC
	)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO






