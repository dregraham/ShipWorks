using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping;
using ShipWorks.UI.Controls.AddressControl;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// View model for the From address 
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.From)]
    public class OrderLookupFromViewModel : AddressViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromViewModel(IOrderLookupMessageBus messageBus, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            MessageBus = messageBus;
            MessageBus.PropertyChanged += MessageBusPropertyChanged;
        }

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus { get; }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" &&
                MessageBus.Order != null)
            {
                base.Load(MessageBus.ShipmentAdapter.Shipment.OriginPerson, MessageBus.ShipmentAdapter.Store);

                
            }

            if (e.PropertyName == "OriginOriginID")
            {
                long originId = MessageBus.ShipmentAdapter.Shipment.OriginOriginID;
                long orderId = MessageBus.Order.OrderID;
                long accountId = MessageBus.ShipmentAdapter.AccountId.GetValueOrDefault();
                ShipmentTypeCode shipmentTypeCode = MessageBus.ShipmentAdapter.ShipmentTypeCode;
                StoreEntity store = MessageBus.ShipmentAdapter.Store;

                base.SetAddressFromOrigin(originId, orderId, accountId, shipmentTypeCode, store);
            }

            handler.RaisePropertyChanged(nameof(MessageBus));
        }
    }
}
