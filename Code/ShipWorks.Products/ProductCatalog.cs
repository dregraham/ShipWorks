using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
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
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly ISqlSession sqlSession;
        private readonly ILog productVariantLog;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCatalog(ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> logFactory, ISqlSession sqlSession)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            productVariantLog = logFactory(typeof(ProductVariant));
            this.sqlSession = sqlSession;
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

            using (var conn = sqlSession.OpenConnection())
            {
                foreach (var (productsChunk, index) in chunks.Select((x, i) => (x, i)))
                {
                    using (var comm = conn.CreateCommand())
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
        /// Create a table parameter for the product variant ID list
        /// </summary>
        private SqlParameter CreateProductVariantIDParameter(IEnumerable<long> productVariantIDs)
        {
            var table = new DataTable();
            table.Columns.Add("item", typeof(long));
            foreach (var value in productVariantIDs)
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
        public IProductVariant FetchProductVariant(string sku) => new ProductVariant(sku, FetchProductVariantReadOnly(sku), productVariantLog);

        /// <summary>
        /// Fetch a product variant based on SKU
        /// </summary>
        private IProductVariantEntity FetchProductVariantReadOnly(string sku)
        {
            ProductVariantEntity productVariant;

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                productVariant = Load(sku, sqlAdapter, ProductPrefetchPath.Value);
            }

            return productVariant?.AsReadOnly();
        }

        /// <summary>
        /// Get product with the data specified in the prefetch path loaded
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        private ProductVariantEntity Load(string sku, ISqlAdapter sqlAdapter, IEnumerable<IPrefetchPathElement2> prefetchPaths)
        {
            return FetchFirst(ProductVariantAliasFields.Sku == sku.Trim(),
                sqlAdapter,
                prefetchPaths);
        }

        /// <summary>
        /// Get the first product in the specified predicate
        /// </summary>
        private ProductVariantEntity FetchFirst(IPredicate predicate, ISqlAdapter sqlAdapter, IEnumerable<IPrefetchPathElement2> prefetchPaths)
        {
            QueryFactory factory = new QueryFactory();
            var from = factory.ProductVariant
                .InnerJoin(factory.ProductVariantAlias)
                .On(ProductVariantFields.ProductVariantID == ProductVariantAliasFields.ProductVariantID);

            EntityQuery<ProductVariantEntity> query = factory.ProductVariant.From(from).Where(predicate);

            foreach (IPrefetchPathElement2 path in prefetchPaths)
            {
                query = query.WithPath(path);
            }

            return sqlAdapter.FetchFirst(query);
        }

        /// <summary>
        /// Create the pre-fetch path used to load a product variant
        /// </summary>
        private static readonly Lazy<IEnumerable<IPrefetchPathElement2>> ProductPrefetchPath = new Lazy<IEnumerable<IPrefetchPathElement2>>(() =>
        {
            List<IPrefetchPathElement2> prefetchPath = new List<IPrefetchPathElement2>();

            prefetchPath.Add(ProductVariantEntity.PrefetchPathProductVariantAlias);

            return prefetchPath;
        });
    }
}
