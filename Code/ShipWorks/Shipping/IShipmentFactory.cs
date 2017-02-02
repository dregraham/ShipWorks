using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Factory for creating shipments
    /// </summary>
    public interface IShipmentFactory
    {
        /// <summary>
        /// Create a shipment for the given order
        /// </summary>
        ShipmentEntity Create(OrderEntity order);

        /// <summary>
        /// Create a shipment for the given OrderID
        /// </summary>
        ShipmentEntity Create(long orderId);

        /// <summary>
        /// Auto create a shipment if necessary
        /// </summary>
        bool AutoCreateIfNecessary(OrderEntity order, bool createIfNoShipments);
    }
}
