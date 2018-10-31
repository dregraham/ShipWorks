using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// View model for the UpsCustomsControl
    /// </summary>
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsCustomsControl))]
    public class UpsCustomsViewModel : GenericCustomsViewModel
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public UpsCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        {
        }
    }
}
