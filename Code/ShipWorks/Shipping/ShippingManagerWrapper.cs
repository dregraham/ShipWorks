using System.Collections.Generic;
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
        public void RefreshShipment(ShipmentEntity shipment)
        {
            ShippingManager.RefreshShipment(shipment);
        }

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingManager.UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);
        }

        /// <summary>
        /// Get the list of shipments that correspond to the given order key.  If no shipment exists for the order,
        /// one will be created if autoCreate is true.  An OrderEntity will be attached to each shipment.
        /// </summary>
        public List<ShipmentEntity> GetShipments(long orderID, bool createIfNone)
        {
            return ShippingManager.GetShipments(orderID, createIfNone);
        }
    }
}