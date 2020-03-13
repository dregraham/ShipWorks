using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of an upload request
    /// </summary>
    public class UploadProductsResult : IProductsChangeResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadProductsResult(UploadResponseData request)
        {

        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(IEnumerable<ProductVariantEntity> productVariants)
        {
            throw new NotImplementedException();
        }
    }
}
