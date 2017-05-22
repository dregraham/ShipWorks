using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    public interface IBigCommerceOnlineUpdater
    {
        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        void UpdateShipmentDetails(OrderEntity order);

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        void UpdateShipmentDetails(long entityID);

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        void UpdateOrderStatus(long orderID, int statusCode);

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        void UpdateOrderStatus(long orderID, int statusCode, UnitOfWork2 commitWork);
    }
}