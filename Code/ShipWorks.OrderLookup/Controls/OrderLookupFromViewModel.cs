using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping;
using ShipWorks.UI.Controls.AddressControl;
using Autofac.Features.Indexed;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.From)]
    public class OrderLookupFromViewModel : AddressViewModel
    {
        private readonly IOrderLookupMessageBus messageBus;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFromViewModel(IOrderLookupMessageBus messageBus, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            : base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {
                base.Load(messageBus.ShipmentAdapter.Shipment.OriginPerson, messageBus.ShipmentAdapter.Store);
            }
        }
    }
}
