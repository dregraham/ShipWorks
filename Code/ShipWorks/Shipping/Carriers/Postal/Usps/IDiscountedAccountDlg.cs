using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Define the similarities between the create and convert expedited account dialogs
    /// </summary>
    public interface IDiscountedAccountDlg : IDisposable
    {
        /// <summary>
        /// Initialize the dialog
        /// </summary>
        /// <param name="shipmentEntity">Shipment that should be used for initialization</param>
        /// <param name="showSingleAccountMarketing">Should the description text use single account marketing text</param>
        void Initialize(ShipmentEntity shipmentEntity, bool showSingleAccountMarketing);

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="owner">Owner of the dialog</param>
        DialogResult ShowDialog(IWin32Window owner);
    }
}