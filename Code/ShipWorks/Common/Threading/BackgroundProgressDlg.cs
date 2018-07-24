using System;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Simulated progress dialog for use in the background
    /// </summary>
    public class BackgroundProgressDlg : IDisposable
    {
        IProgressProvider progressProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundProgressDlg(IProgressProvider progressProvider)
        {
            this.progressProvider = MethodConditions.EnsureArgumentIsNotNull(progressProvider, nameof(progressProvider));

            UpdateProgress();

            foreach (var item in progressProvider.ProgressItems)
            {
                item.Changed += OnProgressItemChanged;
            }

            progressProvider.ProgressItems.CollectionChanged += OnChangeProgressItems;
        }

        /// <summary>
        /// A change to the collection of progress items
        /// </summary>
        private void OnChangeProgressItems(object sender, CollectionChangedEventArgs<IProgressReporter> e)
        {
            if (e.NewItem != null)
            {
                e.NewItem.Changed += OnProgressItemChanged;
            }

            if (e.OldItem != null)
            {
                e.OldItem.Changed -= OnProgressItemChanged;
            }

            UpdateProgress();
        }

        /// <summary>
        /// A property of one of the ProgressItem objects we are listening on has changed
        /// </summary>
        private void OnProgressItemChanged(object sender, EventArgs e) => UpdateProgress();

        /// <summary>
        /// Close the simulated dialog when the provider is complete
        /// </summary>
        private void UpdateProgress()
        {
            if (progressProvider.IsComplete)
            {
                Close();
            }
        }

        /// <summary>
        /// Close the simulated dialog
        /// </summary>
        private void Close()
        {
            progressProvider.ProgressItems.CollectionChanged -= OnChangeProgressItems;

            foreach (var item in progressProvider.ProgressItems)
            {
                item.Changed -= OnProgressItemChanged;
            }

            progressProvider.Terminate();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => Close();
    }
}
