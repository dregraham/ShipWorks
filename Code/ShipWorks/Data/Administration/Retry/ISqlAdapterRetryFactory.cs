using System;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Factory for creating an instance SqlAdapterRetry
    /// </summary>
    public interface ISqlAdapterRetryFactory
    {
        /// <summary>
        /// Creates the SqlAdapterRetry
        /// </summary>
        ISqlAdapterRetry Create<TException>(int retries, int deadlockPriority, string commandDescription)
            where TException : Exception;
    }
}