using System;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Synchronization of products on the Hub
    /// </summary>
    [Component]
    public class WarehouseProductSynchronizer : IWarehouseProductSynchronizer
    {
        /// <summary>
        /// Perform the synchronization
        /// </summary>
        public Task Synchronize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
