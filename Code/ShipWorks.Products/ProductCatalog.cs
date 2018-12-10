﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCatalog(ISqlSession sqlSession, Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            this.sqlSession = sqlSession;
            this.logFactory = logFactory;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Set given products activation to specified value
        /// </summary>
        public async Task SetActivation(IEnumerable<long> productIDs, bool activation, IProgressReporter progressReporter)
        {
            int chunkSize = 100;
            List<IEnumerable<long>> chunks = productIDs.SplitIntoChunksOf(chunkSize).ToList();
            int chunkCount = chunks.Count();

            progressReporter.PercentComplete = 0;

            using (DbConnection conn = sqlSession.OpenConnection())
            {
                foreach ((IEnumerable<long> productsChunk, int index) in chunks.Select((x, i) => (x, i)))
                {
                    using (DbCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = $"UPDATE ProductVariant SET IsActive = {(activation ? "1" : "0")} WHERE ProductVariantID in (SELECT item FROM @ProductVariantIDs)";
                        comm.Parameters.Add(CreateProductVariantIDParameter(productsChunk));

                        await comm.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }

                    if (progressReporter.IsCancelRequested)
                    {
                        break;
                    }

                    progressReporter.PercentComplete = (int) Math.Round(100 * ((index + 1M) / chunkCount));
                }
            }
        }

        /// <summary>
        /// Number of bundles productID exists in.
        /// </summary>
        private async Task<int> InBundleCount(long productVariantID)
        {
            using (DbConnection conn = sqlSession.OpenConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    comm.CommandText = $"SELECT COUNT(*) FROM ProductBundle WHERE ChildProductVariantID = @ProductID";
                    comm.Parameters.Add(new SqlParameter("@ProductID", productVariantID));
                    return (int) await comm.ExecuteScalarAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Remove variant from all bundles
        /// </summary>
        private Task RemoveFromAllBundles(ISqlAdapter adapter, long productVariantID)
        {
            return adapter.ExecuteSQLAsync("DELETE ProductBundle WHERE ChildProductVariantID = @p0",
                new object[] { productVariantID });
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
                await adapter.DeleteEntityAsync(removedBundle);
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

            return variant?.Product.IsBundle ?? false ?
                new ProductBundle(sku, variant, logFactory(typeof(ProductBundle))) :
                new ProductVariant(sku, variant, logFactory(typeof(ProductVariant)));
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
                ProductVariantEntity.PrefetchPathProduct,
                ProductVariantEntity.PrefetchPathAliases
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
        public async Task<Result> Save(ProductEntity product, ISqlAdapterFactory sqlAdapterFactory)
        {
            Result validationResult = await Validate(product);
            if (validationResult.Failure)
            {
                return validationResult;
            }

            if (product.IsNew)
            {
                product.CreatedDate = DateTime.UtcNow;
            }

            product.Variants.Where(v => v.IsNew)?.ForEach(v => v.CreatedDate = DateTime.UtcNow);

            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                try
                {
                    if (product.IsBundle)
                    {
                        await RemoveFromAllBundles(adapter, product.Variants.FirstOrDefault().ProductVariantID).ConfigureAwait(true);
                    }

                    await DeleteRemovedBundleItems(adapter, product).ConfigureAwait(true);

                    adapter.SaveEntity(product);

                    adapter.Commit();
                }
                catch (ORMQueryExecutionException ex)
                {
                    adapter.Rollback();
                    return Result.FromError(ex);
                }
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Checks if product is valid
        /// </summary>
        private async Task<Result> Validate(ProductEntity product)
        {
            var productVariant = product.Variants.FirstOrDefault();

            if (productVariant.Aliases.Any(a => string.IsNullOrWhiteSpace(a.Sku)))
            {
                string message = $"The following field is required: {Environment.NewLine}SKU";

                messageHelper.ShowError(message);
                return Result.FromError(message);
            }

            if (product.IsBundle)
            {
                int inHowManyBundles = await InBundleCount(productVariant.ProductVariantID).ConfigureAwait(true);
                if (inHowManyBundles > 0)
                {
                    string plural = inHowManyBundles > 1 ? "s" : "";
                    string question = $"A bundle cannot contain another bundle.\r\n\r{productVariant.DefaultSku ?? "This Product"} is already a part of {inHowManyBundles} existing bundle{plural}.\r\n\r\nDo you want to remove {productVariant.DefaultSku ?? "this product"} from the existing bundle{plural}? ";

                    DialogResult answer = messageHelper.ShowQuestion(question);
                    if (answer != DialogResult.OK)
                    {
                        return Result.FromError("");
                    }
                }
            }

            return Result.FromSuccess();
        }
    }
}