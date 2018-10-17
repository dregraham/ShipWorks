using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsCustomsControl))]
    public class UpsCustomsViewModel : GenericCustomsViewModel
    {
        public UpsCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager) : base(shipmentModel, shipmentTypeManager)
        {
        }
    }
}
