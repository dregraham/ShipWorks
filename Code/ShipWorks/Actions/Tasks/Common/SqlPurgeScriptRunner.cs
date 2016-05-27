using System;
using System.Data;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

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
                return SqlDateTimeProvider.Current.GetUtcDate();
            }
        }

        /// <summary>
        /// Run the specified purge script with the given parameters
        /// </summary>
        /// <param name="scriptName">Name of script resource to run</param>
        /// <param name="olderThanInUtc">The earliest date for which data should be retained.
        /// Anything older will be purged</param>
        /// <param name="runUntilInUtc">Execution should stop after this time</param>
        /// <param name="retryAttempts">Number of times to retry the purge if a handleable error is detected.  Pass 0 to not retry.</param>
        public void RunScript(string scriptName, DateTime olderThanInUtc, DateTime? runUntilInUtc, int retryAttempts)
        {
            // If we detect a deadlock or sql lock, we'll sleep before we try again.
            // The sleep time will be one second times the sleep multiplier, so that we wait a little longer during each loop.
            int sleepMultiplier = 1;

            // we always want this call to be the deadlock victim
            using (new SqlDeadlockPriorityScope(-6))
            {
                while (retryAttempts >= 0)
                {
                    retryAttempts--;
                    using (SqlConnection connection = SqlSession.Current.OpenConnection())
                    {
                        try
                        {
                            using (SqlCommand command = SqlCommandProvider.Create(connection, scriptName))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                // Disable the command timeout since the scripts should take care of timing themselves out
                                command.CommandTimeout = 0;
                                command.Parameters.AddWithValue("@olderThan", olderThanInUtc);
                                command.Parameters.AddWithValue("@runUntil", (object) runUntilInUtc ?? DBNull.Value);

                                command.ExecuteNonQuery();
                            }

                            // No errors, so just break out of the while loop
                            break;
                        }
                        catch (SqlDeadlockException sqlDeadlockException)
                        {
                            // Log and try again if within number of tries.
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

                            // Log and try again if within number of tries.
                            log.Warn(exception);
                        }
                    }

                    // Sleep to give the other transaction some time to finish.
                    System.Threading.Thread.Sleep(sleepMultiplier * 1000);
                    sleepMultiplier++;
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
