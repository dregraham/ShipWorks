using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers
{
    public interface ICarrierAccountEditorDlg : IDisposable
    {
        DialogResult ShowDialog(IWin32Window owner);
    }
}