using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Wraps PostalUtility
    /// </summary>
    public class PostalUtilityWrapper : IPostalUtility
    {
        /// <summary>
        /// Indicates if the shipment type is a postal shipment
        /// </summary>
        public bool IsPostalShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            return PostalUtility.IsPostalShipmentType(shipmentTypeCode);
        }
    }
}
