using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Updates BigCommerce order status/shipments
    /// </summary>
    public interface IOrderStatusUpdater
    {
        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        Task UpdateOrderStatus(BigCommerceStoreEntity store, long orderID, int statusCode);

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        Task UpdateOrderStatus(BigCommerceStoreEntity store, long orderID, int statusCode, IUnitOfWorkCore commitWork);

        /// <summary>
        /// Changes the status of an BigCommerce order to that specified
        /// </summary>
        Task UpdateOrderStatus(BigCommerceStoreEntity store, OnlineOrder orderDetails, int orderStatusCompleted);
    }
}