using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Wraps the ShippingManager
    /// </summary>
    public class ShippingManagerWrapper : IShippingManager
    {
        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        public void RefreshShipment(ShipmentEntity shipment) =>
            ShippingManager.RefreshShipment(shipment);

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeConfigured(shipmentTypeCode);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetServiceUsed(ShipmentEntity shipment) =>
            ShippingManager.GetServiceUsed(shipment);

        /// <summary>
        /// Ensure the carrier-specific data for the shipment exists, like the associated FedEx table row.  Also ensures
        /// that the custom's data is loaded.  Can throw a SqlForeignKeyException  if the shipment's order has been deleted,
        /// or ObjectDeletedException if the shipment itself has been deleted.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void EnsureShipmentLoaded(ShipmentEntity shipment) =>
                    ShippingManager.EnsureShipmentLoaded(shipment);
    }
}