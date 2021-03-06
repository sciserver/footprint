CREATE TABLE [dbo].[Footprint]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](256) NOT NULL,
	[Owner] [varchar](250) NOT NULL,
	[CombinedRegionID] [int] NOT NULL,
	[CombinationMethod] [tinyint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Comments] [nvarchar](max) NOT NULL,
	[__acl] [varbinary](1024) NOT NULL,

	CONSTRAINT [PK_Footprint] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	),
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE UNIQUE INDEX IX_Footprint_Name ON [dbo].[Footprint]
(
	[Owner],
	[Name]
)

GO

CREATE TABLE [dbo].[FootprintRegion]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FootprintID] [int] NOT NULL,
	[Name] [varchar](256) NOT NULL, 
	[FillFactor] [float] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Region] [varbinary](max) NULL,
	[ImageThumbnail] [varbinary](max) NULL,
	[ImagePreview] [varbinary](max) NULL,

	CONSTRAINT [PK_FootprintRegion] PRIMARY KEY CLUSTERED
	(
		[ID] ASC
	),

	CONSTRAINT FK_Footprint_ID FOREIGN KEY (FootprintID) 
	REFERENCES Footprint (ID)
	ON DELETE CASCADE
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE UNIQUE INDEX IX_FootprintRegion_Name ON [dbo].[FootprintRegion]
(
	[FootprintID],
	[Name]
)
 
GO

CREATE TABLE [dbo].[FootprintRegionHtm]
(
	[RegionID] int NOT NULL,
	[HtmIDStart] bigint NOT NULL,
	[HtmIDEnd] bigint NOT NULL,
	[Partial] bit NOT NULL,

	-- TODO: add PK and reverse index

	CONSTRAINT FK_FootprintRegion_ID FOREIGN KEY (RegionID)
	REFERENCES FootprintRegion (ID)
	ON DELETE CASCADE
)