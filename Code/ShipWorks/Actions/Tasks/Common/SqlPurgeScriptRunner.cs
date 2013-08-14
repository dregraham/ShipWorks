using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using log4net;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Common.Data;
using System;
using System.Data;
using System.Data.SqlClient;

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
        /// <param name="deadlockRetries">Number of times to retry the purge if a sql deadlock is detected.  Pass 0 to not retry.</param>
        public void RunScript(string scriptName, DateTime earliestRetentionDateInUtc, DateTime? stopExecutionAfterUtc, int deadlockRetries)
        {
            // we always want this call to be the deadlock victim
            using (new SqlDeadlockPriorityScope(-5))
            {
                using (SqlConnection connection = SqlSession.Current.OpenConnection())
                {
                    while (deadlockRetries >= 0)
                    {
                        deadlockRetries--;
                        try
                        {
                            using (SqlCommand command = SqlCommandProvider.Create(connection, scriptName))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                // Disable the command timeout since the scripts should take care of timing themselves out
                                command.CommandTimeout = 0;
                                command.Parameters.AddWithValue("@earliestRetentionDateInUtc", earliestRetentionDateInUtc);
                                command.Parameters.AddWithValue("@latestExecutionTimeInUtc", (object) stopExecutionAfterUtc ?? DBNull.Value);

                                command.ExecuteNonQuery();
                            }

                            // No errors, so just break out of the while loop
                            break;
                        }
                        catch (SqlLockException ex)
                        {
                            log.Warn(ex.Message);

                            // It wasn't a deadlock, so we want to exit the while loop
                            break;
                        }
                        catch (SqlDeadlockException sqlDeadlockException)
                        {
                            log.Warn(sqlDeadlockException);
                        }
                        catch (SqlException exception)
                        {
                            // Number = 1205 is a deadlock.  If we aren't a deadlock, rethrow.  
                            // Otherwise, log and try again if appropriate.
                            if (exception.Number != 1205)
                            {
                                throw;
                            }

                            log.Warn(exception);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        public void ShrinkDatabase()
        {
            SqlShrinkDatabase.ShrinkDatabase();
        }
    }
}
