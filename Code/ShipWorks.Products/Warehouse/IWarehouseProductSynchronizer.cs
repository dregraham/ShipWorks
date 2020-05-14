using System.Threading;
using System.Threading.Tasks;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Synchronization of products on the Hub
    /// </summary>
    public interface IWarehouseProductSynchronizer
    {
        /// <summary>
        /// Perform the synchronization
        /// </summary>
        Task Synchronize(CancellationToken cancellationToken);
    }
}
