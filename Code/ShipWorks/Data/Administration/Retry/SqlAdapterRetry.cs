using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Helper to retry sql adapter commands if an exception type occurs.
    /// TException will be compared to any exception and inner exception that is thrown.
    /// If either the exception or inner exception match TException, the command will be retried.
    /// </summary>
    [Component]
    public class SqlAdapterRetry<TException> : ISqlAdapterRetry where TException : Exception
    {
        // Logger - Using the string parameter version so that we don't get the TException.ToString() in the log file
        [SuppressMessage("SonarQube", "S2743:Static fields should not be used in generic types",
            Justification = "It is not a problem if each closed class gets its own logger")]
        private static readonly ILog log = LogManager.GetLogger("SqlAdapterRetry<TException>");

        private readonly object lockObject = new object();
        private readonly int retries = 5;
        private readonly int deadlockPriority = -5;
        private readonly string commandDescription = string.Empty;
        private readonly TimeSpan retryDelay;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="retries">Number of times to attempt execution before allowing the exception to throw.</param>
        /// <param name="deadlockPriority">Deadlock priority for the call.  Used with method that accepts a SqlAdapter parameter.</param>
        /// <param name="commandDescription">Description of the command to be run.</param>
        public SqlAdapterRetry(int retries, int deadlockPriority, string commandDescription) :
            this(retries, deadlockPriority, commandDescription, TimeSpan.FromSeconds(1))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="retries">Number of times to attempt execution before allowing the exception to throw.</param>
        /// <param name="deadlockPriority">Deadlock priority for the call.  Used with method that accepts a SqlAdapter parameter.</param>
        /// <param name="commandDescription">Description of the command to be run.</param>
        public SqlAdapterRetry(int retries, int deadlockPriority, string commandDescription, TimeSpan retryDelay)
        {
            if (retries < 1)
            {
                throw new ArgumentException("retries must be greater than 0.");
            }

            this.retries = retries;
            this.deadlockPriority = deadlockPriority;
            this.commandDescription = commandDescription;
            this.retryDelay = retryDelay;
        }

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public void ExecuteWithRetry(Action<ISqlAdapter> method) =>
            ExecuteWithRetry(method, () => SqlAdapter.Create(true));

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public void ExecuteWithRetry(Action<ISqlAdapter> method, Func<ISqlAdapter> createSqlAdapter) =>
            ExecuteWithRetry(() =>
            {
                using (new SqlDeadlockPriorityScope(deadlockPriority))
                {
                    using (ISqlAdapter adapter = createSqlAdapter())
                    {
                        adapter.CommandTimeOut = (int) TimeSpan.FromMinutes(10).TotalSeconds;

                        method(adapter);

                        adapter.Commit();

                        return;
                    }
                }
            });

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
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
                            var result = HandleException(ex, retryCounter);

                            if (result.Success)
                            {
                                retryCounter = result.Value;
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

        //TODO: Refactor the method below so that it doesn't duplicate most of the method above
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> method) =>
            ExecuteWithRetryAsync<T>(x => method(), ex => false);

        //TODO: Refactor the method below so that it doesn't duplicate most of the method above
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public async Task<T> ExecuteWithRetryAsync<T>(Func<int, Task<T>> method, Func<Exception, bool> exceptionCheck)
        {
            int retryCounter = retries;

            using (new LoggedStopwatch(log, string.Format("ExecuteWithRetryAsync for {0}, iteration {1}, deadlock priority {2}.", commandDescription, retries, deadlockPriority)))
            {
                while (retryCounter >= 0)
                {
                    try
                    {
                        return await method(retries - retryCounter).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        var result = await HandleExceptionAsync(ex, retryCounter, exceptionCheck);

                        if (result.Success)
                        {
                            retryCounter = result.Value;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return default(T);
            }
        }

        //TODO: Refactor the method below so that it doesn't duplicate most of the method above
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task ExecuteWithRetryAsync(Func<Task> method) =>
            ExecuteWithRetryAsync(method, ex => false);

        //TODO: Refactor the method below so that it doesn't duplicate most of the method above
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task ExecuteWithRetryAsync(Func<Task> method, Func<Exception, bool> exceptionCheck) =>
            ExecuteWithRetryAsync(x => method(), exceptionCheck);

        //TODO: Refactor the method below so that it doesn't duplicate most of the method above
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task ExecuteWithRetryAsync(Func<int, Task> method, Func<Exception, bool> exceptionCheck) =>
            ExecuteWithRetryAsync(async (x) =>
            {
                await method(x).ConfigureAwait(false);
                return Unit.Default;
            }, exceptionCheck);

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public Task ExecuteWithRetryAsync(Func<ISqlAdapter, Task> method) =>
            ExecuteWithRetryAsync(method, () => SqlAdapter.Create(true));

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public Task ExecuteWithRetryAsync(Func<ISqlAdapter, Task> method, Func<ISqlAdapter> createSqlAdapter) =>
            ExecuteWithRetryAsync(async () =>
            {
                using (new SqlDeadlockPriorityScope(deadlockPriority))
                {
                    using (ISqlAdapter adapter = createSqlAdapter())
                    {
                        adapter.CommandTimeOut = (int) TimeSpan.FromMinutes(10).TotalSeconds;

                        await method(adapter).ConfigureAwait(false);

                        adapter.Commit();

                        return;
                    }
                }
            });

        /// <summary>
        /// Handle the exception, if possible
        /// </summary>
        private GenericResult<int> HandleException(Exception ex, int retryCounter)
        {
            if (ex is TException || (ex.InnerException is TException))
            {
                log.WarnFormat("{0} detected while trying to execute.  Retrying {1} more times.", typeof(TException).Name, retryCounter);

                if (retryCounter == 0)
                {
                    log.ErrorFormat("Could not execute due to maximum retry failures reached.");
                    return GenericResult.FromError<int>(ex);
                }

                // Wait before trying again, give the other guy some time to resolve itself
                Thread.Sleep(retryDelay);

                return GenericResult.FromSuccess(retryCounter - 1);
            }

            return GenericResult.FromError<int>(ex);
        }

        /// <summary>
        /// Handle the exception, if possible
        /// </summary>
        private async Task<GenericResult<int>> HandleExceptionAsync(Exception ex, int retryCounter, Func<Exception, bool> exceptionCheck)
        {
            if (ex is TException || (ex.InnerException is TException) || exceptionCheck(ex))
            {
                log.WarnFormat("{0} detected while trying to execute.  Retrying {1} more times.", typeof(TException).Name, retryCounter);

                if (retryCounter == 0)
                {
                    log.ErrorFormat("Could not execute due to maximum retry failures reached.");
                    return GenericResult.FromError<int>(ex);
                }

                // Wait before trying again, give the other guy some time to resolve itself
                await Task.Delay(retryDelay).ConfigureAwait(false);

                return GenericResult.FromSuccess(retryCounter - 1);
            }

            return GenericResult.FromError<int>(ex);
        }
    }
}
