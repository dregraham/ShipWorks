DECLARE 
	@archiveSetID uniqueidentifier,
	@thisDb nvarchar(255)
	
SELECT @archiveSetID = ArchiveSetID
FROM {MASTERDATABASE}.dbo.ArchiveSets
WHERE DbName = db_Name()

SELECT COUNT(*) FROM {MASTERDATABASE}.dbo.ArchiveLogs WHERE ArchiveSetID = @archiveSetID