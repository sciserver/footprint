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
	@FolderType tinyint,
	@FolderId bigint,
	@Comment nvarchar(max),

	@NewID bigint OUTPUT

AS
	INSERT Footprint
		(Name, [User], [Public], DateCreated,  [FillFactor],
		FolderType, FolderId, Comment)
	VALUES
		(@Name, @User, @Public, GETDATE(), @FillFactor,
		@FolderType, @FolderId, @Comment)

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
	@FolderType tinyint,
	@FolderId bigint,
    @Comment nvarchar(max)
AS
	UPDATE Footprint
	SET Name = @Name, 
		[Public] = @Public,
		[FillFactor] = @FillFactor,
		FolderType = @FolderType,
		FolderID = @FolderId,
		Comment = @Comment
	WHERE
		FootprintID = @FootprintID AND [User] = @User 
	RETURN @@ROWCOUNT
GO

/****** Object:  StoredProcedure [fps].[spDeleteFootprintFolder]  ******/
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

/****** Object:  StoredProcedure [fps].[spGetFootprintFolder]  ******/

IF (OBJECT_ID('[fps].[spGetFootprint]') IS NOT NULL)
	DROP PROC [fps].[spGetFootprint]
GO

CREATE PROC [fps].[spGetFootprint]
		@User nvarchar(250),
		@FootprintId bigint
AS
	SELECT * FROM Footprint
	WHERE
		FootprintID = @FootprintID
		AND ([User] = @User OR [Public] > 0)
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
    @Type tinyint,
    @Public tinyint,
    @Comment nvarchar(max)
AS
    UPDATE FootprintFolder
	SET Name = @Name, 
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