using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Factory for creating an instance SqlAdapterRetry
    /// </summary>
    [Component]
    public class SqlAdapterRetryFactory : ISqlAdapterRetryFactory
    {
        /// <summary>
        /// Creates the SqlAdapterRetry
        /// </summary>
        public ISqlAdapterRetry Create<TException>(int retries, int deadlockPriority, string commandDescription) where TException : Exception =>
            new SqlAdapterRetry<TException>(retries, deadlockPriority, commandDescription);
    }
}
