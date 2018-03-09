using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Threading;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Message helper that calls all methods on the UI thread
    /// </summary>
    /// <remarks>
    /// All the methods return tasks that will resolve when the UI operation completes
    /// </remarks>
    public interface IAsyncMessageHelper
    {
        /// <summary>
        /// Show a message box with the given text.
        /// </summary>
        /// <param name="message">Message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        Task ShowMessage(string message);

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        /// <param name="message">Error message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        Task ShowError(string message);

        /// <summary>
        /// Set the cursor, then set it back when the result is disposed
        /// </summary>
        /// <param name="cursor">Cursor that should be shown</param>
        /// <returns>
        /// Disposable that will restore the original cursor when disposed
        /// </returns>
        Task<IDisposable> SetCursor(Cursor waitCursor);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        /// <param name="createDialog">Create the dialog that should be shown</param>
        /// <returns>Returns the result of the dialog</returns>
        /// <remarks>The createDialog func will be called on the UI thread</remarks>
        Task<bool?> ShowDialog(Func<IDialog> createDialog);

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        Task<ISingleItemProgressDialog> ShowProgressDialog(string title, string description);
    }
}