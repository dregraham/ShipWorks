using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// A result of a product import
    /// </summary>
    [Obfuscation]
    public class UploadResponseDataResult
    {
        /// <summary>
        /// Id of the product on the Hub
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sku of the product
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Version of the product
        /// </summary>
        public int Version { get; set; }
    }
}
