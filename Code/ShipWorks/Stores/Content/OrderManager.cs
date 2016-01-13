using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order management
    /// </summary>
    public class OrderManager : IOrderManager
    {
        /// <summary>
        /// Get a populated order from a given shipment
        /// </summary>
        public void PopulateOrderDetails(ShipmentEntity shipment)
        {
            OrderUtility.PopulateOrderDetails(shipment);
        }

        /// <summary>
        /// Get a populated order from a order ID
        /// </summary>
        public OrderEntity FetchOrder(long orderID)
        {
            return OrderUtility.FetchOrder(orderID);
        }

        /// <summary>
        /// Get an order with the data specified in the prefetch path loaded
        /// </summary>
        public OrderEntity LoadOrder(long orderId, IPrefetchPath2 prefetchPath)
        {
            MethodConditions.EnsureArgumentIsNotNull(prefetchPath, nameof(prefetchPath));

            OrderEntity order = new OrderEntity(orderId);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(true))
            {
                sqlAdapter.FetchEntity(order, prefetchPath);
            }

            return order;
        }
    }
}
