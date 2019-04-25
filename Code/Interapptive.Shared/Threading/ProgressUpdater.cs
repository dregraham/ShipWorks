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
        /// Update the progress by a single item
        /// </summary>
        public virtual void Update() => Update(1);

        /// <summary>
        /// Update the progress by the specified amount
        /// </summary>
        public virtual void Update(int finishedCount)
        {
            if (TotalItems > 0)
            {
                Count = Math.Min(Count + finishedCount, TotalItems);
                ProgressReporter.PercentComplete = (int) ((Count / (double) TotalItems) * 100);
            }
        }
    }
}
