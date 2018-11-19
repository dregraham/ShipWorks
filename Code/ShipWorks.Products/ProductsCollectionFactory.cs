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
        public DataWrapper<IVirtualizingCollection<IProductListItemEntity>> Create(bool includeInactiveProducts, IBasicSortDefinition sortDefinition)
        {
            var listWrapper = new DataWrapper<IVirtualizingCollection<IProductListItemEntity>>(0);
            var query = new QueryFactory().ProductListItem
                .Select(() => ProductListItemFields.ProductVariantID.ToValue<long>())
                .OrderBy(CreateSortClause(sortDefinition));

            if (!includeInactiveProducts)
            {
                query = query.Where(ProductListItemFields.IsActive == true);
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
            var sortField = new ProductListItemEntity().Fields
                .OfType<IEntityField2>()
                .Single(x => sortDefinition.Name.EndsWith(x.Name, StringComparison.Ordinal));

            return sortDefinition.Direction == ListSortDirection.Ascending ?
                sortField.Ascending() :
                sortField.Descending();
        }
    }
}
