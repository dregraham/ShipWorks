using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading;
using Interapptive.Shared.Utility;
using log4net;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Utility functions for dealing with Sql Server and Sql statements
    /// </summary>
    public static class SqlUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlUtility));

        // Indicates if we've discovered we have to truncate with DELETE due to missing permissions
        static bool truncateWithDelete = false;

        /// <summary>
        /// Max value capable of being held in a money field
        /// </summary>
        public const decimal MoneyMaxValue = 922337203685477.5807M;

        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        public static void ExecuteScriptSql(string name, string sql, DbConnection con)
        {
            ExecuteScriptSql(name, sql, con, null);
        }

        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        public static void ExecuteScriptSql(string name, string sql, DbConnection con, DbTransaction transaction)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlScript script = new SqlScript(name, sql);

            using (DbCommand command = DbCommandProvider.Create(con))
            {
                command.Transaction = transaction;
                script.Execute(command);
            }
        }

        /// <summary>
        /// Enable CLR usage on the given connection
        /// </summary>
        public static void EnableClr(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info("EnableCLR");

            try
            {
                DbCommandProvider.ExecuteNonQuery(con, "sp_configure 'clr enabled', 1");
                DbCommandProvider.ExecuteNonQuery(con, "RECONFIGURE");
            }
            catch (Exception ex)
            {
                log.Error("Failed to enable clr.", ex);

                throw;
            }
        }

        /// <summary>
        /// Set the database on the given connection into single-user mode.
        /// </summary>
        public static void SetSingleUser(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info($"Altering database '{con.Database}' to SINGLE_USER");
            DbCommandProvider.ExecuteNonQuery(con, "ALTER DATABASE [" + con.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
        }

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        public static void SetMultiUser(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info($"Altering database '{con.Database}' to MULTI_USER");
            DbCommandProvider.ExecuteNonQuery(con, "ALTER DATABASE [" + con.Database + "] SET MULTI_USER WITH ROLLBACK IMMEDIATE");
        }

        /// <summary>
        /// Determines if the specified database is in SINGLE_USER mode using the given connection
        /// </summary>
        public static bool IsSingleUser(string connectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);
            csb.InitialCatalog = "master";
            csb.Pooling = false;
            csb.WorkstationID += Guid.NewGuid().ToString("N").Substring(5);

            object result = null;
            using (SqlConnection connection = new SqlConnection(csb.ToString()))
            {
                connection.Open();
                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT user_access FROM sys.databases WHERE name = @name";
                    cmd.AddParameterWithValue("@name", databaseName);

                    result = cmd.ExecuteScalar();
                    if (result == null || result is DBNull)
                    {
                        return false;
                    }
                }
            }

            return Convert.ToInt32(result) == 1;
        }

        /// <summary>
        /// Checks to see if there is an archiving database that didn't get renamed, possibly due to a crash,
        /// and renames it to what is expected.
        /// </summary>
        public static bool RenameArchvingDbIfNeeded(DbConnection con, string databaseName)
        {
            if (con?.Database.ToLowerInvariant() != "master")
            {
                throw new ArgumentException("con must have Database set to master to attempt renaming a database.");
            }

            if (DoesDatabaseExist(con, databaseName) || !DoesDatabaseExist(con, $"{GetArchivingDatabasename(databaseName)}"))
            {
                log.Info("Rename not applicable.");
                return false;
            }

            string rename = $"ALTER DATABASE [{GetArchivingDatabasename(databaseName)}] MODIFY NAME = [{databaseName}]";

            log.Info($"Renaming database: '{rename}'");
            DbCommandProvider.ExecuteNonQuery(con, rename);
            log.Info($"Renamed database: '{databaseName}'");

            // Need to give SQL Server time to finish up, otherwise we get an error that we can't connect yet.
            Thread.Sleep(1000);

            return true;
        }

        /// <summary>
        /// Get the archiving database name
        /// </summary>
        public static string GetArchivingDatabasename(string databaseName)
        {
            return $"ZArchiving_{databaseName}";
        }

        /// <summary>
        /// Set the number of days change tracking should retain data for the database on the given connection
        /// </summary>
        public static void SetChangeTrackingRetention(DbConnection con, int days)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.InfoFormat("Altering database to CHANGE_RETRENTION {0} DAYS", days);

            DbCommandProvider.ExecuteNonQuery(con, string.Format("ALTER DATABASE [" + con.Database + "] SET CHANGE_TRACKING (CHANGE_RETENTION = {0} DAYS)", days));
        }

        /// <summary>
        /// Start a new transaction on the given connection
        /// </summary>
        public static void BeginTransaction(DbConnection con)
        {
            DbCommandProvider.ExecuteNonQuery(con, "BEGIN TRANSACTION");
        }

        /// <summary>
        /// Commit the outstanding transaction on the given connection
        /// </summary>
        public static void CommitTransaction(DbConnection con)
        {
            DbCommandProvider.ExecuteNonQuery(con, "COMMIT");
        }

        /// <summary>
        /// Returns true if the database exists on the server from the specified connection
        /// </summary>
        public static bool DoesDatabaseExist(DbConnection con, string databaseName)
        {
            // query the sys tables to determine if a database exists
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "select count(*) from master..sysdatabases where name = @databaseName";
            cmd.AddParameterWithValue("@databaseName", databaseName);

            if ((int) DbCommandProvider.ExecuteScalar(cmd) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convert the byte[] to a long
        /// </summary>
        public static long GetTimestampValue(byte[] timestamp)
        {
            if (timestamp == null)
            {
                throw new ArgumentNullException("timestamp");
            }

            if (timestamp.Length != 8)
            {
                throw new ArgumentException("timestamp should be 8 bytes");
            }

            long value = 0;

            for (int i = 7; i >= 0; i--)
            {
                value += timestamp[i] * (long) Math.Pow(256, 7 - i);
            }

            return value;
        }

        /// <summary>
        /// Get the effective logged in SQL Server user of the connection
        /// </summary>
        public static string GetUsername(DbConnection con)
        {
            return (string) DbCommandProvider.ExecuteScalar(con, "SELECT SUSER_SNAME()");
        }

        /// <summary>
        /// Checks that the given permission name is available for the current user on the current schema
        /// </summary>
        public static bool CheckSchemaPermission(DbConnection con, string permission)
        {
            int result = (int) DbCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME(SCHEMA_NAME(), 'SCHEMA', '{0}')", permission));

            return result != 0;
        }

        /// <summary>
        /// Checks that the given user has the given permission in the current database
        /// </summary>
        public static bool CheckDatabasePermission(DbConnection con, string permission)
        {
            int result = (int) DbCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME('{0}', 'DATABASE', '{1}')", con.Database, permission));

            return result != 0;
        }

        /// <summary>
        /// Checks that the given user has the given permission on the given object in the current database
        /// </summary>
        public static bool CheckObjectPermission(DbConnection con, string objectName, string permission)
        {
            int result = (int) DbCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME('{0}', 'OBJECT', '{1}')", objectName, permission));

            return result != 0;
        }

        /// <summary>
        /// Get the file path to the master database mdf file
        /// </summary>
        public static string GetMasterDataFilePath(DbConnection con)
        {
            object fileResult = DbCommandProvider.ExecuteScalar(con, "select filename from master..sysdatabases where name = 'master'");

            if (fileResult == null || fileResult is DBNull)
            {
                throw new SqlScriptException(string.Format("The user you are connecting to SQL Server as ('{0}') does not have permissions to create a database.",
                    SqlUtility.GetUsername(con)));
            }

            // Get the filepath to master
            string masterPath = Path.GetDirectoryName((string) fileResult);

            // We need it to end in a slash
            if (!masterPath.EndsWith("\\"))
            {
                masterPath += "\\";
            }

            return masterPath;
        }

        /// <summary>
        /// Configure SQL Server 2017 to work with our assemblies
        /// </summary>
        /// <param name="con">The connection to use</param>
        /// <param name="databaseName">The database to configure</param>
        /// <param name="databaseOwner">The database owner</param>
        public static void ConfigureSql2017ForClr(DbConnection con, string databaseName, string databaseOwner)
        {
            // Only do this for SQL Server 2017 because it has to be trustworthy to run our CLR
            string version = DbCommandProvider.ExecuteScalar(con, "SELECT @@VERSION").ToString();
            if (version.StartsWith("Microsoft SQL Server 2017", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(databaseOwner))
            {
                string trustworthyStatement = $@"ALTER DATABASE [{databaseName}] SET TRUSTWORTHY ON";
                log.Info($"Executing: {trustworthyStatement}");
                DbCommandProvider.ExecuteNonQuery(con, trustworthyStatement);

                string setDatabaseOwnerStatement = $@"ALTER AUTHORIZATION ON DATABASE::[{databaseName}] TO [{databaseOwner}]";
                log.Info($"Executing: {setDatabaseOwnerStatement}");
                DbCommandProvider.ExecuteNonQuery(con, setDatabaseOwnerStatement);
            }
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        public static void TruncateTable(string filterNodeContentDirty, DbConnection sqlConnection)
        {
            TruncateTable(filterNodeContentDirty, sqlConnection, null);
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        public static void TruncateTable(string table, DbConnection con, DbTransaction transaction)
        {
            if (!truncateWithDelete)
            {
                // Try TRUNCATE for optimal performance
                try
                {
                    DbCommand truncateCmd = con.CreateCommand();
                    truncateCmd.Transaction = transaction;
                    truncateCmd.CommandText = "TRUNCATE TABLE [" + table + "]";
                    truncateCmd.ExecuteNonQuery();

                    return;
                }
                catch (SqlException)
                {

                }
            }

            // Fallback to DELETE in case user doesn't have permission
            DbCommand deleteCmd = con.CreateCommand();
            deleteCmd.Transaction = transaction;
            deleteCmd.CommandText = "DELETE " + table;
            deleteCmd.ExecuteNonQuery();

            truncateWithDelete = true;
        }

        /// <summary>
        /// Update telemetry with ingormation about the sql server.
        /// </summary>
        /// <param name="databaseUpdateResult"></param>
        public static void UpdateDatabaseTelemetry(DbConnection con, TelemetricResult<Unit> databaseUpdateResult)
        {
            int? hostCount = SqlUtility.GetConectedHostCount(con);
            databaseUpdateResult.AddProperty("ConnectedHosts", hostCount?.ToString() ?? "unknown");

            var (usedSpace, freeSpace) = SqlUtility.GetUsedAndFreeSpace(con);
            databaseUpdateResult.AddProperty("UsedSpace", usedSpace?.ToString() ?? "unknown");
            databaseUpdateResult.AddProperty("FreeSpace", freeSpace?.ToString() ?? "unknown");
        }


        /// <summary>
        /// Gets the number of hosts connected to the current database
        /// </summary>
        private static int? GetConectedHostCount(DbConnection con)
        {
            int? hostCount = null;
            string commandText = ResourceUtility.ReadString("Interapptive.Shared.Resources.DistinctUserCount.sql");
            
            try
            {
                hostCount = (int) DbCommandProvider.ExecuteScalar(con, commandText);
            } 
            catch(SqlException ex)
            {
                log.Error("Error getting ConnectedHostCount", ex);
            }

            return hostCount;
        }

        /// <summary>
        /// Gets two values: (used space, free space)
        /// </summary>
        private static (int? usedSpace, int? freeSpace) GetUsedAndFreeSpace(DbConnection con)
        {
            int? usedSpace = null;
            int? freeSpace = null;
            string commandText = ResourceUtility.ReadString("Interapptive.Shared.Resources.FreeSpace.sql");

            try
            {
                using (DbDataReader dbReader = DbCommandProvider.ExecuteReader(con, commandText))
                {
                    if (dbReader.Read())
                    {
                        usedSpace = (int) dbReader[0];
                        freeSpace = (int) dbReader[1];
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("Error getting GetUsedAndFreeSpace", ex);
            }

            return (usedSpace, freeSpace);
        }

        /// <summary>
        /// Queries the database for running queries, and returns a pipe delimited CSV of the results.
        /// </summary>
        /// <returns>Pipe delimited CSV of running queries.</returns>
        public static string GetRunningSqlCommands(string connectionString)
        {
            StringBuilder runningDbCommands = new StringBuilder();
            bool isFirstRow = true;

            try
            {
                runningDbCommands.AppendLine("SQL Commands that were running:");

                using (DbConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (DbCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = ResourceUtility.ReadString("Interapptive.Shared.Resources.RunningSqlQueries.sql");
                        sqlConnection.Open();

                        using (DbDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                if (isFirstRow)
                                {
                                    DataTable schemaTable = sqlDataReader.GetSchemaTable();

                                    for (int rowNumber = 0; rowNumber < schemaTable.Rows.Count; rowNumber++)
                                    {
                                        DataRow schemaRow = schemaTable.Rows[rowNumber];
                                        runningDbCommands.AppendFormat("{0}|", schemaRow[0]);
                                    }
                                    runningDbCommands.AppendLine();

                                    isFirstRow = false;
                                }

                                for (int columnNumber = 0; columnNumber < sqlDataReader.FieldCount; columnNumber++)
                                {
                                    string value = sqlDataReader[columnNumber].ToString().Replace(Environment.NewLine, " ");
                                    runningDbCommands.AppendFormat("{0}|", value);
                                }

                                runningDbCommands.AppendLine();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Since this is method is really only for logging additional troubleshooting info, we don't want an exception here to interfere
                // with the real exception that occurred, so we'll just log this exception, and carry on.
                log.Error("An error occurred while attempting to determine sql commands that were running.", ex);
                runningDbCommands.AppendLine(ex.Message);
            }

            return runningDbCommands.ToString();
        }
    }
}
