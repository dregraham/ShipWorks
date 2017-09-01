using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
    /// <summary>
    /// Updates Shopify order status/shipments
    /// </summary>
    public interface IShopifyOnlineUpdater
    {
        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        Task UpdateOnlineStatus(ShopifyStoreEntity store, ShopifyOrderEntity order);

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        Task UpdateOnlineStatus(ShopifyStoreEntity store, long shipmentID, UnitOfWork2 unitOfWork);
    }
}