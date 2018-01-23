using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Messaging.Messages.Dialogs;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Interface for finding/using the MainForm
    /// </summary>
    public interface IMainForm : ISynchronizeInvoke, IWin32Window
    {
        /// <summary>
        /// Returns true if any forms, other than the main UI form or floating panels, are open.  False otherwise.
        /// </summary>
        bool AdditionalFormsOpen();

        /// <summary>
        /// Focus the control
        /// </summary>
        bool Focus();

        /// <summary>
        /// Briefly show a popup message
        /// </summary>
        void ShowPopup(string message);
    }
}
