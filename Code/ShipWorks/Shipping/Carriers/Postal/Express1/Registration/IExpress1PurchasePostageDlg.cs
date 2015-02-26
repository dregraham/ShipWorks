using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// An interface that will show a dialog for purchasing postage from an Express1 partner (e.g. Endicia, USPS). 
    /// This is intended to be used by the generic Express1 setup wizard since it cannot be coupled to a particular
    /// Express1 partner.
    /// </summary>
    public interface IExpress1PurchasePostageDlg
    {
        /// <summary>
        /// This will show the dialog using the information for the given USPS account entity provided.
        /// </summary>
        DialogResult ShowDialog(IWin32Window owner, long accountID);
    }
}
