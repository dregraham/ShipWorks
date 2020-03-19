using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of a product batch change
    /// </summary>
    public interface IProductsChangeResult
    {
        /// <summary>
        /// Apply changes to the product variants
        /// </summary>
        void ApplyTo(IEnumerable<ProductVariantEntity> productVariants);
    }
}
