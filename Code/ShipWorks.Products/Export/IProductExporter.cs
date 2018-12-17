using System.Threading.Tasks;
using Interapptive.Shared.Threading;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Class for exporting products
    /// </summary>
    public interface IProductExporter
    {
        /// <summary>
        /// Export products to the given file
        /// </summary>
        Task Export(string filePath, IProgressReporter progressReporter);
    }
}