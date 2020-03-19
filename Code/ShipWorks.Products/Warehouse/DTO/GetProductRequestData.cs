using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Request for getting a product by productId 
    /// </summary>
    [Obfuscation]
    public class GetProductRequestData
    {
        /// <summary>
        /// ProductId of product to be retrieved
        /// </summary>
        public string ProductId { get; set; }
    }
}
