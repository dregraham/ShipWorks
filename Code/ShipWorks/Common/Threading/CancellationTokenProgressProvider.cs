using System;
using System.Linq;
using System.Threading;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Manages multiple progress items and their status
    /// </summary>
    public class CancellationTokenProgressProvider : IProgressProvider
    {
        ObservableCollection<IProgressReporter> progressItems = new ObservableCollection<IProgressReporter>();

        readonly CancellationTokenSource cancellationSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public CancellationTokenProgressProvider(CancellationTokenSource cancellationSource)
        {
            this.cancellationSource = cancellationSource;
            cancellationSource.Token.Register(CancelItems);
            progressItems.CollectionChanged += OnProgressItemCollectionChanged;
        }

        /// <summary>
        /// The current list of items in progress
        /// </summary>
        public ObservableCollection<IProgressReporter> ProgressItems => progressItems;

        /// <summary>
        /// Indicates if the requested operation has been requested to be canceled.
        /// </summary>
        public bool CancelRequested => cancellationSource.IsCancellationRequested;

        /// <summary>
        /// Indicates if the current state of the items allows for cancellation.
        /// </summary>
        public bool CanCancel
        {
            get
            {
                if (cancellationSource.IsCancellationRequested)
                {
                    return false;
                }

                return progressItems.Any(x => (x.Status == ProgressItemStatus.Running || x.Status == ProgressItemStatus.Pending) && x.CanCancel);
            }
        }

        /// <summary>
        /// Request cancellation of the operation in progress
        /// </summary>
        public void Cancel() => cancellationSource.Cancel();

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
        public bool IsComplete =>
            progressItems.None(x => x.Status == ProgressItemStatus.Running || x.Status == ProgressItemStatus.Pending);

        /// <summary>
        /// Indicates if any items in progress have errors
        /// </summary>
        public bool HasErrors =>
            progressItems.Any(x => x.Status == ProgressItemStatus.Failure || x.Status == ProgressItemStatus.Terminated);

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

        /// <summary>
        /// Handle cancellation
        /// </summary>
        private void CancelItems()
        {
            foreach (ProgressItem item in progressItems)
            {
                item.Cancel();
            }
        }
    }
}
