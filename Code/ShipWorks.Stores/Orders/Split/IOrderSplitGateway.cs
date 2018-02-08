using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Gateway to the database used when splitting orders
    /// </summary>
    public interface IOrderSplitGateway
    {
        /// <summary>
        /// Load an order that will be split
        /// </summary>
        Task<OrderEntity> LoadOrder(long orderID);

        /// <summary>
        /// Get the next order ID that should be used for a split order
        /// </summary>
        Task<string> GetNextOrderNumber(long orderID, string existingOrderNumber);

        /// <summary>
        /// Is this order allowed to be split
        /// </summary>
        bool CanSplit(long orderID);
    }
}
