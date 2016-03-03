/***** INSERT -OOTPRINTFOLDER INIT VALUES FOR TEST *****/

USE [Footprint]

INSERT INTO Footprint ( Name, [User], [Public], DateCreated, [FillFactor], FootprintType, FolderID, Comment, RegionBinary, Thumbnail)
SELECT Name, [User], [Public], DateCreated, [FillFactor], FootprintType, FolderID, Comment, RegionBinary, Thumbnail
FROM Footprint_Init;
GO

INSERT INTO FootprintFolder ( Name, FootprintID, [User], [Type], [Public], DateCreated, DateModified, Comment)
SELECT Name, FootprintID, [User], [Type], [Public], DateCreated, DateModified, Comment 
FROM FootprintFolder_Init;
GO