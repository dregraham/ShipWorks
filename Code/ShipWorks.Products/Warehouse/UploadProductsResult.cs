using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of an upload request
    /// </summary>
    public class UploadProductsResult : IProductsChangeResult
    {
        private readonly UploadResponseData responseData;

        /// <summary>
        /// Constructor
        /// </summary>
        public UploadProductsResult(UploadResponseData responseData)
        {
            this.responseData = responseData;
        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(IEnumerable<ProductVariantEntity> productVariants)
        {
            foreach (var variant in productVariants)
            {
                var itemData = responseData.Results.FirstOrDefault(x => x.Sku == variant.DefaultSku);

                variant.Product.UploadToWarehouseNeeded = false;
                variant.HubVersion = itemData?.Version;

                if (!variant.HubProductId.HasValue)
                {
                    variant.HubProductId = string.IsNullOrEmpty(itemData?.Id) ? (Guid?) null : Guid.Parse(itemData.Id);
                }
            }
        }
    }
}
