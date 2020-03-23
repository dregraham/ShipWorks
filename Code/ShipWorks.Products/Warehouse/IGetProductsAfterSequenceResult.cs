using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Get products from the Hub after newest sequence in the db
    /// </summary>
    public interface IGetProductsAfterSequenceResult
    {
        /// <summary>
        /// Get products from the Hub after newest sequence in the db
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>True if more products to get, false otherwise</returns>
        Task<(long sequence, bool shouldContinue)> Apply(ISqlAdapter sqlAdapter, CancellationToken cancellationToken);
    }
}
