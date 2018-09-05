using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Data.Administration.Recovery
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
        public void ExecuteWithRetry(Action method) =>
            Using(
                new LoggedStopwatch(log, $"ExecuteWithRetry for {commandDescription}, deadlock priority {deadlockPriority}."),
                _ => Retry(method.ToFunc(), retries, retryDelay, ShouldRetry))
            .ThrowOnFailure();

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> method) =>
            UsingAsync(
                new LoggedStopwatch(log, $"ExecuteWithRetry for {commandDescription}, deadlock priority {deadlockPriority}."),
                _ => RetryAsync(method, retries, retryDelay, ShouldRetry));

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public Task ExecuteWithRetryAsync(Func<Task> method) =>
            ExecuteWithRetryAsync(() => method().ToTyped<Unit>());

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
        /// Should the method be retried
        /// </summary>
        private bool ShouldRetry(Exception ex) =>
            ex is TException || (ex.InnerException is TException);
    }
}
