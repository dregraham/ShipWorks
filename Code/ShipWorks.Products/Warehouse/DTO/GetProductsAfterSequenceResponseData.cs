using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Response from the GetProductsAfterSequence request
    /// </summary>
    [Obfuscation]
    public class GetProductsAfterSequenceResponseData
    {
        /// <summary>
        /// List of products returned
        /// </summary>
        public IEnumerable<WarehouseProduct> Products { get; set; }

        /// <summary>
        /// Count of products returned
        /// </summary>
        public int Count { get; set; }
    }
}
