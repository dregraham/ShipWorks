using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Class that provides functionality to update entities on a background thread
    /// </summary>
    public class BackgroundExecutor<T>
    {
        Control owner;

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

        // Information to be displayed in the progress window that is shown
        string progressTitle;
        string progressDescription;
        string progressDetail;

        // Whether to delay showing the progress dialog
        private readonly bool delayProgressDialog;

        #region class OperationState

        class OperationState<TState>
        {
            public List<TState> Items;
            public object UserState;

            public ProgressProvider ProgressProvider;

            public Func<IProgressReporter, List<T>> Initializer;
            public ProgressItem InitializerProgress;

            public BackgroundExecutorCallback<TState> Worker;
            public ProgressItem WorkProgress;

            public ProgressDisplayDelayer DisplayDelayer;

            public TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>> CompletionSource { get; internal set; }
            public TaskScheduler Scheduler { get; internal set; }
        }

        #endregion

        /// <summary>
        /// Create a new instance of the background updater
        /// </summary>
        public BackgroundExecutor(Control owner, string progressTitle, string progressDescription, string progressDetail)
            : this(owner, progressTitle, progressDescription, progressDetail, true)
        { }

        /// <summary>
        /// Create a new instance of the background updater
        /// </summary>
        public BackgroundExecutor(Control owner, string progressTitle, string progressDescription, string progressDetail, bool delayProgressDialog)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;

            this.progressTitle = progressTitle;
            this.progressDescription = progressDescription;
            this.progressDetail = progressDetail;
            this.delayProgressDialog = delayProgressDialog;
        }

        /// <summary>
        /// Indicates if an exception will be propagated to the ExecuteCompleted event.  If false, then the exception is left unhandled,
        /// and the app will likely crash.  Only set this to true if the hooked up ExecuteCompleted even is prepared to handle exceptions.
        /// </summary>
        public bool PropagateException { get; set; }

        /// <summary>
        /// Execute the operation asynchronously using the given entity key collection
        /// </summary>
        public void ExecuteAsync(BackgroundExecutorCallback<T> worker, IEnumerable<T> items)
        {
            ExecuteAsync(worker, items, null);
        }

        /// <summary>
        /// Execute the operation asynchronously using the given item collection
        /// </summary>
        public Task<BackgroundExecutorCompletedEventArgs<T>> ExecuteAsync(BackgroundExecutorCallback<T> worker, IEnumerable<T> items, object userState) =>
            ExecuteAsync(null, worker, items, userState);

        /// <summary>
        /// Execute the operation asynchronously using the given entity key collection
        /// </summary>
        public void ExecuteAsync(Func<IProgressReporter, List<T>> initializer, BackgroundExecutorCallback<T> worker)
        {
            ExecuteAsync(initializer, worker, null);
        }

        /// <summary>
        /// Execute the operation asynchronously using the given item collection
        /// </summary>
        public void ExecuteAsync(Func<IProgressReporter, List<T>> initializer, BackgroundExecutorCallback<T> worker, object userState)
        {
            ExecuteAsync(initializer, worker, null, userState);
        }

        /// <summary>
        /// Execute the operation asynchronously using the given item collection
        /// </summary>
        private Task<BackgroundExecutorCompletedEventArgs<T>> ExecuteAsync(Func<IProgressReporter, List<T>> initializer, BackgroundExecutorCallback<T> worker, IEnumerable<T> items, object userState)
        {
            if (worker == null)
            {
                throw new ArgumentNullException("worker");
            }

            if (items == null)
            {
                if (initializer == null)
                {
                    throw new ArgumentException("Must specify items when there is no initializer.", "items");
                }
            }
            else
            {
                if (initializer != null)
                {
                    throw new ArgumentException("Cannot specify both items and an initializer. The initializer supplies the items.");
                }
            }

            // Progress Provider
            ProgressProvider progressProvider = new ProgressProvider();

            // Initialization progress (if provided)
            ProgressItem initializerProgress = null;
            if (initializer != null)
            {
                initializerProgress = new ProgressItem("Preparing");
                progressProvider.ProgressItems.Add(initializerProgress);
            }

            // Progress Item
            ProgressItem workProgress = new ProgressItem(progressTitle);
            progressProvider.ProgressItems.Add(workProgress);

            // Progress Dialog
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = progressTitle;
            progressDlg.Description = progressDescription;

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>> completionSource =
                new TaskCompletionSource<BackgroundExecutorCompletedEventArgs<T>>();

            // Queue the work.  We copy the items to a new list so changes to the original IEnumerable don't affect us.
            ThreadPool.QueueUserWorkItem(
                ExceptionMonitor.WrapWorkItem(InternalExecute, "working"),
                new OperationState<T>
                {
                    Items = items != null ? items.ToList() : null,
                    UserState = userState,
                    ProgressProvider = progressProvider,
                    Initializer = initializer,
                    InitializerProgress = initializerProgress,
                    Worker = worker,
                    WorkProgress = workProgress,
                    DisplayDelayer = delayer,
                    CompletionSource = completionSource,
                    Scheduler = TaskScheduler.FromCurrentSynchronizationContext()
                });

            // Show the progress window only after a certain amount of time goes by
            // if configured to do so
            double delaySeconds = delayProgressDialog ? .25 : 0;
            delayer.ShowAfter(owner, TimeSpan.FromSeconds(delaySeconds));

            return completionSource.Task;
        }

        /// <summary>
        /// Executes on the background thread
        /// </summary>
        [NDependIgnoreLongMethod]
        private void InternalExecute(object state)
        {
            OperationState<T> operationState = (OperationState<T>) state;

            List<T> itemsToProcess = null;

            // First do the initializer, if it exists
            if (operationState.Initializer != null)
            {
                operationState.InitializerProgress.Starting();
                operationState.InitializerProgress.Detail = "Preparing for operation...";

                itemsToProcess = operationState.Initializer(operationState.InitializerProgress);

                if (itemsToProcess != null)
                {
                    operationState.InitializerProgress.PercentComplete = 100;
                    operationState.InitializerProgress.Detail = "Done";
                    operationState.InitializerProgress.Completed();
                }
                else
                {
                    operationState.ProgressProvider.Cancel();
                }
            }
            else
            {
                itemsToProcess = operationState.Items;
            }

            // Create an issue list each worker to use to append an issue
            List<BackgroundIssue<T>> issues = new List<BackgroundIssue<T>>();
            BackgroundIssueAdder<T> issueAdder = new BackgroundIssueAdder<T>(issues);

            // If exceptions are being propagated and one is thrown, this will be it
            Exception errorEx = null;

            // Could be null if the initializer was canceled
            if (itemsToProcess != null)
            {
                // For progress updating
                ProgressItem workProgress = operationState.WorkProgress;

                int count = 0;
                int total = itemsToProcess.Count;

                workProgress.Starting();

                // Raise the starting event, if there is listeners
                ExecuteStarting?.Invoke(this, EventArgs.Empty);

                try
                {
                    // Go through each item in the list
                    foreach (T item in itemsToProcess)
                    {
                        if (workProgress.IsCancelRequested)
                        {
                            break;
                        }

                        workProgress.Detail = string.Format(progressDetail, count + 1, total);

                        // Execute the work
                        operationState.Worker(item, operationState.UserState, issueAdder);
                        count++;

                        workProgress.PercentComplete = (100 * count) / total;
                    }

                    // Mark the progress as complete
                    workProgress.Completed();
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
                new BackgroundExecutorCompletedEventArgs<T>(operationState.WorkProgress.IsCancelRequested, issues, errorEx, operationState.UserState);

            operationState.CompletionSource.Task.ContinueWith(x =>
            {
                OnExecuteCompleted(eventArgs, operationState.DisplayDelayer);
            }, operationState.Scheduler);

            operationState.CompletionSource.SetResult(eventArgs);
        }

        /// <summary>
        /// Execution has completed
        /// </summary>
        private void OnExecuteCompleted(BackgroundExecutorCompletedEventArgs<T> e, ProgressDisplayDelayer delayer)
        {
            delayer.NotifyComplete();

            ExecuteCompleted?.Invoke(this, e);
        }
    }
}
