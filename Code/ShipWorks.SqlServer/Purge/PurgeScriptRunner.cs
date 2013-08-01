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
        public static void RunPurgeScript(string script, string purgeAppLockName, SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                try
                {
                    // Need to have an open connection for the duration of the lock acquisition/release
                    connection.Open();

                    if (!SqlAppLockUtility.IsLocked(connection, purgeAppLockName))
                    {
                        if (SqlAppLockUtility.AcquireLock(connection, purgeAppLockName))
                        {
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = script;

                                command.Parameters.Add(new SqlParameter(EarliestRetentionDateInUtcParamaterName, earliestRetentionDateInUtc));
                                command.Parameters.Add(new SqlParameter(LatestExecutionTimeInUtcParamaterName, latestExecutionTimeInUtc));
                                //command.ExecuteNonQuery();

                                // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed 
                                // to the console of client (i.e. SQL Management Studio)
                                SqlContext.Pipe.ExecuteAndSend(command);
                            }
                        }
                    }
                    else
                    {
                        // Let the caller know that someone else is already running the purge. (It may
                        // be beneficial to create/throw a more specific exception.)
                        throw new SqlLockException(string.Format("Could not acquire applock: {0}", purgeAppLockName));
                    }
                }
                finally
                {
                    SqlAppLockUtility.ReleaseLock(connection, purgeAppLockName);
                }
            }
        }
    }
}