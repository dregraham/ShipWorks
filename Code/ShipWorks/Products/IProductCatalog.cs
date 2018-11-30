using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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

        /// <summary>
        /// Fetch a product
        /// </summary>
        ProductVariantEntity FetchProductVariant(long productVariantID);

        /// <summary>
        /// Set given products activation to specified value
        /// </summary>
        Task SetActivation(IEnumerable<long> productIDs, bool activation, IProgressReporter progressReporter);

        /// <summary>
        /// Save the given product
        /// </summary>
        Result Save(ProductEntity product);
    }
}