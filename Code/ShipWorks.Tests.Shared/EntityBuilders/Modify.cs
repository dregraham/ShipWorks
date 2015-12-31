using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shared.EntityBuilders
{
    /// <summary>
    /// Build entities for testing
    /// </summary>
    public static class Modify
    {
        /// <summary>
        /// Create a generic entity
        /// </summary>
        public static EntityBuilder<T> Entity<T>(T original) where T : EntityBase2, new() =>
            new EntityBuilder<T>(original);

        /// <summary>
        /// Create an order
        /// </summary>
        public static OrderEntityBuilder Order(OrderEntity order) =>
            new OrderEntityBuilder(order);

        /// <summary>
        /// Create a shipment
        /// </summary>
        public static ShipmentEntityBuilder Shipment(ShipmentEntity shipment) =>
            new ShipmentEntityBuilder(shipment);

        /// <summary>
        /// Create a store entity
        /// </summary>
        public static StoreEntityBuilder<T> Store<T>(T store) where T : StoreEntity, new() =>
            new StoreEntityBuilder<T>(store);
    }
}
