using System;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Update progress
    /// </summary>
    public class ProgressUpdater : IProgressUpdater
    {
        private static Lazy<IProgressUpdater> empty = new Lazy<IProgressUpdater>(() => new EmptyProgressUpdater());
        readonly IProgressReporter progressReporter;
        readonly int totalItems;
        int count = 0;

        /// <summary>
        /// Get an empty progress updater
        /// </summary>
        public static IProgressUpdater Empty => empty.Value;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressUpdater(IProgressReporter progressReporter, int totalItems)
        {
            this.progressReporter = progressReporter;
            this.totalItems = totalItems;
        }

        /// <summary>
        /// Update the progress
        /// </summary>
        public void Update()
        {
            count = Math.Min(count + 1, totalItems);
            progressReporter.PercentComplete = (int) ((count / (double) totalItems) * 100);
        }
    }
}
