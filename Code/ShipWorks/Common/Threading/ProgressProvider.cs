using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Manages multiple progress items and their status
    /// </summary>
    public class ProgressProvider : IProgressProvider
    {
        ThreadSafeObservableCollection<IProgressReporter> progressItems = new ThreadSafeObservableCollection<IProgressReporter>();

        bool cancelRequested = false;
        private readonly TaskCompletionSource<Unit> terminatedCompletionSource = new TaskCompletionSource<Unit>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressProvider()
        {
            progressItems.CollectionChanged += OnProgressItemCollectionChanged;
        }

        /// <summary>
        /// The current list of items in progress
        /// </summary>
        public ThreadSafeObservableCollection<IProgressReporter> ProgressItems => progressItems;

        /// <summary>
        /// Indicates if the requested operation has been requested to be canceled.
        /// </summary>
        public bool CancelRequested => cancelRequested;

        /// <summary>
        /// Indicates if the current state of the items allows for cancellation.
        /// </summary>
        public bool CanCancel
        {
            get
            {
                if (cancelRequested)
                {
                    return false;
                }

                foreach (ProgressItem item in progressItems)
                {
                    if ((item.Status == ProgressItemStatus.Running || item.Status == ProgressItemStatus.Pending) && item.CanCancel)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Request cancellation of the operation in progress
        /// </summary>
        public void Cancel()
        {
            if (cancelRequested)
            {
                return;
            }

            cancelRequested = true;

            foreach (ProgressItem item in progressItems)
            {
                item.Cancel();
            }
        }

        /// <summary>
        /// Sets all running items to have the Failure state with the specified exception, and sets all pending items
        /// to terminated.
        /// </summary>
        public void Terminate(Exception ex)
        {
            foreach (ProgressItem item in progressItems)
            {
                if (item.Status == ProgressItemStatus.Running)
                {
                    item.Failed(ex);
                }

                if (item.Status == ProgressItemStatus.Pending)
                {
                    item.Terminate();
                }
            }
        }

        /// <summary>
        /// Indicates that there are no running or pending items
        /// </summary>
        public bool IsComplete
        {
            get
            {
                foreach (ProgressItem item in progressItems)
                {
                    if (item.Status == ProgressItemStatus.Running || item.Status == ProgressItemStatus.Pending)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Indicates if any items in progress have errors
        /// </summary>
        public bool HasErrors
        {
            get
            {
                foreach (ProgressItem item in progressItems)
                {
                    if (item.Status == ProgressItemStatus.Failure || item.Status == ProgressItemStatus.Terminated)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Creates and adds a new ProgressItem of the given name
        /// </summary>
        public IProgressReporter AddItem(string name)
        {
            ProgressItem item = new ProgressItem(name);
            ProgressItems.Add(item);

            return item;
        }

        /// <summary>
        /// Task that completes when the progress provider is terminated
        /// </summary>
        public Task<Unit> Terminated => terminatedCompletionSource.Task;

        /// <summary>
        /// End the progress provider
        /// </summary>
        public void Terminate() => terminatedCompletionSource.TrySetResult(Unit.Default);

        /// <summary>
        /// Called when changes are made to the progress item collection
        /// </summary>
        void OnProgressItemCollectionChanged(object sender, CollectionChangedEventArgs<IProgressReporter> e)
        {
            if (CancelRequested)
            {
                if (e.NewItem != null)
                {
                    e.NewItem.Cancel();
                }
            }
        }

    }
}
