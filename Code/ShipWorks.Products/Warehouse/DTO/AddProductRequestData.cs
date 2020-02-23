using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Request data for adding a product to the Hub
    /// </summary>
    [Obfuscation]
    public class AddProductRequestData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddProductRequestData(IProductVariantEntity product, string warehouseId)
        {
            Product = new Product(product);
            WarehouseId = warehouseId;
        }

        /// <summary>
        /// Id of the warehouse to which the product should be added
        /// </summary>
        [JsonProperty("warehouseId")]
        public string WarehouseId { get; set; }

        /// <summary>
        /// Product that should be added
        /// </summary>
        public Product Product { get; set; }
    }

    /// <summary>
    /// Response data from adding a product to the hub
    /// </summary>
    [Obfuscation]
    public class AddProductResponseData
    {
        /// <summary>
        /// Id of the product that was added
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Version of the product on the Hub
        /// </summary>
        /// <remarks>
        /// This is used for optimistic concurrency
        /// </remarks>
        public int Version { get; set; }
    }
}
