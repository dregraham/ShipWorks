using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Installer.Sql;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for looking up running sql servers
    /// </summary>
    public class SqlServerLookupService : ISqlServerLookupService
    {
        ISqlSession sqlSession;
        ISqlUtility sqlUtility;
        public SqlServerLookupService(ISqlSession sqlSession, ISqlUtility sqlUtility)
        {
            this.sqlSession = sqlSession;
            this.sqlUtility = sqlUtility;
        }

        /// <summary>
        /// Gets a list of databases from the provided server instance
        /// </summary>
        public async Task<IEnumerable<SqlSessionConfiguration>> GetDatabases(string serverInstance, string username = "", string password = "")
        {
            var config = new SqlSessionConfiguration
            {
                Username = username,
                Password = password,
                ServerInstance = serverInstance
            };

            sqlSession.Configuration = config;
            // Start the background task to try to log in and figure out the background databases...
            SqlSessionConfiguration configuration = await Task.Run(() => sqlUtility.DetermineCredentials(serverInstance, config)).ConfigureAwait(true);
            sqlSession.Configuration = configuration;
            IEnumerable<string> databases = null;

            if (configuration != null)
            {
                using (DbConnection con = sqlSession.OpenConnection())
                {
                    databases = await sqlUtility.GetDatabaseDetails(con).ConfigureAwait(true);
                }
            }

            return databases.Select(d =>
            new SqlSessionConfiguration(configuration)
            {
                DatabaseName = d
            });
        }

        /// <summary>
        /// Test a connection to a database
        /// </summary>
        public async Task<bool> TestConnection(SqlSessionConfiguration config)
        {
            sqlSession.Configuration = config;
            return await Task.Run(sqlSession.CanConnect).ConfigureAwait(true);
        }
    }
}
