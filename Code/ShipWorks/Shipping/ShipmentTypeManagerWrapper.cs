using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public List<ShipmentType> ShipmentTypes => ShipmentTypeManager.ShipmentTypes;
    }
}