using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// Best rate customs view model
    /// </summary>
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.BestRate)]
    [WpfView(typeof(BestRateCustomsControl))]
    public class BestRateCustomsViewModel : GenericCustomsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        {
        }
    }
}
