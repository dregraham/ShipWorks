using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.Settings;
using ShipWorks.Templates.Printing;

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

        /// <summary>
        /// True if shipping history control is active
        /// </summary>
        bool IsShipmentHistoryActive();

        /// <summary>
        /// Update the contents of the status bar
        /// </summary>
        void UpdateStatusBar();

        /// <summary>
        /// Select the order lookup tab
        /// </summary>
        void SelectOrderLookupTab();

        /// <summary>
        /// Start the printing or previewing of the given print job
        /// </summary>
        void StartPrintJob(IPrintJob job, PrintAction action);
    }
}
