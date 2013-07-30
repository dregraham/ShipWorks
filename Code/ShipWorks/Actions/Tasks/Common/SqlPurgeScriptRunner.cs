using System;
using System.Data;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Purge;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Class that encapsulates what it means to run a purge script.
    /// </summary>
    public class SqlPurgeScriptRunner : ISqlPurgeScriptRunner
    {
        static readonly ILog log = LogManager.GetLogger(typeof(PurgeDatabaseTask));

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
        /// <param name="earliestRetentionDateInUtc">The earliest date for which data should be retained.
        /// Anything older will be purged</param>
        /// <param name="stopExecutionAfterUtc">Execution should stop after this time</param>
        public void RunScript(string scriptName, DateTime earliestRetentionDateInUtc, DateTime stopExecutionAfterUtc)
        {
            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                try
                {
                    using (SqlCommand command = SqlCommandProvider.Create(connection, scriptName))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Disable the command timeout since the scripts should take care of timing themselves out
                        command.CommandTimeout = 0;
                        command.Parameters.AddWithValue("@earliestRetentionDateInUtc", earliestRetentionDateInUtc);
                        command.Parameters.AddWithValue("@latestExecutionTimeInUtc", stopExecutionAfterUtc);

                        command.ExecuteNonQuery();
                    }
                }
                catch (PurgeException ex)
                {
                    log.Warn(ex.Message);
                }
            }
        }
    }
}
