using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.Tests.Shared.Database
{
    [SuppressMessage("Sonar", "CS0168: The variable 'ex' is declared but never used", Justification = "Tests")]
    [SuppressMessage("Sonar", "S112", Justification = "Tests")]
    public static class DbUtils
    {
        public static string GetRestoreBackupFilename(Assembly assembly, string resourceName)
        {
            string backupFileName = Path.GetTempFileName();
            using (FileStream backupFile = new FileStream(backupFileName, FileMode.Create))
            {
                assembly.GetManifestResourceStream(resourceName).CopyTo(backupFile);
            }

            return backupFileName;
        }

        public static async Task MakeConnectionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    using (SqlConnection connection = new SqlConnection(SqlSession.Current.Configuration.GetConnectionString()))
                    {
                        connection.Open();
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }
            catch
            {
                // Ignore this.
            }
        }

        public static void BackupDb(DbConnection connection, string backupPathAndfilename)
        {
            DbCommand cmd = DbCommandProvider.Create(connection);
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

            cmd.AddParameterWithValue("@Database", connection.Database);
            cmd.AddParameterWithValue("@FilePath", backupPathAndfilename);

            DbCommandProvider.ExecuteScalar(cmd);
        }

        public static void RestoreDb(DbConnection connection, string backupPathAndfilename)
        {
            string dbName = connection.Database;
            string sourceDb = null;
            string sourceLog = null;
            string targetDb;
            string targetLog;
            DbCommand cmd;

            using (new ConnectionSensitiveScope("DbUtils RestoreDb", null))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    // Change into the database we are restoring into
                    con.ChangeDatabase(connection.Database);

                    GetPhysicalFileLocations(con, out targetDb, out targetLog);

                    if (targetDb == null || targetLog == null)
                    {
                        throw new FileNotFoundException("Could not locate physical database files.");
                    }

                    // We have to move the original logical files to the the file locations we are restoring to.  First we find
                    // the current logical file names.
                    cmd = DbCommandProvider.Create(con);
                    cmd.CommandText = "restore filelistonly from disk = @FilePath";
                    cmd.AddParameterWithValue("@FilePath", backupPathAndfilename);

                    try
                    {
                        // Determine logical names in the backup source
                        using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                        {
                            while (reader.Read())
                            {
                                string type = (string) reader["Type"];

                                if (type == "D")
                                {
                                    sourceDb = (string) reader["LogicalName"];
                                }

                                if (type == "L")
                                {
                                    sourceLog = (string) reader["LogicalName"];
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 262 && ex.Message.ToLower().Contains("create"))
                        {
                            throw new Exception(
                                $"The SQL Server user ('{SqlUtility.GetUsername(con)}') does not have permissions to restore database backups.", ex);
                        }

                        throw;
                    }

                    // See if we have everything we need
                    if (sourceDb == null || sourceLog == null)
                    {
                        throw new InvalidOperationException("The logical file groups could not be located in the SQL Server backup.");
                    }
                }

                cmd = DbCommandProvider.Create(connection);
                string sql =
                    $@"RESTORE DATABASE @Database
                    FROM DISK = @FilePath 
                    WITH NOUNLOAD, STATS = 3, RECOVERY, REPLACE,   
                    MOVE '{sourceDb}' TO '{targetDb}',   
                    MOVE '{sourceLog}' TO '{targetLog}';

                {SqlUtility.ConfigureSqlServerForClrSql(dbName, SqlUtility.GetUsername(connection))}";

                cmd.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, dbName);

                cmd.AddParameterWithValue("@FilePath", backupPathAndfilename);
                cmd.AddParameterWithValue("@Database", dbName);

                DbCommandProvider.ExecuteScalar(cmd);
            }
        }

        public static void DeleteBackupFile(string backupPathAndfilename)
        {
            try
            {
                File.Delete(backupPathAndfilename);
            }
            catch 
            {
                // Ignore this.
            }
        }

        public static void GetPhysicalFileLocations(DbConnection con, out string targetDb, out string targetLog)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "sp_helpfile";
            cmd.CommandType = CommandType.StoredProcedure;

            // set outputs
            targetDb = null;
            targetLog = null;

            // Determine current physical files
            using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    string file = ((string) reader["filename"]).Trim();
                    string usage = ((string) reader["usage"]).Trim();

                    if (usage == "log only")
                    {
                        targetLog = file;
                    }

                    if (usage == "data only")
                    {
                        targetDb = file;
                    }
                }
            }
        }

        public static void DropAssemblies()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (var transaction = con.BeginTransaction())
                {
                    SqlAssemblyDeployer.DropAssemblies(con, transaction);
                    transaction.Commit();
                }
            }
        }

        public static void DeployAssemblies()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (var transaction = con.BeginTransaction())
                {
                    SqlAssemblyDeployer.DeployAssemblies(con, transaction);
                    transaction.Commit();
                }

                SqlSchemaUpdater.UpdateSchemaVersionStoredProcedure(con);
            }
        }
    }
}
