using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipWorks.AddressValidation;

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
        /// Get the list of shipments that correspond to the given order key.  If no shipment exists for the order,
        /// one will be created if autoCreate is true.  An OrderEntity will be attached to each shipment.
        /// </summary>
        List<ShipmentEntity> GetShipments(long orderID, bool createIfNone);

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, ValidatedAddressScope validatedAddressScope, bool forceSave);
    }
}