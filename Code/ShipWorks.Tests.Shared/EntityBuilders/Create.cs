using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build entities for testing
    /// </summary>
    public static class Create
    {
        /// <summary>
        /// Create a generic entity
        /// </summary>
        public static EntityBuilder<T> Entity<T>() where T : EntityBase2, new() => new EntityBuilder<T>();

        /// <summary>
        /// Create an order
        /// </summary>
        public static OrderEntityBuilder Order(StoreEntity store, CustomerEntity customer) =>
            new OrderEntityBuilder(store, customer);

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
        /// Create a store entity
        /// </summary>
        public static StoreEntityBuilder<T> Store<T>() where T : StoreEntity, new() =>
            new StoreEntityBuilder<T>();

        /// <summary>
        /// Create a carrier account
        /// </summary>
        public static CarrierAccountEntityBuilder<T> CarrierAccount<T>() where T : EntityBase2, ICarrierAccount, new() =>
            new CarrierAccountEntityBuilder<T>();
    }
}
