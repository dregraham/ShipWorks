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
    public class BackgroundProgressProvider : IProgressProvider
    {
        private bool cancelRequested = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundProgressProvider(IProgressReporter progressItem)
        {
            ProgressItems = new ThreadSafeObservableCollection<IProgressReporter>();
            ProgressItems.Add(progressItem);
        }

        /// <summary>
        /// The current list of items in progress
        /// </summary>
        public ThreadSafeObservableCollection<IProgressReporter> ProgressItems { get; }

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

                foreach (IProgressReporter item in ProgressItems)
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

            foreach (IProgressReporter item in ProgressItems)
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
            foreach (IProgressReporter item in ProgressItems)
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
                foreach (IProgressReporter item in ProgressItems)
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
                foreach (IProgressReporter item in ProgressItems)
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
        public Task<Unit> Terminated => Task.FromResult(Unit.Default);

        /// <summary>
        /// End the progress provider
        /// </summary>
        public void Terminate()
        {
        }

        /// <summary>
        /// Called when changes are made to the progress item collection
        /// </summary>
        private void OnProgressItemCollectionChanged(object sender, CollectionChangedEventArgs<IProgressReporter> e)
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
