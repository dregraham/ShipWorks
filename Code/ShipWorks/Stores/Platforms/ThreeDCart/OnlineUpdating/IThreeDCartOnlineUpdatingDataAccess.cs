using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading order details
    /// </summary>
    public interface IThreeDCartOnlineUpdatingDataAccess
    {
        /// <summary>
        /// Get order details for uploading
        /// </summary>
        Task<IEnumerable<ThreeDCartOnlineUpdatingOrderDetail>> GetOrderDetails(long orderID);

        /// <summary>
        /// Get ThreeDCartShipmentIDs for uploading
        /// </summary>
        Task<long> GetFirstItemShipmentIDByOriginalOrderID(long originalOrderID);
    }
}