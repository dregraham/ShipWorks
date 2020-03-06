using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Apply no change to a product variant
    /// </summary>
    public class NullProductResult : IProductChangeResult
    {
        /// <summary>
        /// Get the default implementation of the result
        /// </summary>
        public static IProductChangeResult Default { get; } = new NullProductResult();

        /// <summary>
        /// Constructor
        /// </summary>
        private NullProductResult()
        {
            // Do nothing
        }

        /// <summary>
        /// Apply changes to the product variant
        /// </summary>
        public void ApplyTo(ProductVariantEntity productVariant)
        {
            // Do nothing
        }
    }
}
