using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Repository for saving order related content
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        bool ContainsNote(OrderEntity order, string noteText, NoteSource source);

        /// <summary>
        /// Populates the order, order items, order charges, and order item attribute for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        void PopulateOrderDetails(OrderEntity order);
    }
}