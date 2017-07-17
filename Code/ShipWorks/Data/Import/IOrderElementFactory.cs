using System;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Data.Import
{
    /// <summary>
    /// Factory that the generic data importers uses to instantiate order objects
    /// </summary>
    public interface IOrderElementFactory
    {
        /// <summary>
        /// Create a new item attached to the order
        /// </summary>
        OrderItemEntity CreateItem(OrderEntity order);

        /// <summary>
        /// Create a new item attribute attached to the order
        /// </summary>
        OrderItemAttributeEntity CreateItemAttribute(OrderItemEntity item);

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        OrderChargeEntity CreateCharge(OrderEntity order);

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        Task<NoteEntity> CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility);

        /// <summary>
        /// Create a new payment detail attached to the order
        /// </summary>
        OrderPaymentDetailEntity CreatePaymentDetail(OrderEntity order);
    }
}
