using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for working with shipments
    /// </summary>
    public interface IShippingManager
    {
        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        void RefreshShipment(ShipmentEntity shipment);

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        IDictionary<ShipmentEntity, Exception> SaveShipmentToDatabase(ShipmentEntity shipment, bool forceSave);

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, bool forceSave);

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        ShipmentEntity EnsureShipmentLoaded(ShipmentEntity shipment);

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        Task<ShipmentEntity> EnsureShipmentLoadedAsync(ShipmentEntity shipment);

        /// <summary>
        /// Gets the overridden store shipment.
        /// </summary>
        ShipmentEntity GetOverriddenStoreShipment(ShipmentEntity shipment);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        ICarrierShipmentAdapter GetShipment(long shipmentID);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        Task<ICarrierShipmentAdapter> GetShipmentAsync(long shipmentID);

        /// <summary>
        /// Gets the shipment adapter, order will be attached.
        /// </summary>
        ICarrierShipmentAdapter GetShipmentAdapter(ShipmentEntity shipment);

        /// <summary>
        /// Removes the specified shipment from the cache
        /// </summary>
        /// <param name="shipment">Shipment that should be removed from cache</param>
        /// <returns></returns>
        void RemoveShipmentFromRatesCache(ShipmentEntity shipment);

        /// <summary>
        /// Change the shipment type of the provided shipment and return it's shipment adapter
        /// </summary>
        ICarrierShipmentAdapter ChangeShipmentType(ShipmentTypeCode shipmentTypeCode, ShipmentEntity shipment);

        /// <summary>
        /// Indicates if the given shipment type code is enabled for selection in the shipping window
        /// </summary>
        bool IsShipmentTypeEnabled(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        GenericResult<ICarrierShipmentAdapter> VoidShipment(long shipmentID, IShippingErrorManager errorManager);

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Creates a new shipment for the given order ID
        /// </summary>
        ShipmentEntity CreateShipment(long orderID);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment as a return
        /// </summary>
        ShipmentEntity CreateReturnShipment(ShipmentEntity shipment);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment, Action<ShipmentEntity> configure);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        string GetActualServiceUsed(ShipmentEntity shipment);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        string GetOverriddenServiceUsed(ShipmentEntity shipment);

        /// <summary>
        /// Gets the shipment type carrier name.
        /// </summary>
        string GetCarrierName(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Get a description for the 'Other' carrier
        /// </summary>
        CarrierDescription GetOtherCarrierDescription(ShipmentEntity shipment);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        ShipmentEntity EnsureShipmentIsLoadedWithOrder(ShipmentEntity shipment);

        /// <summary>
        /// Validate that the given store is licensed to ship.
        /// </summary>
        Exception ValidateLicense(StoreEntity store, IDictionary<long, Exception> licenseCheckCache);

        /// <summary>
        /// Get the carrier account associated with a shipment. Returns null if the account hasn't been set yet.
        /// </summary>
        ICarrierAccount GetCarrierAccount(ShipmentEntity shipment);

        /// <summary>
        /// Get the carrier account associated with a processed shipment.
        /// </summary>
        ICarrierAccount GetCarrierAccount(ProcessedShipmentEntity processedShipment);
    }
}
