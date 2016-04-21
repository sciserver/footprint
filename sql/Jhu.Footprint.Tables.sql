IF (OBJECT_ID('[dbo].[Footprint]') IS NOT NULL)
DROP TABLE [dbo].[Footprint]
GO

IF (OBJECT_ID('[dbo].[FootprintFolder]') IS NOT NULL)
DROP TABLE [dbo].[FootprintFolder]
GO

CREATE TABLE [dbo].[FootprintFolder]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Owner] [nvarchar](250) NOT NULL,
	[FootprintID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Comments] [nvarchar](max) NOT NULL,
	[__acl] [varbinary](1024) NOT NULL,
	CONSTRAINT [PK_FootprintFolder] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO

CREATE TABLE [dbo].[Footprint]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FolderID] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL, 
	[FillFactor] [float] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Region] [varbinary](max) NULL,
	[Thumbnail] [varbinary](max) NULL,
	CONSTRAINT [PK_Footprint] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	),
	CONSTRAINT FK_FootprintFolder_ID FOREIGN KEY (FolderID) 
	REFERENCES FootprintFolder (ID)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO




