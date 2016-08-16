using System;
using Interapptive.Shared.Collections;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Manages multiple progress items and their status
    /// </summary>
    public interface IProgressProvider
    {
        /// <summary>
        /// Indicates if the current state of the items allows for cancelation.
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        /// Indicates if the requested operation has been requested to be canceled.
        /// </summary>
        bool CancelRequested { get; }

        /// <summary>
        /// Indicates if any items in progress have errors
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Indicates that there are no running or pending items
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// The current list of items in progress
        /// </summary>
        ObservableCollection<IProgressReporter> ProgressItems { get; }

        /// <summary>
        /// Creates and adds a new ProgressItem of the given name
        /// </summary>
        IProgressReporter AddItem(string name);

        /// <summary>
        /// Request cancelation of the operation in progress
        /// </summary>
        void Cancel();

        /// <summary>
        /// Sets all running items to have the Failure state with the specified exception, and sets all pending items
        /// to terminated.
        /// </summary>
        void Terminate(Exception error);
    }
}