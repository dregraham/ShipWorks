using System.Threading.Tasks;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Order loader for loading ShipWorks Warehouse orders
    /// </summary>
    public interface IWarehouseOrderLoader
    {
        /// <summary>
        /// Load the order details from the warehouse order into the order entity
        /// </summary>
        Task LoadOrder(WarehouseOrder warehouseOrder);
    }
}
