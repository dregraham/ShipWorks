using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Common.Logging;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Manages backups for the SchemaUpgrade
    /// </summary>
    public class DatabaseUpgradeBackupManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpgradeDatabaseSchemaCommandLineOption));
        private const string BackupNameFormat = "{0}_AutomaticUpgradeBackup.bak";
        private readonly string database;
        private readonly string backupPathAndName;

        /// <summary>
        /// constructor
        /// </summary>
        public DatabaseUpgradeBackupManager()
        {
            database = SqlSession.Current.DatabaseName;
            backupPathAndName = Path.Combine(GetBackupPath(), string.Format(BackupNameFormat, database));
        }

        /// <summary>
        /// Create the backup
        /// </summary>
        public TelemetricResult<Result> CreateBackup()
        {
            TelemetricResult<Result> telemetricResult = new TelemetricResult<Result>("Database.Backup");
            telemetricResult.RunTimedEvent("CreateBackupTimeInMilliseconds", () => CreateBackup(database, backupPathAndName));
            
            telemetricResult.SetValue(ValidateBackup(backupPathAndName));

            return telemetricResult;
        }

        /// <summary>
        /// ensure the backup got created
        /// </summary>
        private static Result ValidateBackup(string fileName)
        {
            using (var con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand command = con.CreateCommand())
                {
                    command.CommandTimeout = (int) TimeSpan.FromHours(1).TotalSeconds;
                    command.CommandText = "RESTORE VERIFYONLY FROM DISK = @FilePath";
                    command.AddParameterWithValue("@FilePath", fileName);

                    DbCommandProvider.ExecuteNonQuery(command);
                    return Result.FromSuccess();
                }
            }
        }

        /// <summary>
        /// Creates the Restore DbCommand
        /// </summary>
        private void ConfigureRestoreCommand(DbCommand command)
        {
            command.CommandTimeout = (int) TimeSpan.FromHours(2).TotalSeconds;
            command.CommandText = 
                $"ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE " +
                $"RESTORE DATABASE [{database}] " +
                "FROM DISK = @FilePath      " +
                "WITH STATS = 3, RECOVERY, REPLACE " +
                $"ALTER DATABASE [{database}] SET MULTI_USER ";

            command.AddParameterWithValue("@FilePath", backupPathAndName);
        }

        /// <summary>
        /// Restore the backup
        /// </summary>
        public Result RestoreBackup()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    Regex percentRegex = new Regex(@"(\d+) percent processed.");

                    // InfoMessage will provide progress updates
                    SqlConnection sqlConn = con.AsSqlConnection();
                    if (sqlConn != null)
                    {
                        sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                        {
                            Match match = percentRegex.Match(e.Message);
                            if (match.Success)
                            {
                                log.InfoFormat("{0}% complete", Convert.ToInt32(match.Groups[1].Value));
                            }
                        };
                    }

                    log.Info("Initiating restore");

                    // Disconnect all other users
                    SqlUtility.SetSingleUser(con);

                    // Get out of the database we are restoring to
                    con.ChangeDatabase("master");

                    using (DbCommand command = con.CreateCommand())
                    {
                        ConfigureRestoreCommand(command);

                        // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                        DbCommandProvider.ExecuteScalar(command);
                        return Result.FromSuccess();
                    }
                }
                finally
                {
                    try
                    {
                        SqlUtility.ConfigureSql2017ForClr(con, database, SqlUtility.GetUsername(con));
                        con.ChangeDatabase(database);

                        // Allow multiple connections again
                        SqlUtility.SetMultiUser(con);
                    }
                    catch (SqlException ex)
                    {
                        log.Error("Failed to set database back to multi-user mode.", ex);
                    }

                    SqlConnection.ClearAllPools();
                }
            }
        }

        /// <summary>
        /// Get the default backup path for the current SqlSessions database
        /// </summary>
        private string GetBackupPath()
        {
            string command = @"EXEC master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer',N'BackupDirectory'";

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbDataReader dbReader = DbCommandProvider.ExecuteReader(con, command))
                {
                    dbReader.Read();
                    return dbReader["Data"].ToString();
                }
            }
        }

        /// <summary>
        /// Create a backup for the given database to the file
        /// </summary>
        private void CreateBackup(string database, string backupFile)
        {
            log.InfoFormat("Backuping up '{0}' to '{1}'", database, backupFile);
            
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandTimeout = (int) TimeSpan.FromHours(2).TotalSeconds;

                    cmd.CommandText = $@"
                    DECLARE @EditionTypeId sql_variant

                    set @EditionTypeId = serverproperty('EditionID')

                    /* These are editions that support compression 
                    Enterprise:									1804890536
                    Enterprise Edition: Core-based Licensing:	1872460670 
                    Enterprise Evaluation:						610778273
                    Standard:									-1534726760
                    Business Intelligence:						284895786
                    Developer:									-2117995310
                    */
                    IF    1804890536 = @EditionTypeId 
                      or  1872460670 = @EditionTypeId
                      or  610778273  = @EditionTypeId
                      or -1534726760 = @EditionTypeId 
                      or  284895786  = @EditionTypeId 
                      or -2117995310 = @EditionTypeId
	                    BEGIN
		                    BACKUP DATABASE @Database 
		                    TO DISK = @FilePath      
		                    WITH FORMAT, INIT, SKIP, NOREWIND, NOUNLOAD, COMPRESSION,  STATS = 2
	                    END
                    ELSE
	                    BEGIN
		                    BACKUP DATABASE @Database 
		                    TO DISK = @FilePath      
		                    WITH INIT, NOUNLOAD, SKIP, STATS = 2, FORMAT
	                    END";

                    cmd.AddParameterWithValue("@Database", database);
                    cmd.AddParameterWithValue("@FilePath", backupFile);

                    Regex percentRegex = new Regex(@"(\d+) percent processed.");

                    // InfoMessage will provide progress updates
                    SqlConnection sqlConn = con.AsSqlConnection();
                    if (sqlConn != null)
                    {
                        sqlConn.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
                        {
                            Match match = percentRegex.Match(e.Message);
                            if (match.Success)
                            {
                                log.InfoFormat("{0}% complete", Convert.ToInt32(match.Groups[1].Value));
                            }
                        };
                    }

                    // The InfoMessage events only come back in real-time when using ExecuteScalar - NOT ExecuteNonQuery
                    DbCommandProvider.ExecuteScalar(cmd);
                }
            }
        }
    }
}
