using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wrapper for the static shipment type manager
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ShipmentTypeCodes => ShipmentTypeManager.ShipmentTypeCodes;

        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        public IEnumerable<ShipmentTypeCode> EnabledShipmentTypeCodes => ShipmentTypeManager.EnabledShipmentTypeCodes;

        /// <summary>
        /// Get the sort value for a given shipment type code
        /// </summary>
        public int GetSortValue(ShipmentTypeCode shipmentTypeCode) => ShipmentTypeManager.GetSortValue(shipmentTypeCode);
    }
}
