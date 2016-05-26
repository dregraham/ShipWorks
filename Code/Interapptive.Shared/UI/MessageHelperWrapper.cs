using System;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Wraps the static message helper class
    /// </summary>
    public class MessageHelperWrapper : IMessageHelper
    {
        private readonly Func<IWin32Window> ownerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageHelperWrapper(Func<IWin32Window> ownerFactory)
        {
            this.ownerFactory = ownerFactory;
        }

        /// <summary>
        /// Show an error message box with the given error text.
        /// </summary>
        public void ShowError(string message) => MessageHelper.ShowError(ownerFactory(), message);

        /// <summary>
        /// Show an information message
        /// </summary>
        public void ShowInformation(string message) => MessageHelper.ShowMessage(ownerFactory(), message);

        /// <summary>
        /// Show an error, takes an owner
        /// </summary>
        public void ShowError(IWin32Window owner, string message)
        {
            MessageHelper.ShowError(owner, message);
        }

        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        public void ShowInformation(IWin32Window owner, string message)
        {
            MessageHelper.ShowMessage(owner, message);
        }

        /// <summary>
        /// Show a question message box.  
        /// </summary>
        public DialogResult ShowQuestion(MessageBoxIcon icon, MessageBoxButtons buttons, string message)
            => MessageHelper.ShowQuestion(ownerFactory(), icon, buttons, message);
    }
}
