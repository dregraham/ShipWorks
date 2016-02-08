using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// View model for prompting for the store address for counter rates
    /// </summary>
    public class CounterRatesInvalidStoreAddressFootnoteViewModel : ICounterRatesInvalidStoreAddressFootnoteViewModel
    {
        private readonly IMessenger messenger;
        private readonly IStoreManager storeManager;

        public CounterRatesInvalidStoreAddressFootnoteViewModel(IMessenger messenger, IStoreManager storeManager,
            ISecurityContext securityContext)
        {
            this.messenger = messenger;
            this.storeManager = storeManager;

            CanEditStoreAddress = securityContext.HasPermission(PermissionType.ManageStores);
            EditStoreAddress = new RelayCommand(EditStoreAddressAction);
        }

        /// <summary>
        /// Can the store address be edited
        /// </summary>
        public bool CanEditStoreAddress { get; }

        /// <summary>
        /// Edit the store address
        /// </summary>
        public ICommand EditStoreAddress { get; }

        /// <summary>
        /// Shipment adapter associated with the current rates
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// The user has clicked the "Enter store address" link
        /// </summary>
        private void EditStoreAddressAction()
        {
            StoreEntity store = storeManager.GetRelatedStore(ShipmentAdapter.Shipment);

            using (StoreSettingsDlg dialog = new StoreSettingsDlg(store))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    storeManager.CheckForChanges();

                    messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
                }
            }
        }
    }
}
