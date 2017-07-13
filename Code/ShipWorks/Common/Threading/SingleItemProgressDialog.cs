using System;
using Interapptive.Shared.Threading;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Dialog with a single progress item
    /// </summary>
    public class SingleItemProgressDialog : ISingleItemProgressDialog
    {
        private readonly IDisposable disposableDialog;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleItemProgressDialog(IDisposable disposableDialog, IProgressReporter progressItem)
        {
            this.disposableDialog = disposableDialog;
            ProgressItem = progressItem;
        }

        /// <summary>
        /// Progress item associated with the dialog
        /// </summary>
        public IProgressReporter ProgressItem { get; }

        /// <summary>
        /// Dispose the progress item
        /// </summary>
        public void Dispose()
        {
            ProgressItem.Completed();
            disposableDialog.Dispose();
        }
    }
}
