using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.LabelOptions
{
    /// <summary>
    /// View model for the OrderLookupLabelOptionsViewModel
    /// </summary>
    [KeyedComponent(typeof(ILabelOptionsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsLabelOptionsControl))]
    public class UpsLabelOptionsViewModel : GenericLabelOptionsViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLabelOptionsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager) : base(shipmentModel, shipmentTypeManager)
        {
        }
    }
}
