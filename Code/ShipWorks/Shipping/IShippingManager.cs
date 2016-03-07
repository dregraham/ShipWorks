using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
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
        /// Gets the overridden store shipment.
        /// </summary>
        ShipmentEntity GetOverriddenStoreShipment(ShipmentEntity shipment);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        ICarrierShipmentAdapter GetShipment(long shipmentID);

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
        ICarrierShipmentAdapter VoidShipment(long shipmentID);

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        string GetServiceUsed(ShipmentEntity shipment);

        /// <summary>
        /// Gets the shipment type carrier name.
        /// </summary>
        string GetCarrierName(ShipmentTypeCode shipmentTypeCode);
    }
}