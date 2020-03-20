using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Null implementation for ecommerce customers
    /// </summary>
    public class NullGetProductsAfterSequenceResult : IGetProductsAfterSequenceResult
    {
        /// <summary>
        /// Default instance of this class
        /// </summary>
        public static IGetProductsAfterSequenceResult Default { get; } = new NullGetProductsAfterSequenceResult();

        /// <summary>
        /// Constructor
        /// </summary>
        private NullGetProductsAfterSequenceResult()
        {

        }

        /// <summary>
        /// Get products from the Hub after newest sequence in the db
        /// </summary>=
        /// <returns>True if more products to get, false otherwise</returns>
        public Task<(long sequence, bool shouldContinue)> Apply(CancellationToken cancellationToken) =>
            Task.FromResult((0L, false));
    }
}