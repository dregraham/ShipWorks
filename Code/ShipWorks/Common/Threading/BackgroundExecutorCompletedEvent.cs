using System;
using System.Collections.Generic;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Delegate signature for the BackgroundExecutorCompleted event
    /// </summary>
    public delegate void BackgroundExecutorCompletedEventHandler<T>(object sender, BackgroundExecutorCompletedEventArgs<T> e);

    /// <summary>
    /// EventArgs for the BackgroundExecutorCompleted event
    /// </summary>
    public class BackgroundExecutorCompletedEventArgs<T> : EventArgs
    {
        bool canceled;
        List<BackgroundIssue<T>> issues;
        Exception errorEx;
        object userState;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundExecutorCompletedEventArgs(bool canceled, List<BackgroundIssue<T>> issues, Exception errorEx, object userState)
        {
            this.canceled = canceled;
            this.issues = issues;
            this.errorEx = errorEx;
            this.userState = userState;
        }

        /// <summary>
        /// Indicates if the operation was canceled
        /// </summary>
        public bool Canceled
        {
            get { return canceled; }
        }

        /// <summary>
        /// Problems and issues that occurred during processing.  Added by and specific to the worker callback.
        /// </summary>
        public List<BackgroundIssue<T>> Issues
        {
            get { return issues; }
        }

        /// <summary>
        /// Exception thrown during background execution.  Will only be set if PropagateException was set on the BackgroundExecutor.
        /// </summary>
        public Exception ErrorException
        {
            get { return errorEx; }
        }

        /// <summary>
        /// User state object provided to the original operation
        /// </summary>
        public object UserState
        {
            get { return userState; }
        }
    }
}
