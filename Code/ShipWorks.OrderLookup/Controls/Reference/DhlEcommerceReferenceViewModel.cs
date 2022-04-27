using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IReferenceViewModel), ShipmentTypeCode.DhlEcommerce)]
    [WpfView(typeof(DhlEcommerceReferenceControl))]
    public class DhlEcommerceReferenceViewModel : GenericReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceReferenceViewModel(IOrderLookupShipmentModel shipmentModel,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.DhlEcommerceReference;
    }
}
