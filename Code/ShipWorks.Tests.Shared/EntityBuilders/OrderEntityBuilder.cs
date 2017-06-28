using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build an order entity
    /// </summary>
    public class OrderEntityBuilder<TOrder> : EntityBuilder<TOrder> where TOrder : OrderEntity, new()
    {
        /// <summary>
        /// Modify an existing order
        /// </summary>
        public OrderEntityBuilder(TOrder order) : base(order)
        {

        }

        /// <summary>
        /// Create a new order entity builder
        /// </summary>
        public OrderEntityBuilder(StoreEntity store, CustomerEntity customer)
        {
            Set(x => x.Store, store);
            Set(x => x.StoreID, store.StoreID);
            Set(x => x.Customer, customer);
            Set(x => x.CustomerID, customer.CustomerID);
            Set(x => x.CombineSplitStatus, CombineSplitStatusType.None);
        }

        /// <summary>
        /// Set the order number
        /// </summary>
        public OrderEntityBuilder<TOrder> WithOrderNumber(long orderNumber)
        {
            Set(x => x.OrderNumber, orderNumber);

            return this;
        }

        /// <summary>
        /// Set the order number
        /// </summary>
        public OrderEntityBuilder<TOrder> WithOrderNumber(long orderNumber, string prefix, string postfix)
        {
            Set(x => x.OrderNumber, orderNumber);
            Set(x => x.ApplyOrderNumberPrefix(prefix));
            Set(x => x.ApplyOrderNumberPostfix(postfix));

            return this;
        }

        /// <summary>
        /// Add a charge to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithCharge(Action<EntityBuilder<OrderChargeEntity>> builderConfiguration) =>
            CreateCollectionEntity(builderConfiguration, x => x.OrderCharges);

        /// <summary>
        /// Add a shipment to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithShipment() => WithShipment(null);

        /// <summary>
        /// Add a shipment to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithShipment(Action<ShipmentEntityBuilder> builderConfiguration) =>
            CreateCollectionEntity(builderConfiguration, x => x.Shipments);

        /// <summary>
        /// Set the shipping address on the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithShipAddress(string address1, string address2, string city, string state, string postalCode, string country)
        {
            Set(x => x.ShipPerson = new PersonAdapter
            {
                Street1 = address1,
                Street2 = address2,
                City = city,
                StateProvCode = state,
                PostalCode = postalCode,
                CountryCode = country
            });

            return this;
        }

        /// <summary>
        /// Add an item to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithItem() => WithItem(null);

        /// <summary>
        /// Add an item to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithItem(Action<OrderItemEntityBuilder<OrderItemEntity>> builderConfiguration) =>
            CreateCollectionEntity(builderConfiguration, x => x.OrderItems);

        /// <summary>
        /// Add an item to the order
        /// </summary>
        public OrderEntityBuilder<TOrder> WithItem<TOrderItem>(Action<OrderItemEntityBuilder<TOrderItem>> builderConfiguration)
            where TOrderItem : OrderItemEntity, new() =>
            CreateCollectionEntity<OrderItemEntity, TOrderItem, OrderItemEntityBuilder<TOrderItem>>(builderConfiguration, x => x.OrderItems);

        /// <summary>
        /// Create an entity and add it to a collection
        /// </summary>
        protected OrderEntityBuilder<TOrder> CreateCollectionEntity<T, TBuilder>(Action<TBuilder> builderConfiguration,
            Func<OrderEntity, EntityCollection<T>> addAction)
            where T : EntityBase2, new()
            where TBuilder : EntityBuilder<T>, new()
        {
            return CreateCollectionEntity<T, T, TBuilder>(builderConfiguration, addAction);
        }

        /// <summary>
        /// Create an entity and add it to a collection
        /// </summary>
        protected OrderEntityBuilder<TOrder> CreateCollectionEntity<TBase, TSpecific, TBuilder>(Action<TBuilder> builderConfiguration,
            Func<OrderEntity, EntityCollection<TBase>> addAction)
            where TBase : EntityBase2, new()
            where TSpecific : TBase, new()
            where TBuilder : EntityBuilder<TSpecific>, new()
        {
            TBuilder builder = new TBuilder();
            builderConfiguration?.Invoke(builder);

            Set(x => addAction(x).Add(builder.Build()));

            return this;
        }
    }
}
