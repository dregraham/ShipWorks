using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order management
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Get a populated order from a given shipment
        /// </summary>
        void PopulateOrderDetails(ShipmentEntity shipment);

        /// <summary>
        /// Calculates the order total.
        /// </summary>
        decimal CalculateOrderTotal(OrderEntity order);

        /// <summary>
        /// Get a populated order from a order ID
        /// </summary>
        OrderEntity FetchOrder(long orderID);

        /// <summary>
        /// Load the specified order using the given prefetch path
        /// </summary>
        OrderEntity LoadOrder(long orderID, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        ShipmentEntity GetLatestActiveShipment(long orderID);

        /// <summary>
        /// Load the specified orders using the given prefetch path
        /// </summary>
        IEnumerable<OrderEntity> LoadOrders(IEnumerable<long> orderIdList, IPrefetchPath2 prefetchPath);
    }
}
