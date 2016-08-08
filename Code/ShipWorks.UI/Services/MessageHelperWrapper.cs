﻿
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using System;
using System.Reactive.Disposables;
using System.Windows.Forms;

namespace ShipWorks.UI.Services
{
    /// <summary>
    /// Wraps the static message helper class
    /// </summary>
    public class MessageHelperWrapper : IMessageHelper
    {
        private readonly Func<Control> ownerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageHelperWrapper(Func<Control> ownerFactory)
        {
            this.ownerFactory = ownerFactory;
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
    }
}
