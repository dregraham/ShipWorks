using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Dialog with a single progress item
    /// </summary>
    public class BackgroundSingleItemProgressDialog : ISingleItemProgressDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundSingleItemProgressDialog()
        {
            ProgressItem = new ProgressItem("Background progress item");
        }

        /// <summary>
        /// Progress item associated with the dialog
        /// </summary>
        public IProgressReporter ProgressItem { get; }

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        public IProgressUpdater ToUpdater(int totalItems) =>
            new ProgressUpdater(ProgressItem, totalItems);

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        public IProgressUpdater ToUpdater<T>(IEnumerable<T> items) =>
            ToUpdater(items.Count());

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        public IProgressUpdater ToUpdater(int totalItems, string detailFormat) =>
            new DetailProgressUpdater(ProgressItem, totalItems, detailFormat);

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        public IProgressUpdater ToUpdater<T>(IEnumerable<T> items, string detailFormat) =>
            ToUpdater(items.Count(), detailFormat);

        /// <summary>
        /// Dispose the progress item
        /// </summary>
        public void Dispose()
        {

        }
    }
}
