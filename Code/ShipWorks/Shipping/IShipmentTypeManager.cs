using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for the shipment type manager
    /// </summary>
    public interface IShipmentTypeManager
    {
        /// <summary>
        /// Get a list of shipment types
        /// </summary>
        IEnumerable<ShipmentTypeCode> ShipmentTypeCodes { get; }

        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        List<ShipmentType> ShipmentTypes { get; }

        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        IEnumerable<ShipmentTypeCode> EnabledShipmentTypeCodes { get; }

        /// <summary>
        /// Get the sort value for a given shipment type code
        /// </summary>
        int GetSortValue(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Returns a list of ShipmentTypeCodes that support accounts
        /// </summary>
        IEnumerable<ShipmentTypeCode> ShipmentTypesSupportingAccounts { get; }

        /// <summary>
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        ShipmentType InitialShipmentType(ShipmentEntity shipment);

        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        ShipmentType Get(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Get the provider for the specified shipment
        /// </summary>
        ShipmentType Get(ShipmentEntity shipment);
    }
}
