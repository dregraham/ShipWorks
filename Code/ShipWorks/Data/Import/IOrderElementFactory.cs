using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Creates and populates a new OrderItemAttribute based on the given OrderItemEntity, name, description, unitPrice, and isManual flag
        /// </summary>
        OrderItemAttributeEntity CreateItemAttribute(OrderItemEntity item, string name, string description,
            decimal unitPrice, bool isManual);

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        OrderChargeEntity CreateCharge(OrderEntity order);

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        OrderChargeEntity CreateCharge(OrderEntity order, string type, string description, decimal amount);

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        NoteEntity CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility);

        /// <summary>
        /// Create a new payment detail attached to the order
        /// </summary>
        OrderPaymentDetailEntity CreatePaymentDetail(OrderEntity order);

        /// <summary>
        /// Creates the payment detail.
        /// </summary>
        OrderPaymentDetailEntity CreatePaymentDetail(OrderEntity order, string label, string value);
    }
}
