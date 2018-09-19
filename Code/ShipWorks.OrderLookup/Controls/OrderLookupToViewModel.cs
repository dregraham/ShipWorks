using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls
{
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.To)]
    public class OrderLookupToViewModel : AddressViewModel
    {
        private readonly IOrderLookupMessageBus messageBus;
        

        public OrderLookupToViewModel(IOrderLookupMessageBus messageBus, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            :base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBus_PropertyChanged;
        }

        private void MessageBus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {
                base.Load(messageBus.ShipmentAdapter.Shipment.ShipPerson, messageBus.ShipmentAdapter.Store);
            }
        }
    }
}
