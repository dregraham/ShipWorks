﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Products.Import;
using ShipWorks.Products.Warehouse;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product Catalog
    /// </summary>
    [Component]
    public class ProductCatalog : IProductCatalog
    {
        private readonly ISqlSession sqlSession;
        private readonly Func<Type, ILog> logFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IWarehouseProductClient warehouseClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCatalog(ISqlSession sqlSession, Func<Type, ILog> logFactory, IMessageHelper messageHelper, IWarehouseProductClient warehouseClient)
        {
            this.sqlSession = sqlSession;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;
            this.warehouseClient = warehouseClient;
        }

        /// <summary>
        /// Set given products activation to specified value
        /// </summary>
        public async Task SetActivation(IEnumerable<long> productIDs, bool activation)
        {
            int chunkSize = 100;
            List<IEnumerable<long>> chunks = productIDs.SplitIntoChunksOf(chunkSize).ToList();

            using (DbConnection conn = sqlSession.OpenConnection())
            {
                foreach ((IEnumerable<long> productsChunk, int index) in chunks.Select((x, i) => (x, i)))
                {
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        using (DbCommand comm = conn.CreateCommand())
                        {
                            comm.Transaction = transaction;
                            comm.CommandText = $"UPDATE ProductVariant SET IsActive = {(activation ? "1" : "0")} WHERE ProductVariantID in (SELECT item FROM @ProductVariantIDs)";
                            comm.Parameters.Add(CreateProductVariantIDParameter(productsChunk));

                            await comm.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }

                        using (DbCommand comm = conn.CreateCommand())
                        {
                            comm.Transaction = transaction;
                            comm.CommandText = $"UPDATE Product SET UploadToWarehouseNeeded = 1 WHERE ProductId IN (SELECT DISTINCT ProductId FROM ProductVariant WHERE ProductVariantID in (SELECT item FROM @ProductVariantIDs))";
                            comm.Parameters.Add(CreateProductVariantIDParameter(productsChunk));

                            await comm.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Delete any bundle items flagged for removal from a product OR
        /// remove all bundle items if the product is not a bundle
        /// </summary>
        private async Task DeleteRemovedBundleItems(ISqlAdapter adapter, ProductEntity product)
        {
            // If the product is not a bundle, remove all of its bundle items
            if (!product.IsBundle)
            {
                product.Bundles.RemovedEntitiesTracker.AddRange(product.Bundles);
            }

            // Delete the removed items
            foreach (ProductBundleEntity removedBundle in product.Bundles.RemovedEntitiesTracker)
            {
                await adapter.DeleteEntityAsync(removedBundle).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create a table parameter for the product variant ID list
        /// </summary>
        private SqlParameter CreateProductVariantIDParameter(IEnumerable<long> productVariantIDs)
        {
            DataTable table = new DataTable();
            table.Columns.Add("item", typeof(long));
            foreach (long value in productVariantIDs)
            {
                table.Rows.Add(value);
            }

            return new SqlParameter("@ProductVariantIDs", SqlDbType.Structured)
            {
                TypeName = "LongList",
                Value = table
            };
        }

        /// <summary>
        /// Fetch a Product from the database
        /// </summary>
        public IProductVariant FetchProductVariant(ISqlAdapter sqlAdapter, string sku)
        {
            IProductVariantEntity variant = FetchProductVariantEntity(sqlAdapter, sku)?.AsReadOnly();

            if (!IsActive(variant))
            {
                return new ProductVariant(sku, null, logFactory(typeof(ProductVariant)));
            }
            else if (variant?.Product.IsBundle ?? false)
            {
                return new ProductBundle(sku, variant, logFactory(typeof(ProductBundle)));
            }
            else
            {
                return new ProductVariant(sku, variant, logFactory(typeof(ProductVariant)));
            }
        }

        /// <summary>
        /// Returns true if the variant and variant product are set to active.
        /// </summary>
        private bool IsActive(IProductVariantEntity variant)
        {
            if (variant == null || !variant.IsActive || !variant.Product.IsActive)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fetch a product variant based on ProductVariantID
        /// </summary>
        public ProductVariantEntity FetchProductVariantEntity(ISqlAdapter sqlAdapter, long productVariantID) =>
            FetchFirst(ProductVariantFields.ProductVariantID == productVariantID, sqlAdapter);

        /// <summary>
        /// Fetch a product variant based on SKU
        /// </summary>
        public ProductVariantEntity FetchProductVariantEntity(ISqlAdapter sqlAdapter, string sku) =>
            FetchFirst(ProductVariantAliasFields.Sku == sku.Trim(), sqlAdapter);

        /// <summary>
        /// Fetch a product attribute based on name
        /// </summary>
        public ProductAttributeEntity FetchProductAttribute(ISqlAdapter sqlAdapter, string name, long productID)
        {
            IPredicate predicate = new FieldCompareValuePredicate(ProductAttributeFields.AttributeName, null, ComparisonOperator.Equal,
                                                                  name.ToUpper()).CaseInsensitive();

            EntityQuery<ProductAttributeEntity> query = new QueryFactory().ProductAttribute
                .Where(predicate)
                .AndWhere(ProductAttributeFields.ProductID == productID);

            ProductAttributeEntity attribute = sqlAdapter.FetchFirst(query);

            return attribute;
        }

        /// <summary>
        /// Fetch product variants based on a collection of skus
        /// </summary>
        /// <remarks>
        /// This will not populate the individual products bundles if it is a bundle product
        /// </remarks>
        public async Task<IEnumerable<ProductVariantEntity>> FetchProductVariantEntities(ISqlAdapter sqlAdapter, IEnumerable<string> skus)
        {
            if (skus.None())
            {
                return Enumerable.Empty<ProductVariantEntity>();
            }

            QueryFactory factory = new QueryFactory();

            InnerOuterJoin from = factory.ProductVariant
                .InnerJoin(factory.ProductVariantAlias)
                .On(ProductVariantFields.ProductVariantID == ProductVariantAliasFields.ProductVariantID);

            EntityQuery<ProductVariantEntity> query = factory.ProductVariant.From(from).Where(ProductVariantAliasFields.Sku.In(skus));

            foreach (IPrefetchPathElement2 path in ProductPrefetchPath.Value)
            {
                query = query.WithPath(path);
            }

            IEntityCollection2 queryResults = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return queryResults.OfType<ProductVariantEntity>().Select(v => v);
        }

        /// <summary>
        /// Get the available attributes for a variant
        /// </summary>
        public async Task<IEnumerable<IProductAttributeEntity>> GetAvailableAttributesFor(ISqlAdapter sqlAdapter, ProductVariantEntity variant)
        {
            if (variant?.Product?.IsNew ?? true)
            {
                return new IProductAttributeEntity[0];
            }

            QueryFactory factory = new QueryFactory();
            var query = factory.ProductAttribute.Where(ProductAttributeFields.ProductID == variant.ProductID);

            IEntityCollection2 queryResults = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return queryResults.OfType<IProductAttributeEntity>().Select(v => v.AsReadOnly());
        }

        /// <summary>
        /// Get the first product in the specified predicate
        /// </summary>
        private ProductVariantEntity FetchFirst(IPredicate predicate, ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();
            InnerOuterJoin from = factory.ProductVariant
                .InnerJoin(factory.ProductVariantAlias)
                .On(ProductVariantFields.ProductVariantID == ProductVariantAliasFields.ProductVariantID);

            EntityQuery<ProductVariantEntity> query = factory.ProductVariant.From(from).Where(predicate);

            foreach (IPrefetchPathElement2 path in ProductPrefetchPath.Value)
            {
                query = query.WithPath(path);
            }

            ProductVariantEntity productVariant = sqlAdapter.FetchFirst(query);

            if (productVariant?.Product?.IsBundle == true)
            {
                RelationPredicateBucket relationPredicateBucket = new RelationPredicateBucket(ProductEntity.Relations.ProductBundleEntityUsingProductID);
                relationPredicateBucket.Relations.Add(ProductBundleEntity.Relations.ProductVariantEntityUsingChildProductVariantID);
                relationPredicateBucket.Relations.Add(ProductVariantEntity.Relations.ProductVariantAliasEntityUsingProductVariantID);
                relationPredicateBucket.PredicateExpression.Add(ProductVariantAliasFields.IsDefault == true);
                relationPredicateBucket.PredicateExpression.Add(ProductBundleFields.ProductID == productVariant.ProductID);

                sqlAdapter.FetchEntityCollection(productVariant.Product.Bundles,
                    relationPredicateBucket,
                    ProductBundlePrefetchPath.Value);
            }

            return productVariant;
        }

        /// <summary>
        /// Create the pre-fetch path used to load a product variant
        /// </summary>
        private static readonly Lazy<IEnumerable<IPrefetchPathElement2>> ProductPrefetchPath = new Lazy<IEnumerable<IPrefetchPathElement2>>(() =>
        {
            List<IPrefetchPathElement2> prefetchPath = new List<IPrefetchPathElement2>
            {
                ProductVariantEntity.PrefetchPathProduct.WithSubPath(ProductEntity.PrefetchPathAttributes),
                ProductVariantEntity.PrefetchPathAliases,
                ProductVariantEntity.PrefetchPathIncludedInBundles,
                ProductVariantEntity.PrefetchPathAttributeValues.WithSubPath(ProductVariantAttributeValueEntity.PrefetchPathProductAttribute)
            };

            return prefetchPath;
        });

        /// <summary>
        /// Prefetch to include aliases and product with bundled variant
        /// </summary>
        private static readonly Lazy<IPrefetchPath2> ProductBundlePrefetchPath = new Lazy<IPrefetchPath2>(() =>
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.ProductBundleEntity);
            IPrefetchPath2 subPath = prefetchPath.Add(ProductBundleEntity.PrefetchPathChildVariant).SubPath;
            subPath.Add(ProductVariantEntity.PrefetchPathAliases);
            subPath.Add(ProductVariantEntity.PrefetchPathProduct);

            return prefetchPath;
        });

        /// <summary>
        /// Save the product
        /// </summary>
        public async Task<Result> Save(ProductVariantEntity productVariant, ISqlAdapterFactory sqlAdapterFactory)
        {
            Result validationResult = await Validate(productVariant, sqlAdapterFactory).ConfigureAwait(false);
            if (validationResult.Failure)
            {
                return validationResult;
            }

            ProductEntity product = productVariant.Product;
            DateTime now = DateTime.UtcNow;

            if (product.IsNew)
            {
                product.CreatedDate = now;
            }

            if (productVariant.IsNew)
            {
                productVariant.CreatedDate = now;

                return await warehouseClient.AddProduct(productVariant)
                    .Bind(x => SaveProductToDatabase(productVariant, sqlAdapterFactory))
                    .ToResult()
                    .ConfigureAwait(false);
            }

            return await SaveProductToDatabase(productVariant, sqlAdapterFactory)
                .ToResult()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Save the product to the database
        /// </summary>
        private async Task<Unit> SaveProductToDatabase(ProductVariantEntity productVariant, ISqlAdapterFactory sqlAdapterFactory)
        {
            ProductEntity product = productVariant.Product;

            // Set the product to be uploaded to the warehouse
            product.UploadToWarehouseNeeded = true;

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
            {
                try
                {
                    if (product.IsBundle)
                    {
                        foreach (var bundle in productVariant.IncludedInBundles)
                        {
                            await sqlAdapter.DeleteEntityAsync(bundle).ConfigureAwait(true);
                        }
                    }

                    await DeleteRemovedBundleItems(sqlAdapter, product).ConfigureAwait(true);
                    await DeleteRemovedVariantAttributeValues(sqlAdapter, productVariant).ConfigureAwait(true);

                    sqlAdapter.SaveEntity(product);

                    await DeleteUnusedAttributes(sqlAdapter).ConfigureAwait(false);

                    sqlAdapter.Commit();
                }
                catch (ORMQueryExecutionException)
                {
                    sqlAdapter.Rollback();
                    throw;
                }
            }

            return Unit.Default;
        }

        /// <summary>
        /// Delete removed variant attribute values
        /// </summary>
        private async Task DeleteRemovedVariantAttributeValues(ISqlAdapter sqlAdapter, ProductVariantEntity productVariant)
        {
            foreach (ProductVariantAttributeValueEntity removedAttribute in productVariant.AttributeValues.RemovedEntitiesTracker)
            {
                await sqlAdapter.DeleteEntityAsync(removedAttribute).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Delete unused attributes (this deletes all unused attributes, not just for this product)
        /// </summary>
        private async Task DeleteUnusedAttributes(ISqlAdapter sqlAdapter)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.Relations.Add(ProductAttributeEntity.Relations.ProductVariantAttributeValueEntityUsingProductAttributeID, JoinHint.Left);
            bucket.PredicateExpression.Add(ProductVariantAttributeValueFields.ProductVariantAttributeValueID.IsNull());

            await sqlAdapter.DeleteEntitiesDirectlyAsync(typeof(ProductAttributeEntity), bucket).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks if product is valid
        /// </summary>
        private async Task<Result> Validate(ProductVariantEntity productVariant, ISqlAdapterFactory sqlAdapterFactory)
        {
            if (productVariant.Aliases.Any(a => string.IsNullOrWhiteSpace(a.Sku)))
            {
                string message = $"The following field is required: {Environment.NewLine}SKU";
                return Result.FromError(message);
            }

            if (productVariant.Product.IsBundle)
            {
                // A Bundle can't have siblings
                IEnumerable<IProductVariantEntity> siblingVariants;
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    siblingVariants = await FetchSiblingVariants(productVariant, sqlAdapter);
                }
                if (siblingVariants.Any())
                {
                    return Result.FromError("A product with variants cannot be turned into a bundle");
                }

                // A Bundle can't be in another bundle
                int inHowManyBundles = productVariant.IncludedInBundles.Count();
                if (inHowManyBundles > 0)
                {
                    string plural = inHowManyBundles > 1 ? "s" : "";
                    string question = $"A bundle cannot contain another bundle.\r\n\r{productVariant.DefaultSku ?? "This Product"} is already a part of {inHowManyBundles} existing bundle{plural}.\r\n\r\nDo you want to remove {productVariant.DefaultSku ?? "this product"} from the existing bundle{plural}? ";

                    DialogResult answer = messageHelper.ShowQuestion(question);
                    if (answer != DialogResult.OK)
                    {
                        return Result.FromError("A Bundle cannot contain another bundle");
                    }
                }
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Get a DataTable of products from the database
        /// </summary>
        public async Task<DataTable> GetProductDataForExport()
        {
            string exportProductsSql = string.Empty;

            using (Stream stream = GetType().Assembly.GetManifestResourceStream("ShipWorks.Products.Export.ExportProducts.sql"))
            {
                if (stream == null)
                {
                    throw new ProductImportException($"Unable to load ExportProducts SQL.");
                }

                // Return the contents
                using (StreamReader reader = new StreamReader(stream))
                {
                    exportProductsSql = reader.ReadToEnd();
                }
            }

            DataTable dt = new DataTable("Products");
            using (DbConnection conn = sqlSession.OpenConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = exportProductsSql;

                    try
                    {
                        using (DbDataReader reader = await comm.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new ProductImportException($"Unable to select products for export from the database.", ex);
                    }
                }
            }

            AddSecondHeaderRow(dt);

            return dt;
        }

        /// <summary>
        /// Add the second header row.
        /// </summary>
        private static void AddSecondHeaderRow(DataTable dt)
        {
            DataRow secondHeaderRow = dt.NewRow();
            dt.Columns[dt.Columns["Name"].Ordinal].AllowDBNull = true;
            dt.Columns[dt.Columns["Active"].Ordinal].AllowDBNull = true;
            secondHeaderRow[0] = "";
            secondHeaderRow[1] = "SKU : Name | SKU : Name";
            secondHeaderRow[2] = "SKU : Qty | SKU : Qty";
            secondHeaderRow[7] = "lbs";
            secondHeaderRow[8] = "inches";
            secondHeaderRow[9] = "inches";
            secondHeaderRow[10] = "inches";
            dt.Rows.InsertAt(secondHeaderRow, 0);
        }

        /// <summary>
        /// Fetch product variants to upload to the warehouse.
        /// </summary>
        public async Task<IEnumerable<IProductVariantEntity>> FetchProductVariantsForUploadToWarehouse(ISqlAdapter sqlAdapter, int pageSize)
        {
            QueryFactory factory = new QueryFactory();
            InnerOuterJoin from = factory.ProductVariant
                .InnerJoin(factory.Product)
                .On(ProductFields.ProductID == ProductVariantFields.ProductID);

            EntityQuery<ProductVariantEntity> query = factory.ProductVariant
                .From(from)
                .Where(ProductFields.UploadToWarehouseNeeded == true)
                .Limit(pageSize);

            foreach (IPrefetchPathElement2 path in ProductPrefetchPath.Value)
            {
                query = query.WithPath(path);
            }

            IEntityCollection2 queryResults = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return queryResults.OfType<ProductVariantEntity>();
        }

        /// <summary>
        /// Fetch count of product variants to upload to the warehouse.
        /// </summary>
        public async Task<int> FetchProductVariantsForUploadToWarehouseCount(ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();
            InnerOuterJoin from = factory.ProductVariant
                .InnerJoin(factory.Product)
                .On(ProductFields.ProductID == ProductVariantFields.ProductID);

            var query = factory.ProductVariant
                .Select(Functions.CountRow())
                .From(from)
                .Where(ProductFields.UploadToWarehouseNeeded == true);

            return await sqlAdapter.FetchScalarAsync<int>(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Reset the NeedsUploadToWarehouse flag for the given variants
        /// </summary>
        public async Task<int> ResetNeedsWarehouseUploadFlag(ISqlAdapter sqlAdapter, IEnumerable<IProductVariantEntity> variants)
        {
            var productIds = variants.Select(x => x.ProductID).Distinct();
            var updateTemplate = new ProductEntity { UploadToWarehouseNeeded = false };
            var predicateBucket = new RelationPredicateBucket(ProductFields.ProductID.In(productIds));

            return await sqlAdapter.UpdateEntitiesDirectlyAsync(updateTemplate, predicateBucket);
        }

        /// <summary>
        /// Fetch variants of the same product as the passed in variant.
        /// </summary>
        public async Task<IEnumerable<IProductVariantEntity>> FetchSiblingVariants(IProductVariantEntity productVariant, ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<ProductVariantEntity> query = factory.ProductVariant.Where(ProductVariantFields.ProductID == productVariant.ProductID)
                .AndWhere(ProductVariantFields.ProductVariantID != productVariant.ProductVariantID);

            IEntityCollection2 queryResults = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return queryResults.OfType<ProductVariantEntity>().Select(v => v.AsReadOnly());
        }

        /// <summary>
        /// Create a variant based on the given variant
        /// </summary>
        public GenericResult<ProductVariantEntity> CloneVariant(ProductVariantEntity productVariant)
        {
            if (productVariant.Product.IsBundle)
            {
                return GenericResult.FromError<ProductVariantEntity>("You cannot create a variant from a bundle.");
            }

            ProductVariantEntity clone = EntityUtility.CloneAsNew(productVariant);
            clone.ProductVariantID = 0;

            return clone;
        }
    }
}