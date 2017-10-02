using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading order details
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Get order details for uploading
        /// </summary>
        Task<IEnumerable<OrderDetail>> GetOrderDetails(long orderID);
    }
}