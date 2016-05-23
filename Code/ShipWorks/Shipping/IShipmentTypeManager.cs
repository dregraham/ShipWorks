using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Manages all ShipmentTypes available in ShipWorks
    /// </summary>
    public interface IShipmentTypeManager
    {
        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        List<ShipmentType> ShipmentTypes { get; }

        /// <summary>
        /// Returns the ShipmentType for the given type code
        /// </summary>
        ShipmentType GetType(ShipmentTypeCode typeCode);
    }
}
