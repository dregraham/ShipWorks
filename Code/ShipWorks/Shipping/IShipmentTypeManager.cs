using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal;

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
        /// Returns a list of Configured ShipmentTypeCodes
        /// </summary>
        IEnumerable<ShipmentTypeCode> ConfiguredShipmentTypeCodes { get; }

        /// <summary>
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        ShipmentType InitialShipmentType(ShipmentEntity shipment);

        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        ShipmentType Get(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        void LoadShipmentData(ShipmentEntity shipmentEntity, bool v);

        /// <summary>
        /// Get the provider for the specified shipment
        /// </summary>
        ShipmentType Get(IShipmentEntity shipment);

        /// <summary>
        /// Indicates if the shipment type is postal
        /// </summary>
        bool IsPostal(ShipmentTypeCode shipmentType);

        /// <summary>
        /// Indicates if the shipment type is DHL
        /// </summary>
        bool IsDhl(PostalServiceType serviceType);
        
        /// <summary>
        /// Indicates if the shipment type is UPS
        /// </summary>
        bool IsUps(ShipmentTypeCode shipmentType);

        /// <summary>
        /// List of shipment types excluded by best rate
        /// </summary>
        List<ShipmentTypeCode> BestRateExcludedShipmentTypes();
    }
}
