using System;
using System.Diagnostics;
using System.Threading;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Represents a row count - whether complete or not - of a gateway
    /// </summary>
    public class PagedRowCount
    {
        static AsyncLocal<Stopwatch> waitingElapsed = new AsyncLocal<Stopwatch>();
        static AsyncLocal<TimeSpan> waitingTimeout = new AsyncLocal<TimeSpan>();

        /// <summary>
        /// Constructor
        /// </summary>
        public PagedRowCount(int count, bool loadingComplete)
        {
            Count = count;
            LoadingComplete = loadingComplete;
        }

        /// <summary>
        /// The count of rows in the gateway
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Indicates if the rows in the gateway are still loading and the count may continue to grow
        /// </summary>
        public bool LoadingComplete { get; }

        /// <summary>
        /// Sets a timeout flag to wait for "LoadingComplete" when getting PagedRowCount instances.
        /// </summary>
        public static void StartWaitingForComplete(TimeSpan timeout)
        {
            waitingTimeout.Value = timeout;
            waitingElapsed.Value = Stopwatch.StartNew();
        }

        /// <summary>
        /// Indicates if we are still supposed to be waiting for "LoadingComplete" when fetching PagedRowCount instances
        /// </summary>
        public static bool IsStillWaitingForComplete
        {
            get
            {
                return waitingElapsed.Value != null && waitingElapsed.Value.Elapsed < waitingTimeout.Value;
            }
        }
    }
}
