DECLARE @DestinationDatabaseFilesPath nvarchar(400)
DECLARE @SourceDatabaseBackupPathAndFileName nvarchar(400)
DECLARE @SourceDatabaseName nvarchar(400)
DECLARE @SourceDataName nvarchar(400)
DECLARE @SourceLogName nvarchar(400)
DECLARE @DestinationDatabaseDataPathAndFileName nvarchar(400)
DECLARE @DestinationDatabaseLogPathAndFileName nvarchar(400)
DECLARE @ErrorMessage nvarchar(255)
DECLARE @DestinationDatabaseName nvarchar(400)

SELECT @DestinationDatabaseFilesPath = substring(physical_name, 0, LEN(physical_name) - CHARINDEX('\',REVERSE(physical_name)) + 2) FROM sys.database_files WHERE [type] = 0;

SET @SourceDatabaseName = DB_NAME();
SET @DestinationDatabaseName = '%destinationDatabaseName%';
SET @DestinationDatabaseDataPathAndFileName = @DestinationDatabaseFilesPath + '\' + @DestinationDatabaseName + '.mdf'
SET @DestinationDatabaseLogPathAndFileName  = @DestinationDatabaseFilesPath + '\' + @DestinationDatabaseName + '_log.ldf'

SELECT @SourceDataName = [name] FROM sys.database_files WHERE [type] = 0 /* ROWS */
SELECT @SourceLogName  = [name] FROM sys.database_files WHERE [type] = 1 /* LOG */
SELECT @SourceDatabaseBackupPathAndFileName = @DestinationDatabaseFilesPath + '\' + @SourceDatabaseName + CONVERT(NVARCHAR(36), newid()) + '.bak'
SET @SourceDatabaseBackupPathAndFileName = REPLACE(@SourceDatabaseBackupPathAndFileName, '\\', '\')

IF EXISTS(select * from sys.databases where name = @DestinationDatabaseName)
BEGIN
	SET @ErrorMessage = N'The destination database [' + @DestinationDatabaseName + '] already exists.  Please choose a different destination database name.';
	RAISERROR(@ErrorMessage, 16, 1)
END	
ELSE IF EXISTS(select * from sys.master_files where physical_name = @DestinationDatabaseDataPathAndFileName)
BEGIN
	SET @ErrorMessage = N'The destination database data file, ' + @DestinationDatabaseDataPathAndFileName + ' already exists.  Please choose a different destination data file location.';
	RAISERROR(@ErrorMessage, 16, 1)
END	
ELSE IF EXISTS(select * from sys.master_files where physical_name = @DestinationDatabaseLogPathAndFileName)
BEGIN
	SET @ErrorMessage = N'The destination database log file ' + @DestinationDatabaseLogPathAndFileName + ' already exists.  Please choose a different destination log file location.';
	RAISERROR(@ErrorMessage, 16, 1)
END	
ELSE 
BEGIN
	BEGIN TRY
        DECLARE @EditionTypeId sql_variant

        set @EditionTypeId = serverproperty('EditionID')

        /* These are Express or Web editions, so no compression */
        if -1592396055 = @EditionTypeId or -133711905 = @EditionTypeId or 1293598313 = @EditionTypeId
	        BEGIN
				BACKUP DATABASE @SourceDatabaseName
					TO  DISK = @SourceDatabaseBackupPathAndFileName 
					WITH NOFORMAT, INIT,  NAME = N'BackupForArchive', 
					SKIP, NOREWIND, NOUNLOAD, STATS = 10, COPY_ONLY
	        END
        ELSE
	        BEGIN
				BACKUP DATABASE @SourceDatabaseName
					TO  DISK = @SourceDatabaseBackupPathAndFileName 
					WITH NOFORMAT, INIT,  NAME = N'BackupForArchive', 
					SKIP, NOREWIND, NOUNLOAD, COMPRESSION,  STATS = 10, COPY_ONLY;
	        END

		declare @backupSetId as int
		select @backupSetId = position 
			from msdb..backupset 
			where database_name= @SourceDatabaseName
			  and backup_set_id=(select max(backup_set_id) from msdb..backupset where database_name= @SourceDatabaseName )
		if @backupSetId is null begin raiserror(N'Verify failed. Backup information for the database was not found.', 16, 1) end
		RESTORE VERIFYONLY FROM  DISK = @SourceDatabaseBackupPathAndFileName WITH  FILE = @backupSetId,  NOUNLOAD,  NOREWIND;

		RESTORE DATABASE @DestinationDatabaseName
			FROM  DISK = @SourceDatabaseBackupPathAndFileName
			WITH  FILE = 1,  
			MOVE @SourceDataName TO @DestinationDatabaseDataPathAndFileName,  
			MOVE @SourceLogName  TO @DestinationDatabaseLogPathAndFileName,  
			NOUNLOAD,  REPLACE,  STATS = 10;

		DECLARE @version NVARCHAR(20) = CONVERT(VARCHAR(20),SERVERPROPERTY('productversion'));
		IF @version LIKE '14%'
		BEGIN
			EXEC('ALTER DATABASE ' + @DestinationDatabaseName + ' SET TRUSTWORTHY ON')
		END
		
		UPDATE [%destinationDatabaseName%]..[Configuration] SET ArchivalSettingsXml = '%archivalSettingsXml%'

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMsg nvarchar(500)
		DECLARE @ErrorSeverity nvarchar(500)
		DECLARE @ErrorState nvarchar(500)
		
		SET @ErrorMsg = ERROR_MESSAGE();
		SET @ErrorSeverity = ERROR_SEVERITY();
		SET @ErrorState = ERROR_STATE();

		raiserror(@ErrorMsg, @ErrorSeverity, @ErrorState)
	END CATCH

		/*  Delete the backup */
		DECLARE @FileExists int
		EXEC master..xp_FileExist @SourceDatabaseBackupPathAndFileName, @FileExists out
		IF @FileExists = 1
			BEGIN
				execute master.dbo.xp_delete_file 0, @SourceDatabaseBackupPathAndFileName
			END
END