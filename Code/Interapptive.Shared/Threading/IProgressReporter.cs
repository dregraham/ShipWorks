using System;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Interface for operations that are able to report progress
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// The detailed progress message
        /// </summary>
        string Detail { get; set; }

        /// <summary>
        /// The percent complete of the progress, from 0 to 100
        /// </summary>
        int PercentComplete { get; set; }

        /// <summary>
        /// Indicates if the user has requested cancelation
        /// </summary>
        bool IsCancelRequested { get; }

        /// <summary>
        /// Called to indicate the progress item is now running
        /// </summary>
        void Starting();

        /// <summary>
        /// Used to indicate the task finished successfully, or due to cancel.
        /// </summary>
        void Completed();

        /// <summary>
        /// Called when the task finishes due to an error
        /// </summary>
        void Failed(Exception ex);

        /// <summary>
        /// The current status of the progres
        /// </summary>
        ProgressItemStatus Status { get; }

        /// <summary>
        /// Indicates if this operation can be canceled
        /// </summary>
        bool CanCancel { get; set; }

        /// <summary>
        /// Cancel the current progress reporter
        /// </summary>
        void Cancel();
    }
}
