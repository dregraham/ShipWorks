using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupReferenceViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(OrderLookupUspsReferenceControl))]
    public class OrderLookupUspsReferenceViewModel : OrderLookupPostalReferenceViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupUspsReferenceViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {

        }
    }
}
