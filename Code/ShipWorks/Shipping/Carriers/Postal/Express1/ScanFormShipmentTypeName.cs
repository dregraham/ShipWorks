using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Gets the dynamic name of the shipment type for ScanForms
    /// </summary>
    public class ScanFormShipmentTypeName : IScanFormShipmentTypeName
    {
        /// <summary>
        /// Gets the name of the shipment type.
        /// </summary>
        public string GetShipmentTypeName(ShipmentTypeCode shipmentTypeCode)
        {
            return EnumHelper.GetDescription(shipmentTypeCode);
        }
    }
}
