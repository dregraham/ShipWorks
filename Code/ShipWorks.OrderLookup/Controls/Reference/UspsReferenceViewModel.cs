using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
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

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.USPSReference;
    }
}
