using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataVirtualization;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Products
{
    /// <summary>
    /// Product items provider
    /// </summary>
    public class ProductItemsProvider : IItemsProvider<IProductListItemEntity>
    {
        private readonly List<long> productIDs;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductItemsProvider(ISqlAdapterFactory sqlAdapterFactory, List<long> productIDs)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.productIDs = productIDs;
        }

        /// <summary>
        /// Fetch the count of products
        /// </summary>
        public int FetchCount() => productIDs.Count;

        /// <summary>
        /// Fetch a range of products
        /// </summary>
        public async Task<IList<IProductListItemEntity>> FetchRange(int startIndex, int pageCount)
        {
            var ids = productIDs.Skip(startIndex).Take(pageCount).ToList();
            var query = new QueryFactory().ProductListItem.Where(ProductListItemFields.ProductVariantID.In(ids));

            var list = await Functional.UsingAsync(
                    sqlAdapterFactory.Create(),
                    sqlAdapter => sqlAdapter.FetchQueryAsync(query))
                .ConfigureAwait(true);

            return ids.LeftJoin(list.OfType<IProductListItemEntity>(), x => x, x => x.ProductVariantID).Select(x => x.Right).ToList();
        }
    }
}