using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class SqlUtilityTest : IDisposable
    {
        private readonly DataContext context;
        private readonly AutoMock mock;
        private readonly DatabaseFixture db;

        public SqlUtilityTest(DatabaseFixture db)
        {
            this.db = db;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenSqlExceptionOccurrs()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                string sql = "select 1/0;";

                DbCommand cmd = DbCommandProvider.Create(connection as DbConnection);
                cmd.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
                cmd.CommandTimeout = 1800;

                DbCommandProvider.ExecuteScalar(cmd);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenCreatingBackup()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;
                backupPathAndfilename = Path.Combine(System.IO.Path.GetTempPath(), $"{dbName}_{Guid.NewGuid().ToString("N")}.bak");

                BackupDb(connection, backupPathAndfilename);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DeleteBackupFile(backupPathAndfilename);
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenCreatingBackupAndRestoring()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;
                backupPathAndfilename = Path.Combine(System.IO.Path.GetTempPath(), $"{dbName}_{Guid.NewGuid().ToString("N")}.bak");

                BackupDb(connection, backupPathAndfilename);
                RestoreDb(connection, backupPathAndfilename);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DeleteBackupFile(backupPathAndfilename);

            db.ResetDatabase();
        }

        [Fact]
        public void DbIsSetToMultiUser_WithGoodBackup_AfterCreatingBackupAndRestoring()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                string backupFileName = GetRestoreBackupFilename("ShipWorks.Core.Tests.Integration.DbBackups.ShipWorks_Good.bk");

                RestoreDb(connection, backupFileName);
            }

            UpgradeDatabaseSchemaCommandLineOption upgradeDatabaseSchemaCommand = new UpgradeDatabaseSchemaCommandLineOption();
            upgradeDatabaseSchemaCommand.Execute(null);

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DeleteBackupFile(backupPathAndfilename);

            db.ResetDatabase();
        }

        [Fact]
        public void DbIsSetToMultiUser_WithBadBackup_AfterCreatingBackupAndRestoring()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                string backupFileName = GetRestoreBackupFilename("ShipWorks.Core.Tests.Integration.DbBackups.ShipWorks_Bad.bk");
                RestoreDb(connection, backupFileName);
            }

            UpgradeDatabaseSchemaCommandLineOption upgradeDatabaseSchemaCommand = new UpgradeDatabaseSchemaCommandLineOption();
            upgradeDatabaseSchemaCommand.Execute(null);

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DeleteBackupFile(backupPathAndfilename);

            db.ResetDatabase();
        }

        [Fact]
        public void DbIsStillSingleUser_WhenSingleUserBeforeExecution()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            DbConnection singleUserModeConnection = SqlSession.Current.OpenConnection();

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                SqlUtility.SetSingleUser(singleUserModeConnection, dbName);

                string sql = "select 1/0;";

                DbCommand cmd = DbCommandProvider.Create(connection as DbConnection);
                cmd.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
                cmd.CommandTimeout = 1800;

                try
                {
                    DbCommandProvider.ExecuteScalar(cmd);
                }
                catch (Exception e) when (e.Message.Contains("The connection is broken and recovery is not possible"))
                {
                }
            }

            Assert.True(SqlUtility.IsSingleUser(connectionString, dbName));
            SqlUtility.SetMultiUser(singleUserModeConnection);
        }

        [Fact]
        public async Task DbIsSetToMultiUser_WithGoodBackup_AfterCreatingBackupAndRestoring_AndOtherConnectionsBeingMade()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                string backupFileName = GetRestoreBackupFilename("ShipWorks.Core.Tests.Integration.DbBackups.ShipWorks_Good.bk");
                RestoreDb(connection, backupFileName);
            }

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;
            Task makeConnectionsTask = MakeConnectionsAsync(cancellationToken);

            UpgradeDatabaseSchemaCommandLineOption upgradeDatabaseSchemaCommand = new UpgradeDatabaseSchemaCommandLineOption();
            await upgradeDatabaseSchemaCommand.Execute(null);

            source.Cancel();
            await Task.WhenAll(makeConnectionsTask);
            await Task.Delay(TimeSpan.FromMinutes(1));

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DeleteBackupFile(backupPathAndfilename);

            db.ResetDatabase();
        }

        private string GetRestoreBackupFilename(string resourceName)
        {
            string backupFileName = Path.GetTempFileName();
            using (FileStream backupFile = new FileStream(backupFileName, FileMode.Create))
            {
                Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName).CopyTo(backupFile);
            }

            return backupFileName;
        }

        private async Task MakeConnectionsAsync(CancellationToken cancellationToken)
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
            }
        }

        private void BackupDb(DbConnection connection, string backupPathAndfilename)
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

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

        private void RestoreDb(DbConnection connection, string backupPathAndfilename)
        {
            string connectionString = connection.ConnectionString;
            string dbName = connection.Database;
            string sourceDb = null;
            string sourceLog = null;
            string targetDb;
            string targetLog;
            DbCommand cmd;

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
                        throw new ApplicationException(
                            string.Format("The SQL Server user ('{0}') does not have permissions to restore database backups.",
                           SqlUtility.GetUsername(con)), ex);
                    }
                    else
                    {
                        throw;
                    }
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

        private void DeleteBackupFile(string backupPathAndfilename)
        {
            try
            {
                File.Delete(backupPathAndfilename);
            }
            catch
            {
            }
        }

        private static void GetPhysicalFileLocations(DbConnection con, out string targetDb, out string targetLog)
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

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
