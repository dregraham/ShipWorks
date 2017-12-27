using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// User interaction required for splitting an order
    /// </summary>
    public interface IOrderSplitUserInteraction
    {
        /// <summary>
        /// Get details about splitting an order from a user
        /// </summary>
        Task<OrderSplitDefinition> GetSplitDetailsFromUser(OrderEntity order, string newOrderNumber);

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        Task ShowSuccessConfirmation(IEnumerable<string> orderNumbers);
    }
}
