using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Determines whether the specified order has matching note in database.
        /// </summary>
        bool ContainsNote(OrderEntity order, string noteText, NoteSource source);
    }
}