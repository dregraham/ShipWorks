using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Dialog with a single progress item
    /// </summary>
    public interface ISingleItemProgressDialog : IDisposable
    {
        /// <summary>
        /// Progress item associated with the dialog
        /// </summary>
        IProgressReporter ProgressItem { get; }

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        IProgressUpdater ToUpdater(int totalItems);

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        IProgressUpdater ToUpdater<T>(IEnumerable<T> items);

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        IProgressUpdater ToUpdater(int totalItems, string detailFormat);

        /// <summary>
        /// Get a progress updater from this progress reporter
        /// </summary>
        IProgressUpdater ToUpdater<T>(IEnumerable<T> items, string detailFormat);
    }
}
