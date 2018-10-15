using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// ViewModel for To panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(IOrderLookupToViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(OrderLookupUspsToControl))]
    public class OrderLookupUspsToViewModel : OrderLookupGenericToViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupUspsToViewModel(IOrderLookupShipmentModel shipmentModel, AddressViewModel addressViewModel)
            : base(shipmentModel, addressViewModel)
        {

        }
    }
}
