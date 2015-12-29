using System;
using Interapptive.Shared.Business;
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
            CreateCollectionEntity(builderConfiguration, x => x.Shipments);

        /// <summary>
        /// Set the shipping address on the order
        /// </summary>
        public OrderEntityBuilder WithShipAddress(string address1, string address2, string city, string state, string postalCode, string country)
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
        public OrderEntityBuilder WithItem() => WithItem(null);

        /// <summary>
        /// Add an item to the order
        /// </summary>
        public OrderEntityBuilder WithItem(Action<EntityBuilder<OrderItemEntity>> builderConfiguration) =>
            CreateCollectionEntity(builderConfiguration, x => x.OrderItems);

        /// <summary>
        /// Create an entity and add it to a collection
        /// </summary>
        protected OrderEntityBuilder CreateCollectionEntity<T, TBuilder>(Action<TBuilder> builderConfiguration,
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
