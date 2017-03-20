using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    public class UpsPromoFootnoteViewModel : IUpsPromoFootnoteViewModel
    {
        private readonly IMessenger messenger;
        private readonly IWin32Window owner;

        public UpsPromoFootnoteViewModel(IMessenger messenger, IWin32Window owner)
        {
            this.messenger = messenger;
            this.owner = owner;
            ActivatePromo = new RelayCommand(ActivatePromoAction);
        }

        /// <summary>
        /// Command to show the shipping settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ActivatePromo { get; }

        /// <summary>
        /// The UPS promo.
        /// </summary>
        public TelemetricUpsPromo UpsPromo { get; set; }

        /// <summary>
        /// Shipment adapter associated with the current rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Bring up the UpsPromoDlg
        /// </summary>
        private void ActivatePromoAction()
        {
            using (UpsPromoDlg dlg = new UpsPromoDlg(UpsPromo))
            {
                dlg.ShowDialog(owner);
            }

            RateCache.Instance.Clear();

            messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
        }
    }
}