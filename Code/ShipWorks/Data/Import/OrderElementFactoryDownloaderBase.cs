﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Data.Import
{
    /// <summary>
    /// A store downloader that implements IOrderElementFactory, to be derived from for convenience of implementation of the interface
    /// </summary>
    public abstract class OrderElementFactoryDownloaderBase : StoreDownloader, IOrderElementFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected OrderElementFactoryDownloaderBase(StoreEntity store, StoreType storeType, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeType, configurationData, sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Create an order with the given identifier
        /// </summary>
        Task<GenericResult<OrderEntity>> IOrderElementFactory.CreateOrder(OrderIdentifier orderIdentifier)
        {
            return InstantiateOrder(orderIdentifier);
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
        /// Creates and populates a new OrderItemAttribute based on the given OrderItemEntity, name, description, unitPrice, and isManual flag
        /// </summary>
        OrderItemAttributeEntity IOrderElementFactory.CreateItemAttribute(OrderItemEntity item, string name, string description,
            decimal unitPrice, bool isManual)
        {
            return InstantiateOrderItemAttribute(item, name, description, unitPrice, isManual);
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
        OrderChargeEntity IOrderElementFactory.CreateCharge(OrderEntity order, string type, string description, decimal amount)
        {
            return InstantiateOrderCharge(order, type, description, amount);
        }

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        Task<NoteEntity> IOrderElementFactory.CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility) =>
            InstantiateNote(order, noteText, noteDate, noteVisibility, true);

        /// <summary>
        /// Create a new note and attach it to the order.
        /// </summary>
        Task<NoteEntity> IOrderElementFactory.CreateNote(OrderEntity order, string noteText, DateTime noteDate, NoteVisibility noteVisibility, bool ignoreDuplicateText) =>
            InstantiateNote(order, noteText, noteDate, noteVisibility, ignoreDuplicateText);

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

        /// <summary>
        /// Get the next order number to use
        /// </summary>
        async Task<long> IOrderElementFactory.GetNextOrderNumberAsync()
        {
            return await GetNextOrderNumberAsync().ConfigureAwait(false);
        }
    }
}
