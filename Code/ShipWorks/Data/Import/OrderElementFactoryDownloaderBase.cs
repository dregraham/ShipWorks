using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Data.Import
{
    /// <summary>
    /// A store downloader that implements IOrderElementFactory, to be derived from for convenience of implmentation of the interface
    /// </summary>
    public abstract class OrderElementFactoryDownloaderBase : StoreDownloader, IOrderElementFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected OrderElementFactoryDownloaderBase(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Create a new item attached to the order
        /// </summary>
        OrderItemEntity IOrderElementFactory.CreateItem(OrderEntity order)
        {
            return InstantiateOrderItem(order);
        }

        /// <summary>
        /// Create a new item attribute attached to the order
        /// </summary>
        OrderItemAttributeEntity IOrderElementFactory.CreateItemAttribute(OrderItemEntity item)
        {
            return InstantiateOrderItemAttribute(item);
        }

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order)
        {
            return InstantiateOrderCharge(order);
        }

        /// <summary>
        /// Create a new charge attached to the order
        /// </summary>
        public OrderChargeEntity CreateCharge(OrderEntity order, string type, string description, decimal amount)
        {
            return InstantiateOrderCharge(order, type, description, amount);
        }

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        NoteEntity IOrderElementFactory.CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility)
        {
            return InstantiateNote(order, noteText, noteDate, noteVisibility, true);
        }

        /// <summary>
        /// Create a new payment detail attached to the order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order)
        {
            return InstantiateOrderPaymentDetail(order);
        }

        /// <summary>
        /// Create a new payment detail attached to the order
        /// </summary>
        OrderPaymentDetailEntity IOrderElementFactory.CreatePaymentDetail(OrderEntity order, string label, string value)
        {
            return InstantiateOrderPaymentDetail(order, label, value);
        }
    }
}
