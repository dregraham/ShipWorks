﻿using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.UI.Dialogs.Popup;
using ShipWorks.Users;

namespace ShipWorks.UI.Services
{
    /// <summary>
    /// Wraps the static message helper class
    /// </summary>
    [Component]
    public class MessageHelperWrapper : IMessageHelper
    {
        private readonly Func<Control> ownerFactory;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly Func<IUserConditionalNotification> createUserConditionalNotification;
        private readonly Func<IPopupViewModel> popupViewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageHelperWrapper(Func<Control> ownerFactory,
            ISchedulerProvider schedulerProvider,
            Func<IUserConditionalNotification> createUserConditionalNotification,
            Func<IPopupViewModel> popupViewModelFactory)
        {
            this.createUserConditionalNotification = createUserConditionalNotification;
            this.popupViewModelFactory = popupViewModelFactory;
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
        /// Show a user conditional information message
        /// </summary>
        public void ShowUserConditionalInformation(string title, string message, UserConditionalNotificationType notificationType) =>
            createUserConditionalNotification().Show(this, title, message, notificationType);

        /// <summary>
        /// Show a message
        /// </summary>
        public void ShowMessage(string message) => ShowMessage(ownerFactory(), message);

        /// <summary>
        /// Show a message
        /// </summary>
        public void ShowMessage(IWin32Window owner, string message) => MessageHelper.ShowMessage(owner, message);

        /// <summary>
        /// Show a yes/no question with the given text
        /// </summary>
        public DialogResult ShowQuestion(string message)
        {
            Control owner = ownerFactory();

            if (owner.InvokeRequired)
            {
                return (DialogResult) owner.Invoke((Func<string, DialogResult>) ShowQuestion, message);
            }
            else
            {
                return MessageHelper.ShowQuestion(ownerFactory(), message);
            }
        }

        /// <summary>
        /// Show a popup message
        /// </summary>
        public void ShowPopup(string message)
        {
            Control owner = ownerFactory();

            if (owner.InvokeRequired)
            {
                owner.Invoke((Action<string>) ShowPopup, message);
            }
            else
            {
                popupViewModelFactory().Show(message, owner);
            }
        }

        /// <summary>
        /// Show a popup message with a Keyboard Icon
        /// </summary>
        public void ShowKeyboardPopup(string message)
        {
            Control owner = ownerFactory();

            if (owner.InvokeRequired)
            {
                owner.Invoke((Action<string>) ShowPopup, message);
            }
            else
            {
                popupViewModelFactory().ShowWithKeyboard(message, owner);
            }
        }

        /// <summary>
        /// Show a popup message with a barcode icon
        /// </summary>
        public void ShowBarcodePopup(string message)
        {
            Control owner = ownerFactory();

            if (owner.InvokeRequired)
            {
                owner.Invoke((Action<string>) ShowPopup, message);
            }
            else
            {
                popupViewModelFactory().ShowWithBarcode(message, owner);
            }
        }

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        public ISingleItemProgressDialog ShowProgressDialog(string title, string description)
        {
            ProgressItem progressItem = new ProgressItem(title);
            progressItem.Starting();

            ProgressProvider progressProvider = new ProgressProvider
            {
                ProgressItems = { progressItem }
            };

            IDisposable disposable = ShowProgressDialog(title, description, progressProvider, TimeSpan.Zero);

            return new SingleItemProgressDialog(disposable, progressItem);
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
                .Schedule(Tuple.Create(progressDialog, ownerFactory()), timeSpan, OpenProgressDialog);
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
                return (DialogResult) owner.Invoke((Func<Func<IForm>, DialogResult>) ShowDialog, createDialog);
            }

            using (IForm dlg = createDialog())
            {
                return dlg.ShowDialog(ownerFactory());
            }
        }

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        public bool? ShowDialog(Func<IDialog> createDialog)
        {
            Control owner = ownerFactory();
            if (owner.InvokeRequired)
            {
                return (bool?) owner.Invoke((Func<Func<IDialog>, bool?>) ShowDialog, createDialog);
            }

            IDialog dlg = createDialog();
            return ShowDialog(dlg);
        }

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        public bool? ShowDialog(IDialog dialog)
        {
            Control owner = ownerFactory();
            if (owner.InvokeRequired)
            {
                return (bool?) owner.Invoke((Func<IDialog, bool?>) ShowDialog, dialog);
            }

            dialog.LoadOwner(owner);

            return dialog.ShowDialog();
        }

        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        public void ShowInformation(IWin32Window owner, string message) => MessageHelper.ShowInformation(owner, message);

        /// <summary>
        /// Show a question message box.
        /// </summary>
        public DialogResult ShowQuestion(MessageBoxIcon icon, MessageBoxButtons buttons, string message) =>
            MessageHelper.ShowQuestion(ownerFactory(), icon, buttons, message);

        /// <summary>
        /// Show a warning message
        /// </summary>
        public void ShowWarning(string message) => MessageHelper.ShowWarning(ownerFactory(), message);

        /// <summary>
        /// Close the given progress dialog
        /// </summary>
        private IDisposable OpenProgressDialog(IScheduler scheduler, Tuple<ProgressDlg, Control> state)
        {
            ProgressDlg dialog = state.Item1;
            Control owner = state.Item2;

            dialog.Show(owner);

            return Disposable.Create(() => CloseProgressDialog(owner, dialog));
        }

        /// <summary>
        /// Close the given progress dialog
        /// </summary>
        private static void CloseProgressDialog(Control owner, ProgressDlg dialog)
        {
            if (owner.InvokeRequired)
            {
                owner.Invoke((Action<Control, ProgressDlg>) CloseProgressDialog, owner, dialog);
            }

            dialog?.CloseForced();
            dialog?.Dispose();
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
