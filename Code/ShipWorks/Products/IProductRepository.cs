using System.Collections.Generic;
using System.Threading.Tasks;
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

        /// <summary>
        /// Set given products activation to specified value
        /// </summary>
        Task SetActivation(IEnumerable<long> productIDs, bool activation);
    }
}