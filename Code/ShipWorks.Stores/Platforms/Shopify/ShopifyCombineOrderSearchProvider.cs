using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Constructor
    /// </summary>
    [KeyedComponent(typeof(ICombineOrderSearchProvider<ShopifyOrderSearchEntity>), StoreTypeCode.Shopify)]
    public class ShopifyCombineOrderSearchProvider : CombineOrderSearchBaseProvider<ShopifyOrderSearchEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<ShopifyOrderSearchEntity>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<ShopifyOrderSearchEntity>(ShopifyOrderSearchFields.OrderID == order.OrderID)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the Shopify online order identifier
        /// </summary>
        protected override ShopifyOrderSearchEntity GetOnlineOrderIdentifier(IOrderEntity order)
        {
            IShopifyOrderEntity ShopifyOrder = (IShopifyOrderEntity) order;
            return new ShopifyOrderSearchEntity
            {
                OrderID = order.OrderID,
                OriginalOrderID = ShopifyOrder.OrderID,
                ShopifyOrderID = ShopifyOrder.ShopifyOrderID
            };
        }
    }
}
