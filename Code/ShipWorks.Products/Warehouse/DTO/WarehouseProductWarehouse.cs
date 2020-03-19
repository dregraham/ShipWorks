using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Warehouse association for a warehouse product
    /// </summary>
    [Obfuscation]
    public class WarehouseProductWarehouse
    {
        /// <summary>
        /// Id of the warehouse
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Is the product enabled for the warehouse
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Bin location of the product in the warehouse
        /// </summary>
        public string BinLocation { get; set; }
    }
}
