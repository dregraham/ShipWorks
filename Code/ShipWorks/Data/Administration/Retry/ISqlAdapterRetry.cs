using System;
namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Helper to retry sql commands
    /// </summary>
    public interface ISqlAdapterRetry
    {
        /// <summary>
        /// Executes the given method and automatically retries the command if TException is detected.
        ///
        /// The SqlAdapter in method must be the top most transaction.  Do not use this method from within an existing transaction.
        /// </summary>
        void ExecuteWithRetry(Action method);


        /// <summary>
        /// Executes the given method with configured retry logic. A new SqlAdapter is created prior to execution.
        /// </summary>
        void ExecuteWithRetry(Action<Connection.SqlAdapter> method);
    }
}
