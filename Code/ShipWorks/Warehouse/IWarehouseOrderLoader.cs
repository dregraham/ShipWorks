using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    public interface IWarehouseOrderLoader
    {
        void LoadOrder(OrderEntity orderEntity, WarehouseOrder warehouseOrder);
    }
}
