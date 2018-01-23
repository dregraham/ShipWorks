using System.Windows.Forms;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// Interface for a popup message view
    /// </summary>
    public interface IPopup
    {
        /// <summary>
        /// Show the message
        /// </summary>
        void ShowAction();

        /// <summary>
        /// Load owner in order to get the window in the correct position
        /// </summary>
        void LoadOwner(IWin32Window owner);

        /// <summary>
        /// Saves the view model to the data context
        /// </summary>
        IPopupViewModel ViewModel { set; }
    }
}