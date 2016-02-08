using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 rate promotion
    /// </summary>
    public class Express1RatePromotionFootnoteViewModel : IExpress1RatePromotionFootnoteViewModel
    {
        private readonly IMessenger messenger;
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1RatePromotionFootnoteViewModel(IMessenger messenger, IWin32Window owner)
        {
            this.messenger = messenger;
            this.owner = owner;

            ActivateDiscount = new RelayCommand(ActivateDiscountAction);
        }

        /// <summary>
        /// Activate the available discount
        /// </summary>
        public ICommand ActivateDiscount { get; private set; }

        /// <summary>
        /// Settings for the Express 1 dialog
        /// </summary>
        public IExpress1SettingsFacade Settings { get; set; }

        /// <summary>
        /// Shipment associated with the rates
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Open the activate discount dialog
        /// </summary>
        private void ActivateDiscountAction()
        {
            using (Express1ActivateDiscountDlg dlg = new Express1ActivateDiscountDlg(Settings))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
                }
            }
        }
    }
}
