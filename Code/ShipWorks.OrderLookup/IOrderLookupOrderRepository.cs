using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents a Order Repository for Order Lookup
    /// </summary>
    public interface IOrderLookupOrderRepository
    {
        /// <summary>
        /// Get the order
        /// </summary>
        Task<OrderEntity> GetOrder(long orderID);

        /// <summary>
        /// Get the orders order id matching the search text
        /// </summary>
        long GetOrderID(string searchText);
    }
}