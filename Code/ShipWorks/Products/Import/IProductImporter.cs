using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Interface for importing products.
    /// </summary>
    public interface IProductImporter
    {
        /// <summary>
        /// Import products for the given spreadsheet file name and progress reporter.
        /// </summary>
        Task<GenericResult<ImportProductsResult>> ImportProducts(string pathAndFilename, IProgressReporter progressReporter);
    }
}
