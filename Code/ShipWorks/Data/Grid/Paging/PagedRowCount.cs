using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Represents a row count - whether complete or not - of a gateway
    /// </summary>
    public class PagedRowCount
    {
        int count;
        bool loadingComplete;

        [ThreadStatic]
        static Stopwatch waitingElapsed;

        [ThreadStatic]
        static TimeSpan waitingTimeout;

        /// <summary>
        /// Constructor
        /// </summary>
        public PagedRowCount(int count, bool loadingComplete)
        {
            this.count = count;
            this.loadingComplete = loadingComplete;
        }

        /// <summary>
        /// The count of rows in the gateway
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Indicatse if the rows in the gateway are still loading and the count may continue to grow
        /// </summary>
        public bool LoadingComplete
        {
            get { return loadingComplete; }
        }
        
        /// <summary>
        /// Sets a timeout flag to wait for "LoadingComplete" when getting PagedRowCount instances.
        /// </summary>
        public static void StartWaitingForComplete(TimeSpan timeout)
        {
            waitingTimeout = timeout;
            waitingElapsed = Stopwatch.StartNew();
        }

        /// <summary>
        /// Indicates if we are still supposed to be waiting for "LoadingComplete" when fetching PagedRowCount instances
        /// </summary>
        public static bool IsStillWaitingForComplete
        {
            get
            {
                return waitingElapsed != null && waitingElapsed.Elapsed < waitingTimeout;
            }
        }
    }
}
