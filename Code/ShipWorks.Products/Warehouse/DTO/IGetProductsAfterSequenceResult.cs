using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShipWorks.Products.Warehouse.DTO
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
        Task<(long sequence, bool shouldContinue)> Apply(CancellationToken cancellationToken);
    }
}
