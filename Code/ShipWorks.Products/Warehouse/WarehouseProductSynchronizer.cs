using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Synchronization of products on the Hub
    /// </summary>
    [Component]
    public class WarehouseProductSynchronizer : IWarehouseProductSynchronizer
    {
        private readonly ILicenseService licenseService;
        private readonly IProductCatalog productCatalog;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IWarehouseProductClient warehouseProductClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductSynchronizer(
            ILicenseService licenseService,
            IProductCatalog productCatalog,
            ISqlAdapterFactory sqlAdapterFactory,
            IWarehouseProductClient warehouseProductClient)
        {
            this.licenseService = licenseService;
            this.productCatalog = productCatalog;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.warehouseProductClient = warehouseProductClient;
        }

        /// <summary>
        /// Perform the synchronization
        /// </summary>
        public async Task Synchronize(CancellationToken cancellationToken)
        {
            if (!licenseService.IsHub)
            {
                return;
            }

            var sequence = await GetNewestSequence(cancellationToken);
            bool shouldContinue;

            do
            {
                var result = await warehouseProductClient.GetProductsAfterSequence(sequence, cancellationToken);
                (sequence, shouldContinue) = await Functional.UsingAsync(
                        sqlAdapterFactory.Create(),
                        sqlAdapter => result.Apply(sqlAdapter, cancellationToken))
                    .ConfigureAwait(false);
            } while (shouldContinue && !cancellationToken.IsCancellationRequested);
        }

        /// <summary>
        /// Gets the newest product sequence
        /// </summary>
        private async Task<long> GetNewestSequence(CancellationToken cancellationToken)
        {
            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                return await productCatalog.FetchNewestSequence(sqlAdapter, cancellationToken);
            }
        }
    }
}
