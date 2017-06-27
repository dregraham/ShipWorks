using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.UI
{
    /// <summary>
    /// Notification dialog
    /// </summary>
    public interface IUserConditionalNotification : IDialog
    {
        /// <summary>
        /// Show the information message, if necessary
        /// </summary>
        void ShowInformation(IWin32Window owner, string message, UserConditionalNotificationType notificationType);
    }
}
