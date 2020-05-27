using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Products.Warehouse;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Upload products to an associated warehouse
    /// </summary>
    [Component]
    public class WarehouseProductUploader : IWarehouseProductUploader
    {
        private const int batchSize = 100;

        private readonly IProductCatalog productCatalog;
        private readonly IWarehouseProductClient warehouseProductClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public WarehouseProductUploader(
            IProductCatalog productCatalog,
            IWarehouseProductClient warehouseProductClient,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.warehouseProductClient = warehouseProductClient;
            this.productCatalog = productCatalog;
        }

        /// <summary>
        /// Upload changed products to the warehouse
        /// </summary>
        public async Task Upload(ISingleItemProgressDialog progressItem)
        {
            var progressUpdater = await CreateProgressUpdater(progressItem).ConfigureAwait(false);

            var shouldContinue = await PerformUpload(progressItem, progressUpdater, false)
                    .Bind(_ => PerformUpload(progressItem, progressUpdater, true))
                    .ConfigureAwait(false);

            shouldContinue
                .Do(x => progressItem.ProgressItem.Completed())
                .OnFailure(progressItem.ProgressItem.Failed);
        }

        /// <summary>
        /// Perform the upload to the Hub
        /// </summary>
        private async Task<GenericResult<bool>> PerformUpload(ISingleItemProgressDialog progressItem, IProgressUpdater progressUpdater, bool uploadBundles)
        {
            GenericResult<bool> shouldContinue;
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                do
                {
                    var skus = await productCatalog.FetchProductVariantsForUploadToWarehouse(sqlAdapter, batchSize, uploadBundles).ConfigureAwait(false);
                    var results = await warehouseProductClient.Upload(skus)
                        .Do(x => x.ApplyTo(skus))
                        .ToResult()
                        .ConfigureAwait(false);

                    if (results.Success)
                    {
                        await sqlAdapter.SaveEntityCollectionAsync(skus.ToEntityCollection(), false, true).ConfigureAwait(false);
                        progressUpdater.Update(batchSize);
                    }

                    shouldContinue = results.Map(() => skus.Any() && !progressItem.Provider.CancelRequested);
                } while (shouldContinue.Match(x => x, ex => false));
            }
            return shouldContinue;
        }

        /// <summary>
        /// Get a count of products that need to be uploaded
        /// </summary>
        public async Task<int> GetCountOfProductsThatNeedUpload()
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await productCatalog.FetchProductVariantsForUploadToWarehouseCount(sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create the progress updater
        /// </summary>
        private async Task<IProgressUpdater> CreateProgressUpdater(ISingleItemProgressDialog progressItem)
        {
            var productCount = await GetCountOfProductsThatNeedUpload().ConfigureAwait(false);
            return progressItem.ToUpdater(productCount);
        }
    }
}
