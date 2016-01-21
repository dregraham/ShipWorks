using System;
using System.Windows.Forms;
using log4net;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Utility class for displaying various messages to the user
    /// </summary>
    public static class MessageHelper
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MessageHelper));

        // The caption to be displayed by all boxes
        static string caption = null;

        /// <summary>
        /// Initialize the caption to use for all messages
        /// </summary>
        public static void Initialize(string caption)
        {
            if (MessageHelper.caption != null)
            {
                throw new InvalidOperationException("The caption has already been initialized.");
            }

            MessageHelper.caption = caption;
        }

        /// <summary>
        /// The caption that has been initialized, or an InvalidOperationException if not initialized.
        /// </summary>
        private static string Caption
        {
            get
            {
                if (caption == null)
                {
                    throw new InvalidOperationException("The caption has not been initialized.");
                }

                return caption;
            }
        }

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        public static void ShowError(IWin32Window owner, string text)
        {
            log.ErrorFormat("ShowError: {0}", text);

            MessageBox.Show(owner,
                text,
                Caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show an information message box with no icon.
        /// </summary>
        public static void ShowMessage(IWin32Window owner, string text)
        {
            log.InfoFormat("ShowMessage: {0}", text);

            MessageBox.Show(owner,
                text,
                Caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.None);
        }

        /// <summary>
        /// Show an information message box with an "i" icon
        /// </summary>
        public static void ShowInformation(IWin32Window owner, string text)
        {
            log.InfoFormat("ShowInformation: {0}", text);

            MessageBox.Show(owner,
                text,
                Caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show a warning message
        /// </summary>
        public static void ShowWarning(IWin32Window owner, string text)
        {
            log.InfoFormat("ShowWarning: {0}", text);

            MessageBox.Show(owner,
                 text,
                 Caption,
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show a question message box.  Return is "OK" for yes.
        /// </summary>
        public static DialogResult ShowQuestion(IWin32Window owner, string text)
        {
            return MessageHelper.ShowQuestion(owner, MessageBoxIcon.Question, text);
        }

        /// <summary>
        /// Show a question message box.  Return is "OK" for yes.
        /// </summary>
        public static DialogResult ShowQuestion(IWin32Window owner, MessageBoxIcon icon, string text)
        {
            log.InfoFormat("ShowQuestion: {0}", text);

            DialogResult result = MessageBox.Show(owner,
                text,
                Caption,
                MessageBoxButtons.OKCancel,
                icon,
                MessageBoxDefaultButton.Button2);

            log.InfoFormat("Response: {0}", result);

            return result;
        }

        /// <summary>
        /// Show a question message box.
        /// </summary>
        public static DialogResult ShowQuestion(IWin32Window owner, MessageBoxIcon icon, MessageBoxButtons buttons, string text)
        {
            log.InfoFormat("ShowQuestion: {0}", text);

            DialogResult result = MessageBox.Show(owner,
                text,
                Caption,
                buttons,
                icon,
                MessageBoxDefaultButton.Button2);

            log.InfoFormat("Response: {0}", result);

            return result;
        }
    }
}
