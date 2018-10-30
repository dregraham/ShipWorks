using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Shipping;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.To
{
    /// <summary>
    /// ViewModel for To panel in the OrderLookup view
    /// </summary>
    [KeyedComponent(typeof(IToViewModel), ShipmentTypeCode.Usps)]
    [WpfView(typeof(UspsToControl))]
    public class UspsToViewModel : GenericToViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsToViewModel(
            IOrderLookupShipmentModel shipmentModel,
            AddressViewModel addressViewModel,
            ISchedulerProvider schedulerProvider)
            : base(shipmentModel, addressViewModel, schedulerProvider)
        {
        }
    }
}
