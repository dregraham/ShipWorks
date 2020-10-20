using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Warehouse Order DTO Factory
    /// </summary>
    public interface IWarehouseOrderDtoFactory
    {
        /// <summary>
        /// Create a WarehouseOrder from a OrderEntity
        /// </summary>
        WarehouseOrder Create(OrderEntity order, IStoreEntity store);
    }
}