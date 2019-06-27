using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Order loader for loading ShipWorks Warehouse orders
    /// </summary>
    public interface IWarehouseOrderFactory
    {
        /// <summary>
        /// Load the order details from the warehouse order into the order entity
        /// </summary>
        Task<OrderEntity> CreateOrder(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder);
    }
}
