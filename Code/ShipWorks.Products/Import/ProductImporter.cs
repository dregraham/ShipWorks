﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

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
        private readonly ILog log;
        private IProgressReporter itemProgressReporter;
        private List<ProductToImportDto> allSkus;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporter(IProductExcelReader productExcelReader, ISqlAdapterFactory sqlAdapterFactory,
            IProductCatalog productCatalog, Func<Type, ILog> logFactory)
        {
            this.productExcelReader = productExcelReader;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productCatalog = productCatalog;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Import products for the given spreadsheet file name and progress reporter.
        /// </summary>
        public async Task<GenericResult<IImportProductsResult>> ImportProducts(string pathAndFilename, IProgressReporter progressReporter)
        {
            itemProgressReporter = progressReporter;
            IImportProductsResult result = new ImportProductsResult(0, 0, 0);

            itemProgressReporter.PercentComplete = 0;

            var fileLoadResults = await Task.Run(() => productExcelReader.LoadImportFile(pathAndFilename)).ConfigureAwait(false);

            if (fileLoadResults.Failure)
            {
                return GenericResult.FromError(fileLoadResults.Exception, result);
            }

            allSkus = fileLoadResults.Value.SkuRows.Concat(fileLoadResults.Value.BundleRows).ToList();

            if (allSkus.None())
            {
                return GenericResult.FromError("No records found in file.", result);
            }

            var counts = new ProductTelemetryCounts("Import");

            result = await ProcessRows(fileLoadResults.Value.SkuRows, fileLoadResults.Value.BundleRows, counts).ConfigureAwait(false);

            counts.SendTelemetry();

            if (result.FailedCount > 0 || result.FailureResults.Any())
            {
                LogFailures(result);

                return GenericResult.FromError(new FailedProductImportException(result), result);
            }

            return GenericResult.FromSuccess(result);
        }

        /// <summary>
        /// Log failure results
        /// </summary>
        private void LogFailures(IImportProductsResult result)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            result.FailureResults.ForEach(f => sb.AppendLine($"Failed to import SKU '{f.Key}'.  Error: '{f.Value}'"));
            sb.AppendLine();
            log.Error(sb);
        }

        /// <summary>
        /// Process each product row
        /// </summary>
        private async Task<ImportProductsResult> ProcessRows(List<ProductToImportDto> skuRows, List<ProductToImportDto> bundleRows, ProductTelemetryCounts counts)
        {
            ImportProductsResult results = new ImportProductsResult(skuRows.Count + bundleRows.Count, 0, 0);

            await ProcessSkus(skuRows, results, counts).ConfigureAwait(false);

            await ProcessBundles(bundleRows, results, counts).ConfigureAwait(false);

            return results;
        }

        /// <summary>
        /// Import each non-bundle sku into the database
        /// </summary>
        private async Task ProcessSkus(List<ProductToImportDto> rows, ImportProductsResult result, ProductTelemetryCounts counts) =>
            await PerformImport(rows, result,
                async (productVariant, row, sqlAdapter) =>
                {
                    if (productVariant.Product != null)
                    {
                        await sqlAdapter.DeleteEntityCollectionAsync(productVariant.Product.Bundles).ConfigureAwait(false);
                        productVariant.Product.Bundles.Clear();
                    }

                    CopyCsvDataToProduct(productVariant, row);
                }, counts).ConfigureAwait(false);

        /// <summary>
        /// Import each bundle sku into the database
        /// </summary>
        private async Task ProcessBundles(List<ProductToImportDto> rows, ImportProductsResult result, ProductTelemetryCounts counts) =>
            await PerformImport(rows, result,
                async (productVariant, row, sqlAdapter) =>
                {
                    CopyCsvDataToProduct(productVariant, row);

                    await ImportProductVariantBundles(productVariant, row, sqlAdapter).ConfigureAwait(false);
                }, counts)
                .ConfigureAwait(false);

        /// <summary>
        /// Perform the import of a given list of rows
        /// </summary>
        private async Task PerformImport(
            List<ProductToImportDto> rows,
            ImportProductsResult result,
            Func<ProductVariantEntity, ProductToImportDto, ISqlAdapter, Task> updateProductAction,
            ProductTelemetryCounts counts)
        {
            // Loop through each row after the header rows
            foreach (ProductToImportDto row in rows.Where(x => !x.Sku.IsNullOrWhiteSpace()))
            {
                await sqlAdapterFactory
                    .WithPhysicalTransactionAsync(sqlAdapter => ImportProduct(updateProductAction, sqlAdapter, row))
                    .Do(isNew =>
                    {
                        result.ProductSucceeded(isNew);
                        counts.AddSuccess(isNew);
                    }, ex =>
                    {
                        string message = ex.Message;
                        if ((ex.InnerException as SqlException)?.Number == 2627)
                        {
                            message = $"Bundle SKU '{row.Sku}' has the same bundled item SKU referenced more than once: '{row.BundleSkus}'.";
                        }

                        result.ProductFailed(row.Sku, message);

                        if (ex is ProductImporterException exception)
                        {
                            counts.AddFailure(exception.IsNew);
                        }
                    })
                    .Recover(ex => true)
                    .ConfigureAwait(false);

                if (itemProgressReporter.IsCancelRequested)
                {
                    break;
                }

                itemProgressReporter.PercentComplete = (int) Math.Round(100 * (((decimal) result.SuccessCount + result.FailedCount) / result.TotalCount));
            }
        }

        /// <summary>
        /// Import a single product
        /// </summary>
        private async Task<bool> ImportProduct(Func<ProductVariantEntity, ProductToImportDto, ISqlAdapter, Task> updateProductAction, ISqlAdapter sqlAdapter, ProductToImportDto row)
        {
            ProductVariantEntity productVariant = FindExistingProductVariant(sqlAdapter, row.Sku);
            var isNew = productVariant.IsNew;

            try
            {
                await sqlAdapter.DeleteEntityCollectionAsync(productVariant.Aliases).ConfigureAwait(false);
                productVariant.Aliases.Clear();

                await updateProductAction(productVariant, row, sqlAdapter).ConfigureAwait(false);

                // Set the product to be uploaded to the warehouse
                productVariant.Product.UploadToWarehouseNeeded = true;

                await sqlAdapter.SaveEntityAsync(productVariant.Product, true, true).ConfigureAwait(false);
                sqlAdapter.Commit();
                return isNew;
            }
            catch (Exception ex)
            {
                throw new ProductImporterException(ex, isNew);
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
            ValidateRow(row);

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
            productVariant.EAN = row.Ean;
            productVariant.FNSku = row.Fnsku;
            productVariant.Weight = ProductToImportDto.GetValue<decimal?>(row.Weight, nameof(row.Weight), null);
            productVariant.Length = ProductToImportDto.GetValue<decimal?>(row.Length, nameof(row.Length), null);
            productVariant.Width = ProductToImportDto.GetValue<decimal?>(row.Width, nameof(row.Width), null);
            productVariant.Height = ProductToImportDto.GetValue<decimal?>(row.Height, nameof(row.Height), null);
            productVariant.ImageUrl = row.ImageUrl;
            productVariant.BinLocation = row.WarehouseBin;
            productVariant.DeclaredValue = ProductToImportDto.GetValue<decimal?>(row.DeclaredValue, nameof(row.DeclaredValue), null);
            productVariant.CountryOfOrigin = row.CountryOfOrigin;
            productVariant.HarmonizedCode = row.HarmonizedCode;
            productVariant.IsActive = row.IsActive;

            ImportProductVariantAliases(productVariant, row.Sku, row);

            // force parent product to be active because there is nowhere in the UI where you can set this value
            productVariant.Product.IsActive = true;
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
                    .Where(s => !s.Sku.IsNullOrWhiteSpace())
                    .Select(s => new ProductVariantAliasEntity() { IsDefault = false, Sku = s.Sku, AliasName = s.Name }));
            }

            // Aliases that didn't exist that need to be created.
            newAliases
                .ForEach(a => productVariant.Aliases.Add(
                    new ProductVariantAliasEntity()
                    {
                        ProductVariant = productVariant,
                        Sku = a.Sku,
                        AliasName = a.AliasName,
                        IsDefault = a.IsDefault
                    }));
        }

        /// <summary>
        /// Import aliases
        /// </summary>
        private async Task ImportProductVariantBundles(ProductVariantEntity bundleProductVariant, ProductToImportDto productVariantDto,
            ISqlAdapter sqlAdapter)
        {
            if (productVariantDto.BundleSkuList.Any(s => productVariantDto.AliasSkuList.Any(a => a.Sku == s.Sku)))
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

            if ((await productCatalog.FetchSiblingVariants(bundleProductVariant, sqlAdapter).ConfigureAwait(false)).Any())
            {
                // If the product has siblings it cannot be a bundle
                bundleProductVariant.Product.IsBundle = false;
                throw new ProductImportException("A product with variants cannot be turned into a bundle");
            }

            // Get a list of what the final bundle list should be
            List<ProductBundleEntity> bundlesToCreate = productVariantDto.BundleSkuList
                .Where(s => !s.Sku.IsNullOrWhiteSpace())
                .Select(s =>
                {
                    ProductVariantEntity childVariant = productCatalog.FetchProductVariantEntity(sqlAdapter, s.Sku);

                    if (childVariant == null)
                    {
                        if (allSkus.None(x => s.Sku == x.Sku))
                        {
                            throw new ProductImportException($"Unable to import bundle SKU {bundleProductVariant.Aliases.First(a => a.IsDefault).Sku} because child SKU '{s.Sku}' could not be found.");
                        }

                        if (allSkus.Any(x => s.Sku == x.Sku && x.BundleSkuList.Any()))
                        {
                            throw new ProductImportException($"Unable to import bundle SKU {bundleProductVariant.Aliases.First(a => a.IsDefault).Sku} because child SKU '{s.Sku}' is a bundle.  Bundles cannot be comprised of other bundles.");
                        }

                        throw new ProductImportException($"Unable to import bundle SKU {bundleProductVariant.Aliases.First(a => a.IsDefault).Sku} because child SKU '{s.Sku}' could not be found.");
                    }

                    if (childVariant.Product.IsBundle)
                    {
                        throw new ProductImportException($"Unable to import bundle SKU {bundleProductVariant.Aliases.First(a => a.IsDefault).Sku} because child SKU '{s.Sku}' is a bundle.  Bundles cannot be comprised of other bundles.");
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

        /// <summary>
        /// Validate row field lengths
        /// </summary>
        private void ValidateRow(ProductToImportDto row)
        {
            ValidateFieldLength(row.Sku, ProductVariantAliasFields.Sku.MaxLength, "SKU");
            ValidateFieldLength(row.Name, ProductVariantFields.Name.MaxLength, "Name");
            ValidateFieldLength(row.Upc, ProductVariantFields.UPC.MaxLength, "UPC");
            ValidateFieldLength(row.Asin, ProductVariantFields.ASIN.MaxLength, "ASIN");
            ValidateFieldLength(row.Isbn, ProductVariantFields.ISBN.MaxLength, "ISBN");
            ValidateFieldLength(row.ImageUrl, ProductVariantFields.ImageUrl.MaxLength, "Image URL");
            ValidateFieldLength(row.WarehouseBin, ProductVariantFields.BinLocation.MaxLength, "Warehouse-Bin Location");
            ValidateFieldLength(row.CountryOfOrigin, ProductVariantFields.CountryOfOrigin.MaxLength, "Country of Origin");
            ValidateFieldLength(row.HarmonizedCode, ProductVariantFields.HarmonizedCode.MaxLength, "Harmonized Code");

            foreach ((string Sku, string Name) aliasSku in row?.AliasSkuList)
            {
                ValidateFieldLength(aliasSku.Name, ProductVariantAliasFields.AliasName.MaxLength, "Alias Name");
                ValidateFieldLength(aliasSku.Sku, ProductVariantAliasFields.Sku.MaxLength, "Alias SKU");
            }

            foreach (var (Sku, _) in row.BundleSkuList)
            {
                ValidateFieldLength(Sku, ProductVariantAliasFields.Sku.MaxLength, "Bundled SKU");
            }
        }

        /// <summary>
        /// Do validation on given value.  Return value if validation passed.
        /// </summary>
        private void ValidateFieldLength(string value, int maxLength, string nameOfField)
        {
            if (value?.Length > maxLength)
            {
                throw new ProductImportException($"'{nameOfField}' value is longer than max length of {maxLength}.");
            }
        }
    }
}
