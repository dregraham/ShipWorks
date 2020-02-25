using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Apply no change to a product variant batch
    /// </summary>
    public class NullProductsResult : IProductsChangeResult
    {
        /// <summary>
        /// Get the default implementation of the result
        /// </summary>
        public static IProductsChangeResult Default { get; } = new NullProductsResult();

        /// <summary>
        /// Constructor
        /// </summary>
        private NullProductsResult()
        {
            // Do nothing
        }

        /// <summary>
        /// Apply changes to the product variant batch
        /// </summary>
        public void ApplyTo(IEnumerable<ProductVariantEntity> productVariant)
        {
            // Do nothing
        }
    }
}
