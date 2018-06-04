using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;

namespace ShipWorks.UI
{
    /// <summary>
    /// Message helper that calls all methods on the UI thread
    /// </summary>
    /// <remarks>
    /// All the methods return tasks that will resolve when the UI operation completes
    /// </remarks>
    [Component(RegistrationType.Self)]
    public class BackgroundAsyncMessageHelper : IAsyncMessageHelper
    {
        /// <summary>
        /// Show a message box with the given text.
        /// </summary>
        /// <param name="message">Message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        public Task ShowMessage(string message) => Task.CompletedTask;

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        /// <param name="message">Error message that should be displayed</param>
        /// <returns>
        /// Task that will complete when the dialog is closed
        /// </returns>
        public Task ShowError(string message) => Task.CompletedTask;

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        /// <param name="createDialog">Create the dialog that should be shown</param>
        /// <returns>Returns the result of the dialog</returns>
        /// <remarks>The createDialog func will be called on the UI thread</remarks>
        public Task<bool?> ShowDialog(Func<IDialog> createDialog) => Task.FromResult<bool?>(true);

        /// <summary>
        /// Set the cursor, then set it back when the result is disposed
        /// </summary>
        /// <param name="cursor">Cursor that should be shown</param>
        /// <returns>
        /// Disposable that will restore the original cursor when disposed
        /// </returns>
        public Task<IDisposable> SetCursor(Cursor cursor) => Task.FromResult(Disposable.Empty);

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        public Task<ISingleItemProgressDialog> ShowProgressDialog(string title, string description) =>
            Task.FromResult<ISingleItemProgressDialog>(new BackgroundSingleItemProgressDialog());

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        public Task<IDisposable> ShowProgressDialog(string title, string description, IProgressProvider progressProvider, TimeSpan timeSpan) =>
            Task.FromResult(Disposable.Empty);

        /// <summary>
        /// Create a progress provider
        /// </summary>
        public IProgressProvider CreateProgressProvider() => new ProgressProvider();
    }
}
