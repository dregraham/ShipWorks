using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of an SetActivationBulk request
    /// </summary>
    public class SetActivationBulkResult : IProductsChangeResult
    {
        private readonly SetActivationBulkResponseData data;

        /// <summary>
        /// Constructor
        /// </summary
        public SetActivationBulkResult(SetActivationBulkResponseData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(IEnumerable<ProductVariantEntity> productVariants)
        {
            foreach (var (product, result) in productVariants.LeftJoin(data.Items, x => x.HubProductId, x => Guid.Parse(x.ProductId)))
            {
                product.HubVersion = result.Version;
            }
        }
    }
}
