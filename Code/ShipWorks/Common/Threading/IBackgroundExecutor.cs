using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Interface that provides functionality to update entities on a background thread
    /// </summary>
    public interface IBackgroundExecutor<T>
    {
        /// <summary>
        /// Indicates if an exception will be propagated to the ExecuteCompleted event.  If false, then the exception is left unhandled,
        /// and the app will likely crash.  Only set this to true if the hooked up ExecuteCompleted even is prepared to handle exceptions.
        /// </summary>
        bool PropagateException { get; set; }

        /// <summary>
        /// Raised on the background thread right before each individual item is processed.
        /// </summary>
        event EventHandler ExecuteStarting;

        /// <summary>
        /// Raised when execution has completed, but the progress window is still displayed.  This is raised on the background thread.
        /// </summary>
        event EventHandler ExecuteCompleting;

        // <summary>
        /// Execute the operation asynchronously using the given item collection
        /// </summary>
        Task<BackgroundExecutorCompletedEventArgs<T>> ExecuteAsync(BackgroundExecutorCallback<T> worker, IEnumerable<T> items, object userState);
    }
}
