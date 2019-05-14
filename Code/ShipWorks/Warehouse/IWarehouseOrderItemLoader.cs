using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    public interface IWarehouseOrderItemLoader
    {
        void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseOrderItem);
    }
}