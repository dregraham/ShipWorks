using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;
using ShipWorks.Stores.Platforms.Shopify.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Combined order search provider for Shopify
    /// </summary>
    [Component]
    public class ShopifyCombineOrderSearchProvider : CombineOrderSearchBaseProvider<long>, IShopifyOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override async Task<IEnumerable<long>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.ShopifyOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(ShopifyOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => ShopifyOrderSearchFields.ShopifyOrderID.ToValue<long>())
                .Distinct()
                .Where(ShopifyOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the Shopify online order identifier
        /// </summary>
        protected override long GetOnlineOrderIdentifier(IOrderEntity order) =>
            (order as IShopifyOrderEntity)?.ShopifyOrderID ?? 0;
    }
}
