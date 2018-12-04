using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Class for importing products.
    /// </summary>
    [Component]
    public class ProductImporter : IProductImporter
    {
        /// <summary>
        /// Import products for the given spreadsheet file name and progress reporter.
        /// </summary>
#pragma warning disable 1998
        public async Task<GenericResult<ImportProductsResult>> ImportProducts(string pathAndFilename, IProgressReporter progressReporter)
#pragma warning restore 1998
        {
            GenericResult<ImportProductsResult> result = new GenericResult<ImportProductsResult>();

            return result;
        }
    }
}
