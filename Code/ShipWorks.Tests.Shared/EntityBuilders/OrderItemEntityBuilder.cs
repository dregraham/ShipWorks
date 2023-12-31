﻿using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build an order entity
    /// </summary>
    public class OrderItemEntityBuilder<TOrderItem> : EntityBuilder<TOrderItem>
        where TOrderItem : OrderItemEntity, new()
    {
        /// <summary>
        /// Modify an existing order item
        /// </summary>
        public OrderItemEntityBuilder()
        {
        }

        /// <summary>
        /// Modify an existing order item
        /// </summary>
        public OrderItemEntityBuilder(TOrderItem orderItem) : base(orderItem)
        {
        }

        /// <summary>
        /// Create a new order item entity builder
        /// </summary>
        public OrderItemEntityBuilder(OrderEntity order)
        {
            Set(x => x.Order, order);
        }

        /// <summary>
        /// Add an item attribute to the order item
        /// </summary>
        public OrderItemEntityBuilder<TOrderItem> WithItemAttribute() => WithItemAttribute(null);

        /// <summary>
        /// Add an item attribute to the order item
        /// </summary>
        public OrderItemEntityBuilder<TOrderItem> WithItemAttribute(Action<EntityBuilder<OrderItemAttributeEntity>> builderConfiguration) =>
            CreateCollectionEntity(builderConfiguration, x => x.OrderItemAttributes);

        /// <summary>
        /// Create an entity and add it to a collection
        /// </summary>
        protected OrderItemEntityBuilder<TOrderItem> CreateCollectionEntity<T, TBuilder>(Action<TBuilder> builderConfiguration,
            Func<OrderItemEntity, EntityCollection<T>> addAction)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            Set(x => addAction(x).Add(builder.Build()));

            return this;
        }
    }
}
