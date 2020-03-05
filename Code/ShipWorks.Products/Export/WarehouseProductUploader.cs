using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

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
        private readonly IUploadSkusToWarehouse uploadRequest;
        private readonly IConfigurationData configuration;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public WarehouseProductUploader(
            IProductCatalog productCatalog,
            IUploadSkusToWarehouse uploadRequest,
            ISqlAdapterFactory sqlAdapterFactory,
            IConfigurationData configuration)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.configuration = configuration;
            this.uploadRequest = uploadRequest;
            this.productCatalog = productCatalog;
        }

        /// <summary>
        /// Upload changed products to the warehouse
        /// </summary>
        public async Task Upload(ISingleItemProgressDialog progressItem)
        {
            string warehouseId = configuration.FetchReadOnly().WarehouseID;
            var progressUpdater = await CreateProgressUpdater(progressItem).ConfigureAwait(false);

            GenericResult<bool> shouldContinue;
            do
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    var skus = await productCatalog.FetchProductVariantsForUploadToWarehouse(sqlAdapter, batchSize).ConfigureAwait(false);
                    var results = await uploadRequest.Upload(new UploadProductsRequest(skus, warehouseId)).ConfigureAwait(false);

                    if (results.Success)
                    {
                        await productCatalog.ResetNeedsWarehouseUploadFlag(sqlAdapter, skus).ConfigureAwait(false);
                        progressUpdater.Update(batchSize);
                    }

                    shouldContinue = results.Map(() => skus.Any() && !progressItem.Provider.CancelRequested);
                }
            } while (shouldContinue.Match(x => x, ex => false));

            shouldContinue
                .Do(x => progressItem.ProgressItem.Completed())
                .OnFailure(progressItem.ProgressItem.Failed);
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
