USE [Footprint]
-- CREATE SCHEMA --
/*
CREATE SCHEMA [fps]
GO
*/

/***********************************************************************/
/******                     FOOTPRINT PROCEDURES                  ******/
/***********************************************************************/ 

/****** Object:  StoredProcedure [fps].[spCreateFootprint]  ******/
IF (OBJECT_ID('[fps].[spCreateFootprint]') IS NOT NULL)
	DROP PROC [fps].[spCreateFootprint]
GO

CREATE PROC [fps].[spCreateFootprint]
	@Name nvarchar(256),
	@User nvarchar(250),
	@Public tinyint,
	@FillFactor float,
	@FootprintType smallint,
	@FolderId bigint,
	@Comment ntext,
	@RegionBinary varbinary(max),

	@NewID bigint OUTPUT

AS
	INSERT Footprint
		(Name, [User], [Public], DateCreated,  [FillFactor],
		FootprintType, FolderId, Comment, RegionBinary)
	VALUES
		(@Name, @User, @Public, GETDATE(), @FillFactor,
		@FootprintType, @FolderId, @Comment, @RegionBinary)

	SET @NewID = @@IDENTITY

	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spModifyFootprint]  ******/
IF (OBJECT_ID('[fps].[spModifyFootprint]') IS NOT NULL)
	DROP PROC [fps].[spModifyFootprint]

GO

CREATE PROC [fps].[spModifyFootprint]
	@FootprintID bigint,
    @Name nvarchar(256),
    @User nvarchar(250),
    @Public tinyint,
	@FillFactor float,
	@FootprintType smallint,
	@FolderId bigint,
    @Comment ntext,
	@RegionBinary varbinary(max)
AS
	UPDATE Footprint
	SET Name = @Name, 
		[Public] = @Public,
		[FillFactor] = @FillFactor,
		FootprintType = @FootprintType,
		FolderID = @FolderId,
		Comment = @Comment,
		RegionBinary = @RegionBinary
	WHERE
		FootprintID = @FootprintID AND [User] = @User 
	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spDeleteFootprint]  ******/
IF (OBJECT_ID('[fps].[spDeleteFootprint]') IS NOT NULL)
	DROP PROC [fps].[spDeleteFootprint]
GO

CREATE PROC [fps].[spDeleteFootprint]
	@FootprintID bigint,
    @User nvarchar(250)
AS
	DELETE Footprint
	WHERE
		FootprintID = @FootprintID AND [User] = @User 
	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spGetFootprint]  ******/

