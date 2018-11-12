using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.UI;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.OrderLookup.Controls.From
{
    /// <summary>
    /// Viewmodel for the EndiciaFromControl
    /// </summary>
    [KeyedComponent(typeof(IFromViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(EndiciaFromControl))]
    public class EndiciaFromViewModel : GenericFromViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaFromViewModel(
            IOrderLookupShipmentModel shipmentModel, 
            IShipmentTypeManager shipmentTypeManager, 
            ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory, 
            AddressViewModel addressViewModel, 
            ISchedulerProvider schedulerProvider, 
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, carrierAccountRetrieverFactory, addressViewModel, schedulerProvider, fieldLayoutProvider)
        {
        }
    }
}
