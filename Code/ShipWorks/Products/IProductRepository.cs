using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product repo interface
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Fetch a product variant based on SKU
        /// </summary>
        IProductVariantEntity FetchProductVariantReadOnly(string sku);
    }
}