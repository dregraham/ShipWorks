using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Factory for creating an instance SqlAdapterRetry
    /// </summary>
    [Component]
    public class SqlAdapterRetryFactory<TException> : ISqlAdapterRetryFactory where TException : Exception
    {
        /// <summary>
        /// Creates the SqlAdapterRetry
        /// </summary>
        public ISqlAdapterRetry Create(int retries, int deadlockPriority, string commandDescription)
            => new SqlAdapterRetry<TException>(retries, deadlockPriority, commandDescription);
    }
}
