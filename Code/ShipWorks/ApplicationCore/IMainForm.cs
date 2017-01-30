using System.ComponentModel;
using System.Windows.Forms;

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
    }
}
