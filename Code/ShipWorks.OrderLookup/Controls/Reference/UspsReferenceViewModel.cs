using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IReferenceViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(UspsReferenceControl))]
    public class UspsReferenceViewModel : GenericReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsReferenceViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {

        }
    }
}
