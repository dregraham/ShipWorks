using System;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Class that encapsulates what it means to run a purge script.
    /// </summary>
    public class SqlPurgeScriptRunner : ISqlPurgeScriptRunner
    {
        /// <summary>
        /// Gets the Utc time from the Sql server
        /// </summary>
        public DateTime SqlUtcDateTime
        {
            get
            {
                return SqlSession.Current.GetLocalUtcDate();
            }
        }

        /// <summary>
        /// Run the specified purge script with the given parameters
        /// </summary>
        /// <param name="scriptName">Name of script resource to run</param>
        /// <param name="deleteOlderThan">How many days of data should be kept</param>
        /// <param name="stopExecutionAfterUtc">Execution should stop after this time</param>
        public void RunScript(string scriptName, int deleteOlderThan, DateTime stopExecutionAfterUtc)
        {
            string script = GetScript(scriptName);

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                SqlAppLockUtility.AcquireLock(connection, string.Format("PurgeActionTask_{0}", scriptName));
                using (SqlCommand command = SqlCommandProvider.Create(connection, script))
                {
                    command.Parameters.AddWithValue("@StopExecutionAfter", stopExecutionAfterUtc);
                    command.Parameters.AddWithValue("@deleteOlderThan", deleteOlderThan);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Gets the specified script from the embedded task resources
        /// </summary>
        /// <param name="name">Name of the Sql script to retrieve</param>
        /// <returns></returns>
        private static string GetScript(string name)
        {
            string resourceName = string.Format("ShipWorks.Data.Administration.Scripts.CleanupScripts.{0}.sql", name);
            return ResourceUtility.ReadString(resourceName);
        }
    }
}
