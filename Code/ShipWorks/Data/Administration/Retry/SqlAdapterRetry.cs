using System;
using System.Threading;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Helper to retry sql adapter commands if an exception type occurrs.
    /// TException will be compared to any exception and inner exception that is thrown.
    /// If either the exception or inner exception match TException, the command will be retried.
    /// </summary>
    public class SqlAdapterRetry<TException> : ShipWorks.Data.Administration.Retry.ISqlAdapterRetry where TException : Exception
    {
        // Logger - Using the string parameter version so that we don't get the TException.ToString() in the log file
        [SuppressMessage("SonarQube", "S2743:Static fields should not be used in generic types", 
            Justification = "It is not a problem if each closed class gets its own logger")]
        private static readonly ILog log = LogManager.GetLogger("SqlAdapterRetry<TException>");

        private readonly object lockObject = new object();
        private readonly int retries = 5;
        private readonly int deadlockPriority = -5;
        private readonly string commandDescription = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="retries">Number of times to attempt execution before allowing the exception to throw.</param>
        /// <param name="deadlockPriority">Deadlock priority for the call.  Used with method that accepts a SqlAdapter parameter.</param>
        /// <param name="commandDescription">Description of the command to be run.</param>
        public SqlAdapterRetry(int retries, int deadlockPriority, string commandDescription)
        {
            if (retries < 1)
            {
                throw new ArgumentException("retries must be greater than 0.");
            }

            this.retries = retries;
            this.deadlockPriority = deadlockPriority;
            this.commandDescription = commandDescription;
        }

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        /// 
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this, 
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public void ExecuteWithRetry(Action<SqlAdapter> method)
        {
            int retryCounter = retries;

            // TODO: May need to make sure we are not in a transaction, so that this is the ONLY transaction

            lock (lockObject)
            {
                using (new LoggedStopwatch(log, string.Format("SqlAdapterRetry.ExecuteWithRetry for {0}, iteration {1}, deadlock priority {2}.", commandDescription, retries, deadlockPriority)))
                {
                    while (retryCounter >= 0)
                    {
                        try
                        {
                            using (new SqlDeadlockPriorityScope(deadlockPriority))
                            {
                                using (SqlAdapter adapter = new SqlAdapter(true))
                                {
                                    adapter.CommandTimeOut = (int) TimeSpan.FromMinutes(10).TotalSeconds;

                                    method(adapter);

                                    adapter.Commit();
                                    
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is TException || (ex.InnerException is TException))
                            {
                                log.WarnFormat("{0} detected while trying to execute.  Retrying {1} more times.", typeof(TException).Name, retryCounter);

                                if (retryCounter == 0)
                                {
                                    log.ErrorFormat("Could not execute due to maximum retry failures reached.");
                                    throw;
                                }

                                // Wait before trying again, give the other guy some time to resolve itself
                                Thread.Sleep(1000);

                                // Try again
                                retryCounter--;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        /// 
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public void ExecuteWithRetry(Action method)
        {
            int retryCounter = retries;

            lock (lockObject)
            {
                using (new LoggedStopwatch(log, string.Format("ExecuteWithRetry for {0}, iteration {1}, deadlock priority {2}.", commandDescription, retries, deadlockPriority)))
                {
                    while (retryCounter >= 0)
                    {
                        try
                        {
                            method();

                            return;
                        }
                        catch (Exception ex)
                        {
                            if (ex is TException || (ex.InnerException is TException))
                            {
                                log.WarnFormat("{0} detected while trying to execute.  Retrying {1} more times.", typeof(TException).Name, retryCounter);

                                if (retryCounter == 0)
                                {
                                    log.ErrorFormat("Could not execute due to maximum retry failures reached.");
                                    throw;
                                }

                                // Wait before trying again, give the other guy some time to resolve itself
                                Thread.Sleep(1000);

                                // Try again
                                retryCounter--;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }
    }
}
