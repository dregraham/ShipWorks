using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Reference
{
    /// <summary>
    /// View model for order lookup reference control
    /// </summary>
    [KeyedComponent(typeof(IReferenceViewModel), ShipmentTypeCode.FedEx)]
    [WpfView(typeof(FedExReferenceControl))]
    public class FedExReferenceViewModel : GenericReferenceViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExReferenceViewModel(IOrderLookupShipmentModel shipmentModel) : base(shipmentModel)
        {
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.FedExSignatureAndReference;
    }
}
