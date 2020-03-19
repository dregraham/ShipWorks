using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Response from call to get a product
    /// </summary>
    [Obfuscation]
    public class GetProductResponseData
    {
        /// <summary>
        /// WarehouseProduct returned
        /// </summary>
        public WarehouseProduct Product { get; set; }
    }
}
