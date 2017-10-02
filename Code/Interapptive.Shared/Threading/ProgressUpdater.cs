using System;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Update progress
    /// </summary>
    public class ProgressUpdater : IProgressUpdater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressUpdater(IProgressReporter progressReporter, int totalItems)
        {
            ProgressReporter = progressReporter;
            TotalItems = totalItems;
        }

        /// <summary>
        /// Get an empty progress updater
        /// </summary>
        public static IProgressUpdater Empty { get; } = new EmptyProgressUpdater();

        /// <summary>
        /// Progress item that is displayed
        /// </summary>
        protected IProgressReporter ProgressReporter { get; }

        /// <summary>
        /// Total items being processed
        /// </summary>
        protected int TotalItems { get; }

        /// <summary>
        /// Count of completed items
        /// </summary>
        protected int Count { get; private set; }

        /// <summary>
        /// Update the progress
        /// </summary>
        public virtual void Update()
        {
            Count = Math.Min(Count + 1, TotalItems);
            ProgressReporter.PercentComplete = (int) ((Count / (double) TotalItems) * 100);
        }
    }
}
