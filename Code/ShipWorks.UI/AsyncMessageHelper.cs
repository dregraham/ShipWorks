using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;

namespace ShipWorks.UI
{
    /// <summary>
    /// Message helper that calls all methods on the UI thread
    /// </summary>
    /// <remarks>
    /// All the methods return tasks that will resolve when the UI operation completes
    /// </remarks>
    [Component]
    public class AsyncMessageHelper : IAsyncMessageHelper
    {
        readonly Func<Control> ownerFactory;
        readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerFactory">Get the owner of the UI operation</param>
        /// <param name="messageHelper">Message helper that will be used for most operations</param>
        public AsyncMessageHelper(Func<Control> ownerFactory, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.ownerFactory = ownerFactory;
        }

        /// <summary>
        /// Show a message box with the given text.
        /// </summary>
        /// <param name="message">Message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        public Task ShowMessage(string message)
        {
            var owner = ownerFactory();
            return owner.InvokeAsync(() => messageHelper.ShowMessage(owner, message));
        }

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        /// <param name="message">Error message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        public Task ShowError(string message)
        {
            var owner = ownerFactory();
            return owner.InvokeAsync(() => messageHelper.ShowError(owner, message));
        }

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        /// <param name="createDialog">Create the dialog that should be shown</param>
        /// <returns>Returns the result of the dialog</returns>
        /// <remarks>The createDialog func will be called on the UI thread</remarks>
        public Task<bool?> ShowDialog(Func<IDialog> createDialog)
        {
            var owner = ownerFactory();
            return owner.InvokeAsync(() => ShowDialog(owner, createDialog));
        }

        /// <summary>
        /// Set the cursor, then set it back when the result is disposed
        /// </summary>
        /// <param name="cursor">Cursor that should be shown</param>
        /// <returns>
        /// Disposable that will restore the original cursor when disposed
        /// </returns>
        public Task<IDisposable> SetCursor(Cursor cursor) =>
            ownerFactory().InvokeAsync(() => messageHelper.SetCursor(cursor));

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        public Task<ISingleItemProgressDialog> ShowProgressDialog(string title, string description) =>
            ownerFactory().InvokeAsync(() => messageHelper.ShowProgressDialog(title, description));

        /// <summary>
        /// Show a dialog
        /// </summary>
        /// <param name="owner">Owner of the dialog</param>
        /// <param name="createDialog">Func that creates the dialog that should be shown</param>
        /// <returns></returns>
        private bool? ShowDialog(Control owner, Func<IDialog> createDialog)
        {
            var dialog = createDialog();
            dialog.LoadOwner(owner);
            return dialog.ShowDialog();
        }
    }
}