IF (OBJECT_ID('[fps].[spGetFootprint]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprint]
GO

CREATE PROC [fps].[spGetFootprint]
	@User nvarchar(250),
	@FootprintId bigint
AS
	SELECT F.FootprintID AS FootprintID, F.Name AS Name, F.[User] AS [User], F.[Public] AS [Public], 
	F.DateCreated AS [DateCreated], F.[FillFactor] AS [FillFactor], F.FootprintType AS FootprintType, 
	F.FolderId AS FolderId, F.Comment AS Comment, F.RegionBinary AS RegionBinary
	FROM Footprint AS F 
	
	WHERE
		F.FootprintID = @FootprintID
		AND (F.[User] = @User OR F.[Public] > 0)
GO




/****** Object:  StoredProcedure [fps].[spGetFolderId]  ******/

IF (OBJECT_ID('[fps].[spGetFootprintId]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprintId]
GO

CREATE PROC [fps].[spGetFootprintId]
	@User nvarchar(250),
	@FolderName nvarchar(256),
	@FootprintName nvarchar(256),

	@footprintId bigint OUTPUT
AS
	DECLARE @folderId bigint;
	SET @folderId = (SELECT folderId FROM FootprintFolder
	WHERE
		Name = @FolderName
		AND [User] = @User)
	SET @footprintId = (SELECT footprintId FROM Footprint
	WHERE
		Name = @FootprintName
		AND [User] = @User)
GO


/****** Object:  StoredProcedure [fps].[spIsDuplicateFootprintName]  ******/

IF (OBJECT_ID('[fps].[spIsDuplicateFootprintName]') IS NOT NULL)
	DROP PROC [fps].[spIsDuplicateFootprintName]
GO

CREATE PROC [fps].[spIsDuplicateFootprintName]
	@User nvarchar(250),
	@FootprintId bigint,
	@FolderId bigint,
	@FootprintName nvarchar(256),

	@Match int OUTPUT

AS
	SET @Match = (SELECT COUNT(*) FROM Footprint
	WHERE
		Name = @FootprintName
		AND [User] = @User
		AND FolderId = @FolderId
		AND FootprintID != @FootprintId) 
GO

/****** Object:  StoredProcedure [fps].[spGetFootprintsByFolderId]  ******/
IF (OBJECT_ID('[fps].[spGetFootprintsByFolderId]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprintsByFolderId]
GO

CREATE PROC [fps].[spGetFootprintsByFolderId]
	@FolderId bigint,
	@User nvarchar(250)
AS
	SELECT F.FootprintID AS FootprintID, F.Name AS Name, F.[User] AS [User], F.[Public] AS [Public],
	F.DateCreated AS [DateCreated], F.[FillFactor] AS [FillFactor],	F.FootprintType AS FootprintType, 
	F.FolderId AS FolderId, F.Comment AS Comment, F.RegionBinary AS RegionBinary
	FROM Footprint AS F 
	WHERE
		F.FolderId = @FolderId
		AND F.[User]= @User
		AND F.FootprintType != 1
	ORDER BY Name
GO


/****** Object:  StoredProcedure [fps].[spGetFolderFootprint] ******/
IF (OBJECT_ID('[fps].[spGetFolderFootprint]') IS NOT NULL)
	DROP PROC [fps].[spGetFolderFootprint]
GO

CREATE PROC [fps].[spGetFolderFootprint]
	@FolderID bigint
AS
	DECLARE @FootprintID bigint
	
	SELECT FootprintID = @FootprintID FROM FootprintFolder WHERE FolderID = @FolderID

	IF(@FootprintID IS NULL) -- no cache, single region
	BEGIN
		SELECT TOP 1 F.FootprintID AS FootprintID, F.Name AS Name, F.[User] AS [User], F.[Public] AS [Public],
		F.DateCreated AS [DateCreated], F.[FillFactor] AS [FillFactor],	F.FootprintType AS FolderType, 
		F.FolderId AS FolderId, F.Comment AS Comment, F.RegionBinary AS RegionBinary
		FROM Footprint AS F 
		WHERE F.FolderID = @FolderID
	END
	ELSE
	BEGIN
		SELECT F.FootprintID AS FootprintID, F.Name AS Name, F.[User] AS [User], F.[Public] AS [Public],
		F.DateCreated AS [DateCreated], F.[FillFactor] AS [FillFactor],	F.FootprintType AS FolderType, 
		F.FolderId AS FolderId, F.Comment AS Comment, F.RegionBinary AS RegionBinary
		FROM Footprint AS F 
		WHERE F.FootprintID = @FootprintID
	END
GO

/***********************************************************************/
/******                 FOOTPRINT FOLDER PROCEDURES               ******/ 
/***********************************************************************/

/****** Object:  StoredProcedure [fps].[spCreateFootprintFolder]  ******/

IF (OBJECT_ID('[fps].[spCreateFootprintFolder]') IS NOT NULL)
DROP PROC [fps].[spCreateFootprintFolder]
GO

CREATE PROC [fps].[spCreateFootprintFolder]
	@Name nvarchar(256),
	@User nvarchar(250),
	@Type tinyint,
	@Public tinyint,
	@Comment nvarchar(max),

	@NewID bigint OUTPUT
AS
	INSERT FootprintFolder
		([Type],  [User], [Public], 
			DateCreated, DateModified, Name, Comment)
	VALUES
		(@Type, @User, @Public,
		GETDATE(), GETDATE(), @Name, @Comment)

	SET @NewID = @@IDENTITY
	RETURN @@ROWCOUNT
GO


/****** Object:  StoredProcedure [fps].[spModifyFootprintFolder]  ******/
IF (OBJECT_ID('[fps].[spModifyFootprintFolder]') IS NOT NULL)
	DROP PROC [fps].[spModifyFootprintFolder]

GO

CREATE PROC [fps].[spModifyFootprintFolder]
	@FolderID bigint,
    @Name nvarchar(256),
    @User nvarchar(250),
	@FootprintId bigint,
    @Type tinyint,
    @Public tinyint,
    @Comment nvarchar(max)
AS
    UPDATE FootprintFolder
	SET Name = @Name, 
		FootprintId = @FootprintId,
		[Type] = @Type,
		[Public] = @Public,
		DateModified = GETDATE(),
		Comment = @Comment
	WHERE
		FolderID = @FolderID AND [User] = @User 
	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spDeleteFootprintFolder]  ******/
IF (OBJECT_ID('[fps].[spDeleteFootprintFolder]') IS NOT NULL)
	DROP PROC [fps].[spDeleteFootprintFolder]
GO

CREATE PROC [fps].[spDeleteFootprintFolder]
	@FolderID bigint,
    @User nvarchar(250)
AS
	DECLARE @CountAll int
	DELETE FootprintFolder
	WHERE
		FolderID = @FolderID AND [User] = @User


		 
	SET @CountAll = @@ROWCOUNT
	DELETE Footprint
	WHERE 
		FolderID = @FolderID AND [User] = @User

	RETURN @CountAll  + @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spGetFootprintFolder]  ******/

IF (OBJECT_ID('[fps].[spGetFootprintFolder]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprintFolder]
GO

CREATE PROC [fps].[spGetFootprintFolder]
       @User nvarchar(250),
       @FolderId bigint
AS
       SELECT * FROM FootprintFolder
       WHERE
             FolderID = @FolderID
             AND ([User] = @User OR [Public] > 0)
 
GO

/****** Object:  StoredProcedure [fps].[spFindFootprintFolder]  ******/
IF (OBJECT_ID('[fps].[spFindFootprintFolderByName]') IS NOT NULL)
	DROP PROC [fps].[spFindFootprintFolderByName]
GO

CREATE PROC [fps].[spFindFootprintFolderByName]
	@Name nvarchar(256),
	@User nvarchar(250),
	@Source int
AS
	SELECT * FROM FootprintFolder
	WHERE
		Name LIKE '%' + @Name + '%'
		AND
		-- public
		((@Source & 1) > 0 
		AND [Public] > 0)
		OR
		-- private
		(@User = [User] AND (@Source & 2) > 0)
	ORDER BY Name
GO

/****** Object:  StoredProcedure [fps].[spFindFootprintFolder]  ******/
IF (OBJECT_ID('[fps].[spCountFootprintFolderByName]') IS NOT NULL)
	DROP PROC [fps].[spCountFootprintFolderByName]
GO

CREATE PROC [fps].[spCountFootprintFolderByName]
	@Name nvarchar(256),
	@User nvarchar(250),
	@Source int
	
AS
	DECLARE @Count int;
	SET @Count = (SELECT Count(*) FROM FootprintFolder
	WHERE
		Name LIKE '%' + @Name + '%'
		AND
		-- public
		((@Source & 1) > 0 
		AND [Public] > 0)
		OR
		-- private
		(@User = [User] AND (@Source & 2) > 0))
	RETURN @Count
GO

/****** Object:  StoredProcedure [fps].[spGetFootrpintFolderNameById]  ******/
IF (OBJECT_ID('[fps].[spGetFootprintFolderNameById]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprintFolderNameById]
GO

CREATE PROC [fps].[spGetFootprintFolderNameById]
	@FolderID bigint,

	@FolderName nvarchar(256) OUTPUT
AS
	SET @FolderName = 6
	(
	SELECT Name From FootprintFolder
	WHERE @FolderID = FolderID
	)
	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spGetFootprintFolderId]  ******/

IF (OBJECT_ID('[fps].[spGetFootprintFolderId]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprintFolderId]
GO

CREATE PROC [fps].[spGetFootprintFolderId]
	@User nvarchar(250),
	@FolderName nvarchar(256),

	@folderId bigint OUTPUT
AS
	SET @folderId = (SELECT [folderId] FROM FootprintFolder
	WHERE
		Name = @FolderName
		AND [User] = @User)
GO


/****** Object:  StoredProcedure [fps].[spIsDuplicateFootprintFolderName]  ******/

IF (OBJECT_ID('[fps].[spIsDuplicateFootprintFolderName]') IS NOT NULL)
	DROP PROC [fps].[spIsDuplicateFootprintFolderName]
GO

CREATE PROC [fps].[spIsDuplicateFootprintFolderName]
	@User nvarchar(250),
	@FolderId bigint,
	@FolderName nvarchar(256),

	@Match int OUTPUT

AS
	SET @Match = (SELECT COUNT(*) FROM FootprintFolder
	WHERE
		Name = @FolderName
		AND [User] = @User
		AND FolderId != @FolderId) 
GO