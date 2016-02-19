USE [footprint]
GO

/***********************************************************************/
/******                         CREATE TABLES                     ******/
/***********************************************************************/

IF (OBJECT_ID('[dbo].[Footprint]') IS NOT NULL)
DROP TABLE [dbo].[Footprint]
GO

CREATE TABLE [dbo].[Footprint](
       [FootprintID] [bigint] IDENTITY(1,1) NOT NULL,
	   [Name] [nvarchar](256) NOT NULL DEFAULT (''),
       [User] [nvarchar](250) NOT NULL DEFAULT (''),
       [Public] [tinyint] NOT NULL  DEFAULT ((0)),
       [DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
       [FillFactor] [float] NOT NULL  DEFAULT ((1.0)),
       [FootprintType] [tinyint] NOT NULL DEFAULT ((0)),
	   [FolderId] [bigint] NOT NULL DEFAULT((0)),
       [Comment] [ntext] NOT NULL DEFAULT (''),
       [RegionBinary] [varbinary](max) NULL,
       [Thumbnail] [varbinary](max) NULL,
CONSTRAINT [PK_Footprint] PRIMARY KEY CLUSTERED
(
       [FootprintID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO

IF (OBJECT_ID('[dbo].[FootprintFolder]') IS NOT NULL)
DROP TABLE [dbo].[FootprintFolder]
GO

CREATE TABLE [dbo].[FootprintFolder](
       [FolderID] [bigint] IDENTITY(1,1) NOT NULL,
	   [FootprintID] [bigint] NULL,
       [Name] [nvarchar](256) NOT NULL DEFAULT (''),
       [User] [nvarchar](250) NOT NULL DEFAULT (''),
       [Type] [tinyint] NOT NULL,
       [Public] [tinyint] NOT NULL,
       [DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
       [DateModified] [datetime] NOT NULL DEFAULT (getdate()),
       [Comment] [ntext] NOT NULL DEFAULT (''),
CONSTRAINT [PK_FootprintFolder] PRIMARY KEY CLUSTERED
(
       [FolderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO






