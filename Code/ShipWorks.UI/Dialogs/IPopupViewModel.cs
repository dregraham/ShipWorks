using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.UI.Dialogs
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
        /// Shows the popup
        /// </summary>
        void Show(string message, IWin32Window owner, IconType icon, TimeSpan fadeTime);
    }
}