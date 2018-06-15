using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using static Interapptive.Shared.Utility.Functional;
using static ShipWorks.Data.Utility.DataFunctions;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Helper to retry sql adapter commands if an exception type occurs.
    /// TException will be compared to any exception and inner exception that is thrown.
    /// If either the exception or inner exception match TException, the command will be retried.
    /// </summary>
    public static class SqlAdapterRetry
    {
        // Logger - Using the string parameter version so that we don't get the TException.ToString() in the log file
        private static readonly ILog log = LogManager.GetLogger("SqlAdapterRetry<TException>");

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public static GenericResult<Unit> ExecuteWithRetry(
                string commandDescription,
                Action<ISqlAdapter> method,
                Func<ISqlAdapter> createSqlAdapter,
                Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetry(commandDescription, SqlAdapterRetryOptions.Default, method, createSqlAdapter, isHandleableException);

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public static GenericResult<Unit> ExecuteWithRetry(
                string commandDescription,
                SqlAdapterRetryOptions options,
                Action<ISqlAdapter> method,
                Func<ISqlAdapter> createSqlAdapter,
                Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetry(
                commandDescription,
                options,
                () => WithDeadlockPriority(
                    options.DeadlockPriority,
                    () => WithSqlAdapter(createSqlAdapter, method.ToFunc())),
                isHandleableException);

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public static GenericResult<Unit> ExecuteWithRetry(
                string commandDescription,
                Action method,
                Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetry(commandDescription, SqlAdapterRetryOptions.Default, method, isHandleableException);

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public static GenericResult<Unit> ExecuteWithRetry(
                string commandDescription,
                SqlAdapterRetryOptions options,
                Action method,
                Func<Exception, bool> isHandleableException) =>
            Using(
                new LoggedStopwatch(log, $"ExecuteWithRetry for {commandDescription}, deadlock priority {options.DeadlockPriority}."),
                _ => method.ToFunc().Retry(options.Retries, options.RetryDelay, isHandleableException, options.Log));

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public static Task<T> ExecuteWithRetryAsync<T>(
                string commandDescription,
                Func<ISqlAdapter, Task<T>> method,
                Func<ISqlAdapter> createSqlAdapter, Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetryAsync(commandDescription, SqlAdapterRetryOptions.Default, method, createSqlAdapter, isHandleableException);

        /// <summary>
        /// Executes the given method with a new sql adapter that is setup with SqlDeadlockPriorityScope, and automatic retries the command
        /// if TException is detected.
        ///
        /// This cannot be called within a current transaction.  This method will create a new transacted SqlAdapter for each retry.  If we didn't do this,
        /// and an exception is thrown, everything would be rolled back.
        /// </summary>
        /// <param name="method">Method to execute.  </param>
        public static Task<T> ExecuteWithRetryAsync<T>(
                string commandDescription,
                SqlAdapterRetryOptions options,
                Func<ISqlAdapter, Task<T>> method,
                Func<ISqlAdapter> createSqlAdapter,
                Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetryAsync(
                commandDescription,
                options,
                () => WithDeadlockPriorityAsync(
                    options.DeadlockPriority,
                    () => WithSqlAdapterAsync(createSqlAdapter, method)),
                isHandleableException);

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public static Task<T> ExecuteWithRetryAsync<T>(
                string commandDescription,
                Func<Task<T>> method,
                Func<Exception, bool> isHandleableException) =>
            ExecuteWithRetryAsync(commandDescription, SqlAdapterRetryOptions.Default, method, isHandleableException);

        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        public static Task<T> ExecuteWithRetryAsync<T>(
                string commandDescription,
                SqlAdapterRetryOptions options,
                Func<Task<T>> method,
                Func<Exception, bool> isHandleableException) =>
            UsingAsync(
                new LoggedStopwatch(log, $"ExecuteWithRetryAsync for {commandDescription}, deadlock priority {options.DeadlockPriority}."),
                _ => method.RetryAsync(options.Retries, options.RetryDelay, isHandleableException, options.Log));
    }
}
