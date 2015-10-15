using Interapptive.Shared.UI;
using ShipWorks.Shipping.UI.ShippingPanel;
using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.UI.Services
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
    }
}
