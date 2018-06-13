using System;

namespace ShipWorks.Data.Administration.Retry
{
    /// <summary>
    /// Options for a SqlAdapterRetry
    /// </summary>
    public class SqlAdapterRetryOptions
    {
        private const int defaultRetryDelayInSeconds = 1;
        private const int defaultRetries = 5;
        private const int defaultDeadlockPriority = -5;

        private static SqlAdapterRetryOptions defaultOptions = new SqlAdapterRetryOptions();

        /// <summary>
        /// Default retry options
        /// </summary>
        public static SqlAdapterRetryOptions Default => defaultOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAdapterRetryOptions(int retryDelayInSeconds = 1, int retries = 5, int deadlockPriority = -5) :
            this(TimeSpan.FromSeconds(retryDelayInSeconds), retries, deadlockPriority)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAdapterRetryOptions(TimeSpan retryDelay, int retries = 5, int deadlockPriority = -5)
        {
            RetryDelay = retryDelay;
            Retries = retries;
            DeadlockPriority = deadlockPriority;
        }

        /// <summary>
        /// Delay between each retry
        /// </summary>
        public TimeSpan RetryDelay { get; }

        /// <summary>
        /// Number of retries before giving up
        /// </summary>
        public int Retries { get; }

        /// <summary>
        /// Deadlock priority
        /// </summary>
        public int DeadlockPriority { get; }
    }
}
