using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.UI.Services
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
    }
}
