﻿using System;
using System.Windows.Forms;
using Interapptive.Shared.Threading;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Display messages to the user without taking a dependency on the UI
    /// </summary>
    public interface IMessageHelper
    {
        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(IWin32Window owner, string message);

        /// <summary>
        /// Show an information message
        /// </summary>
        void ShowInformation(string message);

        /// <summary>
        /// Show a user conditional information message
        /// </summary>
        void ShowUserConditionalInformation(string title, string message, UserConditionalNotificationType notificationType);

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        ISingleItemProgressDialog ShowProgressDialog(string title, string description);

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        IDisposable ShowProgressDialog(string title, string description, IProgressProvider progressProvider, TimeSpan timeSpan);

        /// <summary>
        /// Show a yes/no question with the given text
        /// </summary>
        DialogResult ShowQuestion(string text);

        /// <summary>
        /// Show a popup message
        /// </summary>
        void ShowPopup(string message);

        /// <summary>
        /// Show a popup message with an image
        /// </summary>
        void ShowPopup(string message, char fontAwesomeIcon);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        DialogResult ShowDialog(Func<IForm> createDialog);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        DialogResult ShowDialog(Func<Form> createDialog);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        bool? ShowDialog(IDialog dialog);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        bool? ShowDialog(Func<IDialog> createDialog);

        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        void ShowInformation(IWin32Window owner, string message);

        /// <summary>
        /// Show a question message box.
        /// </summary>
        DialogResult ShowQuestion(MessageBoxIcon icon, MessageBoxButtons buttons, string message);

        /// <summary>
        /// Show a warning message
        /// </summary>
        void ShowWarning(string message);

        /// <summary>
        /// Show a message
        /// </summary>
        void ShowMessage(string message);

        /// <summary>
        /// Set the cursor, then set it back when the result is disposed
        /// </summary>
        IDisposable SetCursor(Cursor waitCursor);
    }
}