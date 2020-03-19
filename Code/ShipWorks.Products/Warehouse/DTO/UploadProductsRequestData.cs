using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Warehouse.DTO
{

    /// <summary>
    /// Represents the data for a batch upload request
    /// </summary>
    [Obfuscation]
    public class UploadProductsRequestData : IWarehouseProductRequestData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadProductsRequestData(IEnumerable<IProductVariantEntity> productVariants)
        {
            Products = productVariants.WhereNotNull().Select(Product.Create).ToList();
        }

        /// <summary>
        /// Id of the warehouse on the Hub
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Collection of SKUs to upload
        /// </summary>
        public IEnumerable<Product> Products { get; set; }
    }
}
