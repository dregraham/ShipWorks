using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using log4net;

namespace ShipWorks.Installer.Sql
{
    /// <summary>
    /// Helper class for communicating with a sql server
    /// </summary>
    public class SqlUtility : ISqlUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlUtility));

        /// <summary>
        /// The name given to the special default instance of sql server
        /// </summary>
        public string DefaultInstanceName => "MSSQLSERVER";

        /// <summary>
        /// The default password ShipWorks uses for sa when it installs new SQL instances
        /// </summary>
        public string ShipWorksSaPassword => "ShipW@rks1";

        /// <summary>
        /// See if we can figure out the credentials necessary to connect to the given instance.  If provided, the configuration given in firstTry will be attempted first
        /// </summary>
        public async Task<SqlSessionConfiguration> DetermineCredentials(string instance, SqlSessionConfiguration firstTry = null)
        {
            List<SqlSessionConfiguration> configsToTry = new List<SqlSessionConfiguration>();

            // If firstTry was given, and we are connecting to the same instance it represents, use its config first as it will likely work.
            if ((firstTry != null) &&
                (firstTry.WindowsAuth || !string.IsNullOrWhiteSpace(firstTry.Username)) &&
                (firstTry.ServerInstance == instance))
            {
                configsToTry.Add(new SqlSessionConfiguration(firstTry));
            }

            // Then we'll try the sa account with the password we create - we know that'd be an admin
            configsToTry.Add(new SqlSessionConfiguration()
            {
                Username = "sa",
                Password = this.ShipWorksSaPassword,
                WindowsAuth = false
            });


            // Then we'll try windows auth
            configsToTry.Add(new SqlSessionConfiguration()
            {
                WindowsAuth = true
            });

            foreach (SqlSessionConfiguration config in configsToTry)
            {
                try
                {
                    config.ServerInstance = instance;
                    config.DatabaseName = "";

                    SqlSession session = new SqlSession(config);
                    await session.TestConnection(TimeSpan.FromSeconds(3)).ConfigureAwait(false);

                    return config;
                }
                catch (SqlException ex)
                {
                    log.Info("Failed to connect.", ex);
                }
            }

            return null;
        }

        /// <summary>
        /// Validates that an open connection is actually open.  With connection pooling, a connection in the Open state
        /// may not actually have a real connection to the database if the database has gone down or the network has dropped.
        ///
        /// This also serves to reset the connection isolation level to READ COMMITTED for every connection, as the connection pool
        /// does not do this! http://support.microsoft.com/kb/309544
        ///
        /// </summary>
        public async Task<bool> ValidateOpenConnection(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            // In the case of actions where WorkstationID could be longer than 128 due to the task name,
            // go ahead and truncate to 127 just to be sure we don't go over the context allowed size.
            string workstationID = string.Empty;
            if (System.Environment.MachineName.Length > 127)
            {
                workstationID = System.Environment.MachineName.Substring(0, 127);
            }
            else
            {
                workstationID = System.Environment.MachineName;
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandTimeout = 5;
            cmd.CommandText = $@"
                SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

                DECLARE @Ctx varbinary(128)
                SELECT @Ctx = CONVERT(varbinary(128), '{ workstationID }')
                SET CONTEXT_INFO @Ctx
                ";
            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get all of the details about all of the databases on the instance of the connection
        /// </summary>
        public async Task<IEnumerable<string>> GetDatabaseDetails(DbConnection con)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.CommandText = "select name from master..sysdatabases where name not in ('master', 'model', 'msdb', 'tempdb')";

            List<string> names = new List<string>();

            using (DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    names.Add((string) reader["name"]);
                }
            }

            List<string> shipWorksDBs = new List<string>();

            foreach (string name in names)
            {
                con.ChangeDatabase(name);

                var command = con.CreateCommand();
                command.CommandText = "SELECT COALESCE(OBJECT_ID('GetSchemaVersion'), 0)";
                if ((int) await command.ExecuteScalarAsync().ConfigureAwait(false) > 0)
                {
                    shipWorksDBs.Add(name);
                }
            }

            return shipWorksDBs;
        }
    }
}
