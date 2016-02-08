using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    /// <summary>
    /// USPS promotion footnote
    /// </summary>
    public class UspsRatePromotionFootnoteViewModel : IUspsRatePromotionFootnoteViewModel
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsRatePromotionFootnoteViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            ActivateDiscount = new RelayCommand(ActivateDiscountAction);
        }

        /// <summary>
        /// Activate the discount
        /// </summary>
        public ICommand ActivateDiscount { get; }

        /// <summary>
        /// Shipment associated with the promotion
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Should the single account dialog be displayed
        /// </summary>
        public bool ShowSingleAccountDialog { get; set; }

        /// <summary>
        /// Link to activate the USPS discount
        /// </summary>
        private void ActivateDiscountAction()
        {
            ShipmentTypeCode originalShipmentTypeCode = ShipmentAdapter.ShipmentTypeCode;

            using (UspsActivateDiscountDlg dlg = GetDiscountDialog())
            {
                dlg.Initialize(ShipmentAdapter.Shipment);

                // If they clicked ok and it didn't cause the shipment type to change, refresh rates.
                if (dlg.ShowDialog() == DialogResult.OK && originalShipmentTypeCode == ShipmentAdapter.ShipmentTypeCode)
                {
                    messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
                }
            }
        }

        /// <summary>
        /// Gets the appropriate discount dialog based on ShowSingleAccountDialog.
        /// </summary>
        private UspsActivateDiscountDlg GetDiscountDialog()
        {
            return ShowSingleAccountDialog ? new SingleAccountMarketingDlg() : new UspsActivateDiscountDlg();
        }
    }
}
