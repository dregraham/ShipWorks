using System.ComponentModel;
using System.Reflection;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupToViewModel(IOrderLookupMessageBus messageBus, IShippingOriginManager shippingOriginManager, IMessageHelper messageHelper,
            IValidatedAddressScope validatedAddressScope, IAddressValidator validator, IAddressSelector addressSelector)
            :base(shippingOriginManager, messageHelper, validatedAddressScope, validator, addressSelector)
        {
            this.messageBus = messageBus;
            this.messageBus.PropertyChanged += MessageBusPropertyChanged;

            IsAddressValidationEnabled = true;
        }

        /// <summary>
        /// Is address validation enabled or not
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupMessageBus MessageBus
        {
            get { return messageBus; }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Order" && messageBus.Order != null)
            {
                base.Load(messageBus.ShipmentAdapter.Shipment.ShipPerson, messageBus.ShipmentAdapter.Store);
                handler.RaisePropertyChanged(nameof(MessageBus));
            }
        }
    }
}
