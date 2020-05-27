using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Class for handling syncing product bundles 
    /// </summary>
    [Component]
    public class WarehouseProductBundleService : IWarehouseProductBundleService
    {
        private readonly IWarehouseProductClient warehouseProductClient;
        private readonly IProductCatalog productCatalog;
        private readonly Func<WarehouseProduct, IHubProductUpdater> createHubProductUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductBundleService(IWarehouseProductClient warehouseProductClient, IProductCatalog productCatalog,
            Func<WarehouseProduct, IHubProductUpdater> createHubProductUpdater)
        {
            this.warehouseProductClient = warehouseProductClient;
            this.productCatalog = productCatalog;
            this.createHubProductUpdater = createHubProductUpdater;
        }

        /// <summary>
        /// Handle syncing product bundles 
        /// </summary>
        public async Task UpdateProductBundleDetails(ISqlAdapter sqlAdapter, ProductVariantEntity productVariant,
            WarehouseProduct warehouseProductDto, CancellationToken cancellationToken)
        {
            if (!warehouseProductDto.IsBundle)
            {
                return;
            }

            if (productVariant == null)
            {
                throw new ArgumentNullException("productVariant");
            }

            if (productVariant.Product.Bundles.RemovedEntitiesTracker == null)
            {
                productVariant.Product.Bundles.RemovedEntitiesTracker = new EntityCollection<ProductBundleEntity>();
            }

            foreach (var bundle in productVariant.Product.Bundles.ToList())
            {
                productVariant.Product.Bundles.Remove(bundle);
                productVariant.Product.Bundles.RemovedEntitiesTracker.Add(bundle);
            }

            // Try to get all hub product ids from the sw db, keep a list of 
            foreach (var huBundledProduct in warehouseProductDto.BundleItems)
            {
                var swBundledProdEntity = productCatalog.FetchProductVariantEntity(sqlAdapter, Guid.Parse(huBundledProduct.Key));

                if (swBundledProdEntity == null)
                {
                    swBundledProdEntity = await AddProductBundle(huBundledProduct.Value, sqlAdapter, cancellationToken).ConfigureAwait(false);
                }
                else if (swBundledProdEntity.Product.IsBundle)
                {
                    throw new WarehouseProductException($"Bundles should not be comprised of bundles. Parent Sku: ${productVariant.DefaultSku}, Child ProductId: ${huBundledProduct.Value.ProductId}");
                }

                productVariant.Product.Bundles.Add(new ProductBundleEntity()
                {
                    ChildVariant = swBundledProdEntity,
                    ChildProductVariantID = swBundledProdEntity.ProductVariantID,
                    ProductID = productVariant.ProductID,
                    Quantity = huBundledProduct.Value.Quantity
                });
            }

            await productCatalog.SaveProductToDatabase(productVariant, sqlAdapter).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a product bundle
        /// </summary>
        private async Task<ProductVariantEntity> AddProductBundle(BundledProduct bundledProduct, ISqlAdapter sqlAdapter, CancellationToken cancellationToken)
        {
            var productDto = await warehouseProductClient.GetProduct(bundledProduct.ProductId, cancellationToken).ConfigureAwait(false);
            var updater = createHubProductUpdater(productDto);

            var productVariant = ProductVariantEntity.Create(productDto.Sku, productDto.CreatedDate);
            updater.ProductVariant = productVariant;
            updater.UpdateProductVariant();

            await sqlAdapter.SaveAndRefetchAsync(productVariant.Product).ConfigureAwait(false);

            return productVariant;
        }
    }
}
