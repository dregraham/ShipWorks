using System;
using System.Threading;
using log4net;
using System.Threading.Tasks;

namespace ShipWorks.Core.Common.Threading
{
    /// <summary>
    /// Helper to retry a method if an exception type occurrs.
    /// TException will be compared to any exception and inner exception that is thrown.
    /// If either the exception or inner exception match TException, the method will be retried.
    /// </summary>
    public static class ActionRetry
    {
        // Logger - Using the string parameter version so that we don't get the TException.ToString() in the log file
        private static readonly ILog log = LogManager.GetLogger("ActionRetry");

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        /// </summary>
        public static async Task ExecuteWithRetry<TException>(int retries, Func<Task> method) where TException : Exception
        {
            int retryCounter = retries;

            while (retryCounter >= 0)
            {
                try
                {
                    await method();
                    return;
                }
                catch (Exception ex)
                {
                    if (ex is TException || (ex.InnerException is TException))
                    {
                        log.WarnFormat("{0} detected while trying to execute.  Retrying {1} more times.", typeof(TException).Name, retryCounter);

                        if (retryCounter == 0)
                        {
                            log.Error("Could not execute due to maximum retry failures reached.");
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

            throw new InvalidOperationException("Cound not execute due to maximum retry failures reached");
        }
    }
}
