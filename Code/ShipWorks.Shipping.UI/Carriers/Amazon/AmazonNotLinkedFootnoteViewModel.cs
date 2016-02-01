using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Shipping.Carriers.Amazon;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonNotLinked footnote
    /// </summary>
    public class AmazonNotLinkedFootnoteViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonNotLinkedFootnoteViewModel(ShipmentTypeCode shipmentTypeCode)
        {
            Debug.Assert(shipmentTypeCode == ShipmentTypeCode.Usps || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools);

            ShipmentTypeCode = shipmentTypeCode;
            ShowDialog = new RelayCommand(ShowDialogAction);
        }

        /// <summary>
        /// Shipment type
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Command to show the dialog
        /// </summary>
        public ICommand ShowDialog { get; }

        /// <summary>
        /// Show the given dialog
        /// </summary>
        private void ShowDialogAction()
        {
            using (AmazonNotLinkedDlg dlg = new AmazonNotLinkedDlg(ShipmentTypeCode))
            {
                dlg.ShowDialog();
            }
        }
    }
}