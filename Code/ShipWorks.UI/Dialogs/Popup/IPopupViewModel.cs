using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.UI.Dialogs.Popup
{
    /// <summary>
    /// Interface for the PopupViewModel
    /// </summary>
    public interface IPopupViewModel
    {
        /// <summary>
        /// Shows the popup
        /// </summary>
        void Show(string message, IWin32Window owner);

        /// <summary>
        /// Show the popup with a keyboard icon
        /// </summary>
        void ShowWithKeyboard(string message, Control owner);
        
        /// <summary>
        /// Show the popup with a barcode icon
        /// </summary>
        void ShowWithBarcode(string message, Control owner);
    }
}