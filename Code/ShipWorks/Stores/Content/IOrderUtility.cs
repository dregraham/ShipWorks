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
        /// Get a populated order from a order ID
        /// </summary>
        OrderEntity FetchOrder(long orderID);

        /// <summary>
        /// Load the specified order using the given prefetch path
        /// </summary>
        OrderEntity LoadOrder(long orderID, IPrefetchPath2 prefetchPath);
    }
}
