using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.General;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Helper class for shrinking a SQL Server database
    /// </summary>
    public static class SqlShrinkDatabase
    {
        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        public static void ShrinkDatabase()
        {
            // Attach to the connection
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // Perform the shrink
                SqlCommand calculationCmd = con.CreateCommand();
                calculationCmd.CommandText = ShrinkDbSql;
                calculationCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The TSQL to shrink the db
        /// </summary>
        private static string ShrinkDbSql
        {
            get 
            { 
                return @"   declare @DatabaseName nvarchar(255)
                            declare @DataFileName nvarchar(255)
                            declare @LogFileName nvarchar(255)
                            declare @BackupToFileName nvarchar(255)
                            declare @BackupDisplayName nvarchar(255)
                            declare @IsRecoveryModeFull bit
                            declare @FullDBBackupExists bit
                            set @FullDBBackupExists = 0

                            SET @DatabaseName = DB_NAME() 

                            SELECT @FullDBBackupExists = 1 where exists
                            (  select bmf.physical_device_name
                               FROM msdb..backupmediafamily bmf 
                               INNER JOIN msdb..backupset bms ON bmf.media_set_id = bms.media_set_id 
                               INNER JOIN master..sysdatabases sd ON bms.database_name = sd.name 
                               AND bms.backup_start_date = (SELECT MAX(backup_start_date) FROM [msdb]..[backupset] b2 
                                                               WHERE bms.database_name = b2.database_name AND b2.type = 'D')
                               WHERE sd.name = @DatabaseName
                            )

                            set @BackupDisplayName =  @DatabaseName + N'-Transaction Log  Backup'

                            select @IsRecoveryModeFull = 
		                            case recovery_model
			                            when '1' then 1
			                            else 0
		                            end
                               FROM sys.databases
                                  WHERE name = @DatabaseName 
	  
                            set @BackupToFileName = N'' + @DatabaseName + '_ShrinkDbTask_log.bak'

                            select @DataFileName = mf.name
                            from sys.master_files mf, sys.databases d
                            where d.database_id = mf.database_id
                            and d.name = @DatabaseName and mf.type = 0

                            select @LogFileName = mf.name
                            from sys.master_files mf, sys.databases d
                            where d.database_id = mf.database_id
                            and d.name = @DatabaseName and mf.type = 1

                            if @IsRecoveryModeFull = 1 and @FullDBBackupExists = 1
                            begin
	                            print '1st log Back up of ' + @DatabaseName + ' to ' + @BackupToFileName
	                            BACKUP LOG @DatabaseName 
	                            TO  DISK = @BackupToFileName 
	                            WITH NOFORMAT, INIT,  NAME = @BackupDisplayName, SKIP, NOREWIND, NOUNLOAD,  STATS = 10
                            end

                            DBCC SHRINKDATABASE (@DatabaseName, 1);

                            DBCC SHRINKFILE (@DataFileName , 0, TRUNCATEONLY)

                            DBCC SHRINKFILE (@LogFileName , 0, TRUNCATEONLY)


                            DBCC SHRINKDATABASE (@DatabaseName, 1);

                            if @IsRecoveryModeFull = 1 and @FullDBBackupExists = 1
                            begin
	                            print '2nd log Back up of ' + @DatabaseName + ' to ' + @BackupToFileName
	                            BACKUP LOG @DatabaseName 
	                            TO  DISK = @BackupToFileName 
	                            WITH NOFORMAT, INIT,  NAME = @BackupDisplayName, SKIP, NOREWIND, NOUNLOAD,  STATS = 10
                            end

                            DBCC SHRINKDATABASE (@DatabaseName, 1)"; 
            }
        }
    }
}
