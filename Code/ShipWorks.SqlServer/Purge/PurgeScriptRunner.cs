using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.SqlServer.Purge
{
    /// <summary>
    /// Runs Purge Scripts
    /// </summary>
    public static class PurgeScriptRunner
    {
        public const string EarliestRetentionDateInUtcParamaterName = "@earliestRetentionDateInUtc";

        public const string LatestExecutionTimeInUtcParamaterName = "@latestExecutionTimeInUtc";

        /// <summary>
        /// Runs a sql script. Assumes script has paramaters @RetentionDateInUtc and @LatestExecutionTimeInUtc.
        /// </summary>
        /// <param name="script">Sql script to run. Assumes script has paramaters @RetentionDateInUtc and @LatestExecutionTimeInUtc.</param>
        /// <param name="purgeAppLockName">Checks for this AppLock. If applock is taken, script will not run and purge exception is thrown.</param>
        /// <param name="earliestRetentionDateInUtc">Delete records older than this date</param>
        /// <param name="latestExecutionTimeInUtc">Stop running the script if it runs longer than this time</param>
        public static void RunPurgeScript(string script, string purgeAppLockName, SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                SqlAppLockUtility.RunLockedCommand(connection, purgeAppLockName, command =>
                {
                    command.CommandText = script;

                    command.Parameters.Add(new SqlParameter(EarliestRetentionDateInUtcParamaterName, earliestRetentionDateInUtc));
                    command.Parameters.Add(new SqlParameter(LatestExecutionTimeInUtcParamaterName, latestExecutionTimeInUtc));

                    // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed 
                    // to the console of client (i.e. SQL Management Studio)
                    if (SqlContext.Pipe != null)
                    {
                        SqlContext.Pipe.ExecuteAndSend(command);
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                });
            }
        }
    }
}