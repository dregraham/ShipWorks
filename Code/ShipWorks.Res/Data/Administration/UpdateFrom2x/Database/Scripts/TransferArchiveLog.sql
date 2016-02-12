DECLARE 
	@workCounter int,
	@archiveSetID uniqueidentifier
	
-- source table variables
DECLARE
    -- @MasterDatabase will be generated at runtime
    -- @IsArchive will be generated at runtime    
    @sArchiveLogID int, 
    @sOperationID uniqueidentifier, 
    @sStoreID int, 
    @sType smallint, 
    @sSourceKey int, 
    @sTargetKey int, 
    @sSummary nvarchar, 
    @sDataDate datetime, 
    @sExtra nvarchar(max), 
    @sArchiveSetID uniqueidentifier, 
    @sArchiveDate datetime 

-- target table variables
DECLARE
    @tArchiveLogID int, 
    @tOperationID uniqueidentifier, 
    @tStoreID int, 
    @tType smallint, 
    @tSourceKey int, 
    @tTargetKey int, 
    @tSummary nvarchar, 
    @tDataDate datetime, 
    @tExtra nvarchar(max), 
    @tArchiveSetID uniqueidentifier, 
    @tArchiveDate datetime 

SELECT @archiveSetID = ArchiveSetID
FROM {MASTERDATABASE}.dbo.ArchiveSets
WHERE DbName = db_Name()

-- if the archive set doesn't exist here, create it
IF NOT EXISTS(
  SELECT 1 FROM dbo.ArchiveSets
  WHERE ArchiveSetID = @archiveSetID)
BEGIN
    -- copy the row over to the archive database
	INSERT INTO ArchiveSets (ArchiveSetID, ArchiveSetName, DbName, [Description], CreateDate)
		SELECT ArchiveSetID, ArchiveSetName, DbName, [Description], CreateDate FROM {MASTERDATABASE}.dbo.ArchiveSets WHERE ArchiveSetID = @archiveSetID
END

SET IDENTITY_INSERT dbo.ArchiveLogs ON

-- Track Progress
SET @workCounter = 0

-- the cursor for cycling through the source table
DECLARE workCursor CURSOR FORWARD_ONLY FOR
SELECT TOP 1000
    [ArchiveLogID],
    [OperationID],
    [StoreID],
    [Type],
    [SourceKey],
    [TargetKey],
    [Summary],
    [DataDate],
    [Extra],
    [ArchiveSetID],
    [ArchiveDate]
    FROM {MASTERDATABASE}.dbo.ArchiveLogs
    WHERE ArchiveSetID = @archiveSetID

-- open the source table cursor
OPEN workCursor

-- populate source table variables from the source cursor
FETCH NEXT FROM workCursor
INTO
    @sArchiveLogID,
    @sOperationID,
    @sStoreID,
    @sType,
    @sSourceKey,
    @sTargetKey,
    @sSummary,
    @sDataDate,
    @sExtra,
    @sArchiveSetID,
    @sArchiveDate
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @workCounter = @workCounter + 1
    
    -- copy values from the old to new tables
    SET @tArchiveLogID = @sArchiveLogID
    SET @tOperationID = @sOperationID
    SET @tStoreID = @sStoreID  /* StoreIDs were consistent between SW and Archive databases, no translation needed */
    SET @tType = @sType
    SET @tSourceKey = @sSourceKey
    SET @tTargetKey = @sTargetKey
    SET @tSummary = @sSummary
    SET @tDataDate = @sDataDate
    SET @tExtra = @sExtra
    SET @tArchiveSetID = @archiveSetID
    SET @tArchiveDate = @sArchiveDate
    
    	
    INSERT INTO dbo.ArchiveLogs  (
    [ArchiveLogID],
    [OperationID],
    [StoreID],
    [Type],
    [SourceKey],
    [TargetKey],
    [Summary],
    [DataDate],
    [Extra],
    [ArchiveSetID],
    [ArchiveDate]
            )
    VALUES
            (
    @tArchiveLogID,
    @tOperationID,
    @tStoreID,
    @tType,
    @tSourceKey,
    @tTargetKey,
    @tSummary,
    @tDataDate,
    @tExtra,
    @tArchiveSetID,
    @tArchiveDate
            )             
            
    -- delete the original
    DELETE FROM {MASTERDATABASE}.dbo.ArchiveLogs WHERE ArchiveLogID = @sArchiveLogID

-- fetch next row from source table
FETCH NEXT FROM workCursor
INTO
    @sArchiveLogID,
    @sOperationID,
    @sStoreID,
    @sType,
    @sSourceKey,
    @sTargetKey,
    @sSummary,
    @sDataDate,
    @sExtra,
    @sArchiveSetID,
    @sArchiveDate
END
CLOSE workCursor
DEALLOCATE workCursor

-- data migration "protocol" demands we return the number of rows/work completed
SELECT @workCounter as WorkCompleted