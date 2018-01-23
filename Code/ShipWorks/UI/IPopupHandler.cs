using System.Windows.Forms;

namespace ShipWorks.UI
{
    /// <summary>
    /// Handle showing the popup
    /// </summary>
    public interface IPopupHandler
    {
        /// <summary>
        /// Show the popup window with the appropriate message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="mainForm"></param>
        void ShowAction(string message, IWin32Window mainForm);
    }
}