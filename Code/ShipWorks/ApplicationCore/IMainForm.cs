using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Settings;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Interface for finding/using the MainForm
    /// </summary>
    public interface IMainForm : ISynchronizeInvoke, IWin32Window
    {
        /// <summary>
        /// Gets the main forms UIMode
        /// </summary>
        UIMode UIMode { get; }

        /// <summary>
        /// Returns true if any forms, other than the main UI form or floating panels, are open.  False otherwise.
        /// </summary>
        bool AdditionalFormsOpen();

        /// <summary>
        /// Focus the control
        /// </summary>
        bool Focus();

        /// <summary>
        /// True if shippingPanel is Open
        /// </summary>
        bool IsShippingPanelOpen();
    }
}
