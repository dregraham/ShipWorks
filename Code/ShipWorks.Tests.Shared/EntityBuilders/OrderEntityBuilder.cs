using System;
using ShipWorks.Data.Model.EntityClasses;

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
        public OrderEntityBuilder WithShipment(Action<ShipmentEntityBuilder> builderConfiguration)
        {
            ShipmentEntityBuilder builder = new ShipmentEntityBuilder();
            builderConfiguration?.Invoke(builder);

            Set(x => x.Shipments.Add(builder.Build()));

            return this;
        }
    }
}
