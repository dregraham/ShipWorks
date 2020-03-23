using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Get products from the Hub after newest sequence in the db
    /// </summary>
    public class GetProductsAfterSequenceResult : IGetProductsAfterSequenceResult
    {
        private readonly GetProductsAfterSequenceResponseData data;
        private readonly IProductCatalog productCatalog;
        private readonly IWarehouseProductBundleService bundleService;
        private readonly Func<WarehouseProduct, IHubProductUpdater> createHubProductUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetProductsAfterSequenceResult(
            GetProductsAfterSequenceResponseData data,
            IProductCatalog productCatalog,
            IWarehouseProductBundleService bundleService,
            Func<WarehouseProduct, IHubProductUpdater> createHubProductUpdater)
        {
            this.data = data;
            this.productCatalog = productCatalog;
            this.bundleService = bundleService;
            this.createHubProductUpdater = createHubProductUpdater;
        }

        /// <summary>
        /// Get products from the Hub after newest sequence in the db
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>True if more products to get, false otherwise</returns>
        public async Task<(long sequence, bool shouldContinue)> Apply(ISqlAdapter sqlAdapter, CancellationToken cancellationToken)
        {
            var hubDetails = data.Products.ToDictionary(x => Guid.Parse(x.ProductId), createHubProductUpdater);

            var existingKnownProducts = await productCatalog.GetProductsByHubIds(sqlAdapter, hubDetails.Keys).ConfigureAwait(false);
            foreach (var product in existingKnownProducts)
            {
                hubDetails[product.HubProductId.Value].ProductVariant = product;
            }

            var unknownProductSkus = hubDetails.Values.Where(x => x.ProductVariant == null).Select(x => x.ProductData.Sku).ToArray();
            var existingUnknownProducts = await productCatalog.FetchProductVariantEntities(sqlAdapter, unknownProductSkus, true).ConfigureAwait(false);
            foreach (var product in existingUnknownProducts)
            {
                var hubProductId = hubDetails.Where(x => x.Value.ProductData.Sku == product.DefaultSku).Select(x => x.Key).First();
                product.HubProductId = hubProductId;
                hubDetails[product.HubProductId.Value].ProductVariant = product;
            }

            var missingProducts = hubDetails.Where(x => x.Value.ProductVariant == null);
            foreach (var productData in missingProducts)
            {
                var product = ProductVariantEntity.Create(productData.Value.ProductData.Sku, productData.Value.ProductData.CreatedDate.ToSqlSafeDateTime());
                product.HubProductId = productData.Key;
                productData.Value.ProductVariant = product;
            }

            foreach (var productData in hubDetails.Values)
            {
                productData.UpdateProductVariant();
            }

            var productsToSave = hubDetails.Values.Select(x => x.ProductVariant).ToEntityCollection();
            await sqlAdapter.SaveEntityCollectionAsync(productsToSave, true, true, cancellationToken).ConfigureAwait(false);

            foreach (var productData in hubDetails.Values)
            {
                await bundleService
                    .UpdateProductBundleDetails(sqlAdapter, productData.ProductVariant, productData.ProductData, cancellationToken)
                    .ConfigureAwait(false);
            }

            if (productsToSave.Any())
            {
            await sqlAdapter.SaveEntityCollectionAsync(productsToSave, false, true, cancellationToken).ConfigureAwait(false);
            }

            bool anyToReturn = hubDetails.Values.Any();
            return (
                sequence: anyToReturn ? hubDetails.Values.Select(x => x.ProductData.Sequence).Max() : 0,
                shouldContinue: anyToReturn);
        }
    }
}
