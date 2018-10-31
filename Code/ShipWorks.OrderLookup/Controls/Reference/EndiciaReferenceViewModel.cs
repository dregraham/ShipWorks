using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IReferenceViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(EndiciaReferenceControl))]
    public class EndiciaReferenceViewModel : GenericReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaReferenceViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
        }
    }
}
