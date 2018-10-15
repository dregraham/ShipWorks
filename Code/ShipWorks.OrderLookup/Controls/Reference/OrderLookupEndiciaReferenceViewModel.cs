using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupReferenceViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(OrderLookupEndiciaReferenceControl))]
    public class OrderLookupEndiciaReferenceViewModel : OrderLookupPostalReferenceViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupEndiciaReferenceViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {

        }
    }
}
