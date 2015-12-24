using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build an order entity
    /// </summary>
    public class OrderEntityBuilder : EntityBuilder<OrderEntity>
    {
        /// <summary>
        /// Create a new order entity builder
        /// </summary>
        public OrderEntityBuilder(StoreEntity store, CustomerEntity customer)
        {
            SetField(x => x.Store, store);
            SetField(x => x.Customer, customer);
        }

        /// <summary>
        /// Set the order number
        /// </summary>
        public OrderEntityBuilder WithOrderNumber(long orderNumber)
        {
            SetField(x => x.OrderNumber, orderNumber);

            return this;
        }

        /// <summary>
        /// Add a shipment to the order
        /// </summary>
        public OrderEntityBuilder WithShipment() => WithShipment(null);

        /// <summary>
        /// Add a shipment to the order
        /// </summary>
        public OrderEntityBuilder WithShipment(Action<ShipmentEntityBuilder> builderConfiguration) =>
            AddItemToCollection(builderConfiguration, x => x.Shipments);

        public OrderEntityBuilder WithItem() => WithItem(null);

        public OrderEntityBuilder WithItem(Action<EntityBuilder<OrderItemEntity>> builderConfiguration) =>
            AddItemToCollection(builderConfiguration, x => x.OrderItems);

        /// <summary>
        /// Set the shipment type
        /// </summary>
        protected OrderEntityBuilder AddItemToCollection<T, TBuilder>(Action<TBuilder> builderConfiguration,
            Func<OrderEntity, EntityCollection<T>> addAction)
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
