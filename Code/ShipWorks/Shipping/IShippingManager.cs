using ShipWorks.Data.Model.EntityClasses;

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
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        string GetServiceUsed(ShipmentEntity shipment);

        /// <summary>
        /// Ensure the carrier-specific data for the shipment exists, like the associated FedEx table row.  Also ensures
        /// that the custom's data is loaded.  Can throw a SqlForeignKeyException  if the shipment's order has been deleted,
        /// or ObjectDeletedException if the shipment itself has been deleted.
        /// </summary>
        void EnsureShipmentLoaded(ShipmentEntity shipment);
    }
}