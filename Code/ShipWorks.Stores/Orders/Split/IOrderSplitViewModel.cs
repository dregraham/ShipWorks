using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the split orders dialog
    /// </summary>
    public interface IOrderSplitViewModel
    {
        /// <summary>
        /// Get order split details from user
        /// </summary>
        GenericResult<OrderSplitDefinition> GetSplitDetailsFromUser(OrderEntity order, string suggestedOrderNumber);
    }
}
