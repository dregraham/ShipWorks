using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Stores;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Client for retrieving orders from the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseOrderClient
    {
        /// <summary>
        /// Get orders for the given warehouse store ID from the ShipWorks Warehouse app
        /// </summary>
        Task<IEnumerable<WarehouseOrder>> GetOrders(string warehouseID, string warehouseStoreID, DateTime lastModified,
                                                    StoreTypeCode storeType);
    }
}
