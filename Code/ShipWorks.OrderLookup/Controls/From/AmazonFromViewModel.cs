using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// Amazon address view model
    /// </summary>
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.Amazon)]
    [WpfView(typeof(AmazonFromControl))]
    public class AmazonFromViewModel : GenericFromViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonFromViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory,
            AddressViewModel addressViewModel,
            ISchedulerProvider schedulerProvider,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) :
            base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
        }
    }
}
