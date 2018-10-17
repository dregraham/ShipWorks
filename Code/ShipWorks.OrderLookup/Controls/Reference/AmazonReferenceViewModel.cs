using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IReferenceViewModel), ShipmentTypeCode.Amazon)]
    [WpfView(typeof(AmazonReferenceControl))]
    public class AmazonReferenceViewModel : GenericReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonReferenceViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {
        }
    }
}
