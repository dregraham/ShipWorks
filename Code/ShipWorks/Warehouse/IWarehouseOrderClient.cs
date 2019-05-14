using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    public interface IWarehouseOrderClient
    {
        Task<IEnumerable<WarehouseOrder>> GetOrders(string warehouseID);
    }
}