using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;

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
        /// Set given products activation to specified value
        /// </summary>
        Task SetActivation(IEnumerable<long> productIDs, bool activation, IProgressReporter progressReporter);
    }
}