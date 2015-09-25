using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for the shipment type manager
    /// </summary>
    public interface IShipmentTypeManager
    {
        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        List<ShipmentType> EnabledShipmentTypes { get; }

        /// <summary>
        /// Get the sort value for a given shipment type code
        /// </summary>
        int GetSortValue(ShipmentTypeCode shipmentTypeCode);
    }
}
