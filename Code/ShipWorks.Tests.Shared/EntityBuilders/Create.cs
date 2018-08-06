using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build entities for testing
    /// </summary>
    public static class Create
    {
        /// <summary>
        /// Create an audit entity
        /// </summary>
        public static AuditEntityBuilder Audit(IUserEntity user, IComputerEntity computer) =>
            new AuditEntityBuilder(user, computer);

        /// <summary>
        /// Create a generic entity
        /// </summary>
        public static EntityBuilder<T> Entity<T>() where T : EntityBase2, new() => new EntityBuilder<T>();

        /// <summary>
        /// Create an order
        /// </summary>
        public static OrderEntityBuilder<OrderEntity> Order(StoreEntity store, CustomerEntity customer) =>
            new OrderEntityBuilder<OrderEntity>(store, customer);

        /// <summary>
        /// Create an order
        /// </summary>
        public static OrderEntityBuilder<TOrder> Order<TOrder>(StoreEntity store, CustomerEntity customer)
            where TOrder : OrderEntity, new() =>
            new OrderEntityBuilder<TOrder>(store, customer);

        /// <summary>
        /// Create a profile
        /// </summary>
        public static ProfileEntityBuilder Profile() =>
            new ProfileEntityBuilder();

        /// <summary>
        /// Create a shipment
        /// </summary>
        public static ShipmentEntityBuilder Shipment(OrderEntity order) =>
            new ShipmentEntityBuilder(order);

        /// <summary>
        /// Create a shipment
        /// </summary>
        public static ShipmentEntityBuilder Shipment() =>
            new ShipmentEntityBuilder(new OrderEntity());

        /// <summary>
        /// Create a store entity
        /// </summary>
        public static StoreEntityBuilder<T> Store<T>() where T : StoreEntity, new() =>
            new StoreEntityBuilder<T>();

        /// <summary>
        /// Create a store entity
        /// </summary>
        public static StoreEntityBuilder<T> Store<T>(StoreTypeCode type) where T : StoreEntity, new() =>
            (StoreEntityBuilder<T>) new StoreEntityBuilder<T>().Set(x => x.StoreTypeCode, type);

        /// <summary>
        /// Create a carrier account
        /// </summary>
        public static CarrierAccountEntityBuilder<T, TInterface> CarrierAccount<T, TInterface>()
            where T : EntityBase2, TInterface, new()
            where TInterface : ICarrierAccount =>
            new CarrierAccountEntityBuilder<T, TInterface>();

        /// <summary>
        /// Create a manual order
        /// </summary>
        public static OrderEntity CreateManualOrder(StoreEntity store, CustomerEntity customer, long orderNumber)
        {
            return Create.Order<OrderEntity>(store, customer)
                .WithItem<OrderItemEntity>(i => i
                    .Set(x => x.Description, orderNumber.ToString())
                    .Set(i2 => i2.Quantity, 2))
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Set(x => x.IsManual, true)
                .Save();
        }
    }
}
