using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Result of a product change
    /// </summary>
    public interface IProductChangeResult
    {
        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        void ApplyTo(ProductVariantEntity productVariant);
    }
}
