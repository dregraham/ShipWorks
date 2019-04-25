using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Upload products to an associated warehouse
    /// </summary>
    [Component]
    public class WarehouseProductUploader : IWarehouseProductUploader
    {
        private const int batchSize = 1;

        private readonly IProductCatalog productCatalog;
        private readonly IUploadSkusToWarehouse uploadRequest;
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        public WarehouseProductUploader(
            IProductCatalog productCatalog,
            IUploadSkusToWarehouse uploadRequest,
            ISqlAdapterFactory sqlAdapterFactory,
            IDatabaseIdentifier databaseIdentifier)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.databaseIdentifier = databaseIdentifier;
            this.uploadRequest = uploadRequest;
            this.productCatalog = productCatalog;
        }

        /// <summary>
        /// Upload changed products to the warehouse
        /// </summary>
        public async Task Upload(ISingleItemProgressDialog progressItem)
        {
            string databaseId = databaseIdentifier.Get().ToString();
            Func<IProductVariantEntity, SkusToUploadDto> createDto = x => new SkusToUploadDto(x, databaseId);
            var progressUpdater = await CreateProgressUpdater(progressItem).ConfigureAwait(false);

            bool shouldContinue = true;
            do
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    var skus = await productCatalog.FetchProductVariantsForUploadToWarehouse(sqlAdapter, batchSize).ConfigureAwait(false);
                    var results = await uploadRequest.Upload(skus.Select(createDto)).ConfigureAwait(false);
                    progressUpdater.Update(batchSize);

                    shouldContinue = results.Match(skus.Any, x =>
                    {
                        progressItem.ProgressItem.Failed(x);
                        return false;
                    });
                }
            } while (shouldContinue);

            progressItem.ProgressItem.Completed();
        }

        /// <summary>
        /// Create the progress updater
        /// </summary>
        private async Task<IProgressUpdater> CreateProgressUpdater(ISingleItemProgressDialog progressItem)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var productCount = await productCatalog.FetchProductVariantsForUploadToWarehouseCount(sqlAdapter).ConfigureAwait(false);
                return progressItem.ToUpdater(productCount);
            }
        }
    }
}
