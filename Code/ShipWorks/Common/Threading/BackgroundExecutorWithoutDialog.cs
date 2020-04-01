using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Class that provides functionality to update entities on a background thread
    /// </summary>
    public class BackgroundExecutorWithoutDialog<T> : IBackgroundExecutor<T>
    {
        /// <summary>
        /// Raised on the background thread right before each individual item is processed.
        /// </summary>
        public event EventHandler ExecuteStarting;

        /// <summary>
        /// Raised when execution has completed, but the progress window is still displayed.  This is raised on the background thread.
        /// </summary>
        public event EventHandler ExecuteCompleting;

        /// <summary>
        /// Raised when the execute is complete.  Raised on the UI thread of the owner.
        /// </summary>
        public event BackgroundExecutorCompletedEventHandler<T> ExecuteCompleted;

        #region class OperationState

        class OperationState<TState>
        {
            public List<TState> Items;
            public object UserState;

            public BackgroundExecutorCallback<TState> Worker;

            public TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>> CompletionSource { get; internal set; }
            public TaskScheduler Scheduler { get; internal set; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundExecutorWithoutDialog()
        {

        }

        /// <summary>
        /// Indicates if an exception will be propagated to the ExecuteCompleted event.  If false, then the exception is left unhandled,
        /// and the app will likely crash.  Only set this to true if the hooked up ExecuteCompleted even is prepared to handle exceptions.
        /// </summary>
        public bool PropagateException { get; set; }

        /// <summary>
        /// Execute the operation asynchronously using the given item collection
        /// </summary>
        public Task<BackgroundExecutorCompletedEventArgs<T>> ExecuteAsync(BackgroundExecutorCallback<T> worker, IEnumerable<T> items, object userState)
        {
            MethodConditions.EnsureArgumentIsNotNull(worker, nameof(worker));

            if (items == null)
            {
                throw new ArgumentException("Must specify items when there is no initializer.", "items");
            }

            TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>> completionSource =
                new TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>>();

            // Queue the work.  We copy the items to a new list so changes to the original IEnumerable don't affect us.
            ThreadPool.QueueUserWorkItem(
                ExceptionMonitor.WrapWorkItem(InternalExecute, "working"),
                new OperationState<T>
                {
                    Items = items != null ? items.ToList() : null,
                    UserState = userState,
                    Worker = worker,
                    CompletionSource = completionSource,
                    Scheduler = TaskScheduler.FromCurrentSynchronizationContext()
                });

            return completionSource.Task;
        }

        /// <summary>
        /// Executes on the background thread
        /// </summary>
        private void InternalExecute(object state)
        {
            OperationState<T> operationState = (OperationState<T>) state;

            List<T> itemsToProcess = operationState.Items;

            // Create an issue list each worker to use to append an issue
            List<BackgroundIssue<T>> issues = new List<BackgroundIssue<T>>();
            BackgroundIssueAdder<T> issueAdder = new BackgroundIssueAdder<T>(issues);

            // If exceptions are being propagated and one is thrown, this will be it
            Exception errorEx = null;

            // Could be null if the initializer was canceled
            if (itemsToProcess != null)
            {
                int count = 0;
                int total = itemsToProcess.Count;

                // Raise the starting event, if there are listeners
                ExecuteStarting?.Invoke(this, EventArgs.Empty);

                try
                {
                    // Go through each item in the list
                    foreach (T item in itemsToProcess)
                    {
                        // Execute the work
                        operationState.Worker(item, operationState.UserState, issueAdder);
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    if (PropagateException)
                    {
                        errorEx = ex;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Handler for ExecuteCompleting
            if (errorEx == null)
            {
                try
                {
                    ExecuteCompleting?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    if (PropagateException)
                    {
                        errorEx = ex;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            BackgroundExecutorCompletedEventArgs<T> eventArgs =
                new BackgroundExecutorCompletedEventArgs<T>(false, issues, errorEx, operationState.UserState);

            operationState.CompletionSource.Task.ContinueWith(x =>
            {
                OnExecuteCompleted(eventArgs);
            }, operationState.Scheduler);

            operationState.CompletionSource.SetResult(eventArgs);
        }

        /// <summary>
        /// Execution has completed
        /// </summary>
        private void OnExecuteCompleted(BackgroundExecutorCompletedEventArgs<T> e)
        {
            ExecuteCompleted?.Invoke(this, e);
        }
    }
}
