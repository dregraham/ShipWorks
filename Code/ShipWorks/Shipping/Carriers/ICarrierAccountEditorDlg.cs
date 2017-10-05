using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Interface that represents an account editor
    /// </summary>
    public interface ICarrierAccountEditorDlg : IDisposable
    {
        /// <summary>
        /// show the dialog
        /// </summary>
        DialogResult ShowDialog(IWin32Window owner);
    }
}