using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval
{
    /// <summary>
    /// An interface for removing items of a Newegg order.
    /// </summary>
    public interface IRemoveItemRequest
    {
        /// <summary>
        /// Removes the items from the given order.
        /// </summary>
        /// <param name="order">The order items should be removed from.</param>
        /// <param name="items">The items to be removed.</param>
        /// <returns>An ItemRemovalResult object.</returns>
        Task<ItemRemovalResult> RemoveItems(Order order, IEnumerable<Item> items);
    }
}
