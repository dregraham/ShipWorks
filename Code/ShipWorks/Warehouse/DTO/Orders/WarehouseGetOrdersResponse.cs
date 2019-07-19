using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Orders
{
    [Obfuscation]
    public class WarehouseGetOrdersResponse
    {
        public IEnumerable<WarehouseOrder> Orders { get; set; }
        public WarehouseImportDetails ImportDetails { get; set; }

    }
}
