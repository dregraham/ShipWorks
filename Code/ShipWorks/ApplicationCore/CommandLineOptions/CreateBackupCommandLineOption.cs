using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Logging;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for taking a backup prior to upgrade
    /// </summary>
    public class CreateBackupCommandLineOption : ICommandLineCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpgradeDatabaseSchemaCommandLineOption));
        private const string BackupNameFormat = "{0}_AutomaticUpgradeBackup.bak";

        /// <summary>
        /// the command name
        /// </summary>
        public string CommandName => "createdefaultbackup";

        /// <summary>
        /// Execute the backup
        /// </summary>
        /// <param name="args"></param>
        public Task Execute(List<string> args)
        {
            if (!SqlSession.IsConfigured)
            {
                SqlSession.Initialize();
            }

            string database = SqlSession.Current.DatabaseName;
            string path = GetBackupPath(); ;
            string backupName = string.Format(BackupNameFormat, database);

            try
            {
                CreateBackup(database, $"{path}\\{backupName}");
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                Environment.ExitCode = ex.Number;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Environment.ExitCode = -1;
            }

            return Task.CompletedTask;
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

        private void CreateBackup(string database, string backupFile)
        {
            log.InfoFormat("Backuping up '{0}' to '{1}'", database, backupFile);

            SqlServerEditionIdType sqlServerEditionId = SqlServerEditionIdType.Express;

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
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
	                    END

                    SELECT serverproperty('EditionID')";

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
                object commandResult = DbCommandProvider.ExecuteScalar(cmd);
                sqlServerEditionId = sqlServerEditionId.Parse(commandResult.ToString(), SqlServerEditionIdType.Express);
            }
        }
    }
}
