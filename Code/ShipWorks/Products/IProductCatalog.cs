using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product Catalog Interface
    /// </summary>
    public interface IProductCatalog
    {
       /// <summary>
       /// Fetch a product
       /// </summary>
        IProductVariant FetchProductVariant(string sku);
    }
}