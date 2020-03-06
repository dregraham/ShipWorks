using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// Represents the data for a batch upload request
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class UploadProductsRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadProductsRequest(IEnumerable<IProductVariantEntity> productVariants, string databaseId)
        {
            this.DatabaseId = databaseId;
            Products = productVariants.Select(x => new Product(x)).ToList();
        }

        /// <summary>
        /// Id of the ShipWorks database
        /// </summary>
        [JsonProperty("databaseId")]
        public string DatabaseId { get; set; }

        /// <summary>
        /// Collection of SKUs to upload
        /// </summary>
        [JsonProperty("products")]
        public IEnumerable<Product> Products { get; set; }
    }
}
