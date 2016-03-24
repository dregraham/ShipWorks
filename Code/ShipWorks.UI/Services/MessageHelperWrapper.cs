using System;
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
        private readonly Func<IWin32Window> ownerFactory;
        private readonly ISchedulerProvider schedulerProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageHelperWrapper(Func<IWin32Window> ownerFactory, ISchedulerProvider schedulerProvider)
        {
            this.ownerFactory = ownerFactory;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        public void ShowError(string message)
        {
            schedulerProvider.WindowsFormsEventLoop
                .Schedule(new { Owner = ownerFactory(), Message = message }, (x, y) =>
            {
                MessageHelper.ShowError(y.Owner, y.Message);
                return Disposable.Empty;
            });
        }

        /// <summary>
        /// Show an information message
        /// </summary>
        public void ShowInformation(string message)
        {
            schedulerProvider.WindowsFormsEventLoop
                .Schedule(new { Owner = ownerFactory(), Message = message }, (x, y) =>
                {
                    MessageHelper.ShowInformation(y.Owner, y.Message);
                    return Disposable.Empty;
                });
        }

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

            ProgressDlg progressDialog = new ProgressDlg(progressProvider)
            {
                Title = title,
                Description = description
            };

            progressDialog.Show(ownerFactory());

            return Disposable.Create(() =>
            {
                progressItem.Completed();
                progressDialog.CloseForced();
                progressDialog.Dispose();
            });
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
    }
}
