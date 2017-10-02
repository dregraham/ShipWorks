using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Web client for updating shipment details online
    /// </summary>
    public interface IBigCommerceShipmentDetailsUpdaterClient
    {
        /// <summary>
        /// Update shipment details for a single order
        /// </summary>
        Task UpdateOnline(IBigCommerceStoreEntity store,
            BigCommerceOnlineOrderDetails orderDetail,
            string orderNumberComplete,
            ShipmentEntity shipment,
            IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems);

        /// <summary>
        /// Determines if an orders' items are all digital
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <returns>True if all items are digital, otherwise false</returns>
        bool IsOrderAllDigital(IEnumerable<IOrderItemEntity> orderItems);
    }
}