using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

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
        GenericResult<OrderSplitDefinition> GetSplitDetailsFromUser(IOrderEntity order, string newOrderNumber);

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        void ShowSuccessConfirmation(IEnumerable<string> orderNumbers);
    }
}
