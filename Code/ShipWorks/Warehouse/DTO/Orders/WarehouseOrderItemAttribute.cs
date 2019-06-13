using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Orders
{
    /// <summary>
    /// Item attribute for warehouse item
    /// </summary>
    [Obfuscation]
    public class WarehouseOrderItemAttribute
    {
        /// <summary>
        /// Name of the item attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the item attribute
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unit price of the item attribute
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
