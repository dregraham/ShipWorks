using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Windows.Forms;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;

namespace ShipWorks.UI.Services
{
    /// <summary>
    /// Wraps the static message helper class
    /// </summary>
    public class MessageHelperWrapper : IMessageHelper
    {
        private readonly Func<Control> ownerFactory;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageHelperWrapper(Func<Control> ownerFactory, ISchedulerProvider schedulerProvider)
        {
            this.ownerFactory = ownerFactory;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        public void ShowError(string message) => MessageHelper.ShowError(ownerFactory(), message);

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        public void ShowError(IWin32Window owner, string message) => MessageHelper.ShowError(owner, message);

        /// <summary>
        /// Show an information message
        /// </summary>
        public void ShowInformation(string message) => MessageHelper.ShowInformation(ownerFactory(), message);

        /// <summary>
        /// Show a message
        /// </summary>
        public void ShowMessage(string message) => MessageHelper.ShowMessage(ownerFactory(), message);

        /// <summary>
        /// Show a yes/no question with the given text
        /// </summary>
        public DialogResult ShowQuestion(string message)
        {
            return MessageHelper.ShowQuestion(ownerFactory(), message);
        }

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        public IDisposable ShowProgressDialog(string title, string description)
        {
            ProgressItem progressItem = new ProgressItem(title);
            progressItem.Starting();

            ProgressProvider progressProvider = new ProgressProvider
            {
                ProgressItems = { progressItem }
            };

            IDisposable disposable = ShowProgressDialog(title, description, progressProvider, TimeSpan.Zero);

            return Disposable.Create(() =>
            {
                progressItem.Completed();
                disposable.Dispose();
            });
        }

        /// <summary>
        /// Show a new progress dialog with the given provider
        /// </summary>
        public IDisposable ShowProgressDialog(string title, string description, IProgressProvider progressProvider, TimeSpan timeSpan)
        {
            ProgressDlg progressDialog = new ProgressDlg(progressProvider)
            {
                Title = title,
                Description = description
            };

            return schedulerProvider.WindowsFormsEventLoop
                .Schedule(progressDialog, timeSpan, OpenProgressDialog);
        }

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        public DialogResult ShowDialog(Func<Form> createDialog)
        {
            using (Form dlg = createDialog())
            {
                return dlg.ShowDialog(ownerFactory());
            }
        }

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        public DialogResult ShowDialog(Func<IForm> createDialog)
        {
            Control owner = ownerFactory();
            if (owner.InvokeRequired)
            {
                return (DialogResult) owner.Invoke((Func<Func<IForm>, DialogResult>) (ShowDialog), createDialog);
            }

            using (IForm dlg = createDialog())
            {
                return dlg.ShowDialog(ownerFactory());
            }
        }

        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        public void ShowInformation(IWin32Window owner, string message) => MessageHelper.ShowInformation(owner, message);

        /// <summary>
        /// Show a question message box.
        /// </summary>
        public DialogResult ShowQuestion(MessageBoxIcon icon, MessageBoxButtons buttons, string message)
            => MessageHelper.ShowQuestion(ownerFactory(), icon, buttons, message);

        /// <summary>
        /// Show a warning message
        /// </summary>
        public void ShowWarning(string message) => MessageHelper.ShowWarning(ownerFactory(), message);

        /// <summary>
        /// Close the given progress dialog
        /// </summary>
        private IDisposable OpenProgressDialog(IScheduler scheduler, ProgressDlg dialog)
        {
            dialog.Show(ownerFactory());

            return Disposable.Create(() => scheduler.Schedule(dialog, CloseProgressDialog));
        }

        /// <summary>
        /// Close the given progress dialog
        /// </summary>
        private static IDisposable CloseProgressDialog(IScheduler _, ProgressDlg dialog)
        {
            dialog?.CloseForced();
            dialog?.Dispose();

            return Disposable.Empty;
        }

        /// <summary>
        /// Set the cursor, then set it back when the result is disposed
        /// </summary>
        public IDisposable SetCursor(Cursor cursor)
        {
            Cursor current = Cursor.Current;
            Cursor.Current = cursor;

            return Disposable.Create(() => Cursor.Current = current);
        }
    }
}
