using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCatalog(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }
        
        /// <summary>
        /// Fetch a Product from the database
        /// </summary>
        public IProductVariant FetchProductVariant(string sku) => new ProductVariant(sku, FetchProductVariantReadOnly(sku));

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
            return FetchFirst(ProductVariantAliasFields.Sku == sku,
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
        private static readonly Lazy<IEnumerable<IPrefetchPathElement2>>ProductPrefetchPath = new Lazy<IEnumerable<IPrefetchPathElement2>>(() =>
        {
            List<IPrefetchPathElement2> prefetchPath = new List<IPrefetchPathElement2>();

            prefetchPath.Add(ProductVariantEntity.PrefetchPathProductVariantAlias);

            return prefetchPath;
        });
    }
}
