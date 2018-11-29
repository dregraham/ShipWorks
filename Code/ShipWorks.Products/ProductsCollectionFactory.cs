using System;
using System.ComponentModel;
using System.Linq;
using DataVirtualization;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
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
    /// Factory for creating product collections
    /// </summary>
    [Component]
    public class ProductsCollectionFactory : IProductsCollectionFactory
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsCollectionFactory(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Create a collection of products
        /// </summary>
        public DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Create(bool includeInactiveProducts, string searchText, IBasicSortDefinition sortDefinition)
        {
            searchText = searchText?.Trim();

            var listWrapper = new DataWrapper<IVirtualizingCollection<IProductListItemEntity>>(0, 0, () => { });
            QueryFactory factory = new QueryFactory();
            var from = factory.ProductVariant
                .InnerJoin(factory.ProductVariantAlias)
                .On(ProductVariantFields.ProductVariantID == ProductVariantAliasFields.ProductVariantID);

            var query = new QueryFactory().ProductVariant
                .From(from)
                .Select(() => ProductVariantFields.ProductVariantID.ToValue<long>())
                .Distinct()
                .OrderBy(CreateSortClause(sortDefinition));

            if (!includeInactiveProducts)
            {
                query = query.Where(ProductVariantFields.IsActive == true);
            }

            if (!searchText.IsNullOrWhiteSpace())
            {
                query = query.Where(ProductVariantFields.Name.StartsWith(searchText) |
                                    ProductVariantFields.ASIN.StartsWith(searchText) |
                                    ProductVariantFields.UPC.StartsWith(searchText) |
                                    ProductVariantAliasFields.Sku.StartsWith(searchText));
            }

            Functional.UsingAsync(
                    sqlAdapterFactory.Create(),
                    sqlAdapter => sqlAdapter.FetchQueryAsync(query)
                .ContinueWith(t => listWrapper.Data = new VirtualizingCollection<IProductListItemEntity>(new ProductItemsProvider(sqlAdapterFactory, t.Result))));

            return listWrapper;
        }

        /// <summary>
        /// Create a sort clause from the given sort definition
        /// </summary>
        private ISortClause CreateSortClause(IBasicSortDefinition sortDefinition)
        {
            IEntityField2 sortField;

            if (sortDefinition.Name.Contains("sku", StringComparison.InvariantCultureIgnoreCase))
            {
                sortField = ProductVariantAliasFields.Sku;
            }
            else
            {
                sortField = new ProductVariantEntity().Fields
                    .OfType<IEntityField2>()
                    .Single(x => sortDefinition.Name.EndsWith(x.Name, StringComparison.Ordinal));
            }

            return sortDefinition.Direction == ListSortDirection.Ascending ?
                sortField.Ascending() :
                sortField.Descending();
        }
    }
}
