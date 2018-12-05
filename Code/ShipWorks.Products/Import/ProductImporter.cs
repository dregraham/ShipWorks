﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Class for importing products.
    /// </summary>
    [Component]
    public class ProductImporter : IProductImporter
    {
        private readonly IProductExcelReader productExcelReader;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IProductCatalog productCatalog;
        private IProgressReporter itemProgressReporter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporter(IProductExcelReader productExcelReader, ISqlAdapterFactory sqlAdapterFactory, IProductCatalog productCatalog)
        {
            this.productExcelReader = productExcelReader;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productCatalog = productCatalog;
        }

        /// <summary>
        /// Import products for the given spreadsheet file name and progress reporter.
        /// </summary>
        public async Task<GenericResult<ImportProductsResult>> ImportProducts(string pathAndFilename, IProgressReporter progressReporter)
        {
            itemProgressReporter = progressReporter;
            ImportProductsResult result = new ImportProductsResult(0, 0, 0);

            itemProgressReporter.PercentComplete = 0;

            var fileLoadResults = productExcelReader.LoadImportFile(pathAndFilename);

            if (fileLoadResults.Failure)
            {
                return GenericResult.FromError(fileLoadResults.Exception, result);
            }

            result = await ProcessRows(fileLoadResults.Value.SkuRows, fileLoadResults.Value.BundleRows).ConfigureAwait(false);

            if (result.FailedCount > 0 || result.FailureResults.Any())
            {
                return GenericResult.FromError(new FailedProductImportException(result), result);
            }

            return GenericResult.FromSuccess(result);
        }

        /// <summary>
        /// Process each product row
        /// </summary>
        private async Task<ImportProductsResult> ProcessRows(List<ProductToImportDto> skuRows, List<ProductToImportDto> bundleRows)
        {
            ImportProductsResult results = new ImportProductsResult(skuRows.Count + bundleRows.Count, 0, 0);

            await ProcessSkus(skuRows, results).ConfigureAwait(false);

            await ProcessBundles(bundleRows, results).ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Import each non-bundle sku into the database
        /// </summary>
        private async Task ProcessSkus(List<ProductToImportDto> skuRows, ImportProductsResult result)
        {
            // Loop through each row after the header rows
            foreach (ProductToImportDto row in skuRows)
            {
                if (row.Sku.IsNullOrWhiteSpace())
                {
                    continue;
                }

                try
                {
                    await sqlAdapterFactory.WithPhysicalTransactionAsync(async (transaction, sqlAdapter) =>
                    {
                        ProductVariantEntity productVariant = FindExistingProductVariant(sqlAdapter, row.Sku);

                        await sqlAdapter.DeleteEntityCollectionAsync(productVariant.Aliases).ConfigureAwait(false);
                        productVariant.Aliases.Clear();

                        if (productVariant.Product != null)
                        {
                            await sqlAdapter.DeleteEntityCollectionAsync(productVariant.Product.Bundles).ConfigureAwait(false);
                            productVariant.Product.Bundles.Clear();
                        }

                        CopyCsvDataToProduct(productVariant, row);

                        return await sqlAdapter.SaveEntityAsync(productVariant.Product, true, true)
                            .ContinueWith(task =>
                            {
                                sqlAdapter.Commit();
                                return true;
                            })
                            .ConfigureAwait(false);
                    });

                    result.SuccessCount++;
                }
                catch (Exception e)
                {
                    result.FailedCount++;
                    result.FailureResults.Add(row.Sku, e.Message);
                }

                if (itemProgressReporter.IsCancelRequested)
                {
                    break;
                }

                itemProgressReporter.PercentComplete = (int) Math.Round(100 * (((decimal) result.SuccessCount + result.FailedCount) / result.TotalCount));
            }
        }

        /// <summary>
        /// Import each bundle sku into the database
        /// </summary>
        private async Task ProcessBundles(List<ProductToImportDto> bundleRows, ImportProductsResult result)
        {
            // Loop through each row after the header rows
            foreach (ProductToImportDto row in bundleRows)
            {
                if (row.Sku.IsNullOrWhiteSpace())
                {
                    continue;
                }

                try
                {
                    await sqlAdapterFactory.WithPhysicalTransactionAsync(async (transaction, sqlAdapter) =>
                    {
                        ProductVariantEntity bundleProductVariant = FindExistingProductVariant(sqlAdapter, row.Sku);

                        await sqlAdapter.DeleteEntityCollectionAsync(bundleProductVariant.Aliases).ConfigureAwait(false);
                        bundleProductVariant.Aliases.Clear();

                        CopyCsvDataToProduct(bundleProductVariant, row);

                        await ImportProductVariantBundles(bundleProductVariant, row, sqlAdapter).ConfigureAwait(false);

                        return await sqlAdapter.SaveEntityAsync(bundleProductVariant.Product, true, true)
                            .ContinueWith(task =>
                            {
                                sqlAdapter.Commit();
                                return true;
                            })
                            .ConfigureAwait(false);
                    });

                    result.SuccessCount++;
                }
                catch (Exception e)
                {
                    result.FailedCount++;
                    result.FailureResults.Add(row.Sku, e.Message);
                }

                if (itemProgressReporter.IsCancelRequested)
                {
                    break;
                }

                itemProgressReporter.PercentComplete = (int) Math.Round(100 * (((decimal) result.SuccessCount + result.FailedCount) / result.TotalCount));
            }
        }

        /// <summary>
        /// Find an existing product variant
        /// </summary>
        private ProductVariantEntity FindExistingProductVariant(ISqlAdapter sqlAdapter, string sku)
        {
            ProductVariantEntity productVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, sku);
            return productVariant ?? new ProductVariantEntity();
        }

        /// <summary>
        /// Processes the row.
        /// </summary>
        private void CopyCsvDataToProduct(ProductVariantEntity productVariant, ProductToImportDto row)
        {
            if (productVariant.Product == null)
            {
                productVariant.Product = new ProductEntity()
                {
                    CreatedDate = DateTime.UtcNow,
                    IsBundle = !row.BundleSkus.IsNullOrWhiteSpace(),
                };
            }
            else
            {
                productVariant.Product.IsBundle = !row.BundleSkus.IsNullOrWhiteSpace();
            }

            productVariant.CreatedDate = DateTime.UtcNow;
            productVariant.Name = row.Name;
            productVariant.UPC = row.Upc;
            productVariant.ASIN = row.Asin;
            productVariant.ISBN = row.Isbn;
            productVariant.Weight = ProductToImportDto.GetValue<decimal>(row.Weight, nameof(row.Weight));
            productVariant.Length = ProductToImportDto.GetValue<decimal>(row.Length, nameof(row.Length));
            productVariant.Width = ProductToImportDto.GetValue<decimal>(row.Width, nameof(row.Width));
            productVariant.Height = ProductToImportDto.GetValue<decimal>(row.Height, nameof(row.Height));
            productVariant.ImageUrl = row.ImageUrl;
            productVariant.BinLocation = row.WarehouseBin;
            productVariant.DeclaredValue = ProductToImportDto.GetValue<decimal>(row.DeclaredValue, nameof(row.DeclaredValue));
            productVariant.CountryOfOrigin = row.CountryOfOrigin;
            productVariant.HarmonizedCode = row.HarmonizedCode;
            productVariant.IsActive = row.IsActive;

            ImportProductVariantAliases(productVariant, row.Sku, row);

            productVariant.Product.IsActive = productVariant.IsActive;
            productVariant.Product.Name = productVariant.Name;
        }

        /// <summary>
        /// Import aliases
        /// </summary>
        private void ImportProductVariantAliases(ProductVariantEntity productVariant, string mainSku, ProductToImportDto productVariantDto)
        {
            List<ProductVariantAliasEntity> newAliases = new List<ProductVariantAliasEntity>()
                { new ProductVariantAliasEntity() {IsDefault = true, Sku = mainSku, AliasName = mainSku}};

            if (productVariantDto.AliasSkuList.Any())
            {
                newAliases.AddRange(productVariantDto.AliasSkuList
                    .Where(s => !s.IsNullOrWhiteSpace())
                    .Select(s => new ProductVariantAliasEntity() { IsDefault = false, Sku = s, AliasName = s }));
            }

            // Aliases that didn't exist that need to be created.
            newAliases
                .ForEach(a => productVariant.Aliases.Add(
                    new ProductVariantAliasEntity()
                    {
                        ProductVariant = productVariant,
                        Sku = a.Sku,
                        AliasName = a.Sku,
                        IsDefault = a.IsDefault
                    }));
        }

        /// <summary>
        /// Import aliases
        /// </summary>
        private async Task ImportProductVariantBundles(ProductVariantEntity bundleProductVariant, ProductToImportDto productVariantDto,
            ISqlAdapter sqlAdapter)
        {
            if (productVariantDto.BundleSkuList.Any(s => productVariantDto.AliasSkuList.Any( a => a == s.Sku)))
            {
                throw new ProductImportException($"Bundles cannot reference themselves.  Product SKU: {productVariantDto.Sku}");
            }

            // Delete any bundles currently associated.  We'll add them back later if any were requested.
            await sqlAdapter.DeleteEntityCollectionAsync(bundleProductVariant.Product.Bundles).ConfigureAwait(false);
            bundleProductVariant.Product.Bundles.Clear();

            if (productVariantDto.BundleSkuList.None())
            {
                // No bundles requested, so just return.
                // If the variant had bundles associated, but then do an import where the cell is empty,
                // we are assuming they wanted to mark the variant as NOT being a bundle any more.
                return;
            }

            // Get a list of what the final bundle list should be
            List<ProductBundleEntity> bundlesToCreate = productVariantDto.BundleSkuList
                .Where(s => !s.Sku.IsNullOrWhiteSpace())
                .Select(s =>
                {
                    ProductVariantEntity childVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, s.Sku);

                    if (childVariant == null)
                    {
                        throw new ProductImportException($"Unable to import bundle SKU {bundleProductVariant.Aliases.First(a => a.IsDefault).Sku} because child SKU '{s.Sku}' could not be found.");
                    }

                    return new ProductBundleEntity()
                    {
                        Product = bundleProductVariant.Product,
                        ChildVariant = childVariant,
                        ChildProductVariantID = childVariant.ProductVariantID,
                        Quantity = s.Quantity
                    };

                }).ToList();

            bundleProductVariant.Product.Bundles.AddRange(bundlesToCreate);
        }
    }
}
