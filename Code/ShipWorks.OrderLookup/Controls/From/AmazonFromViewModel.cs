using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.Amazon)]
    [WpfView(typeof(AmazonFromControl))]
    public class AmazonFromViewModel : GenericFromViewModel
    {
        public AmazonFromViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            AddressViewModel addressViewModel,
            ISchedulerProvider schedulerProvider,
            OrderLookupFromFieldLayoutProvider fieldLayoutProvider) 
            base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
        }
    }
}
