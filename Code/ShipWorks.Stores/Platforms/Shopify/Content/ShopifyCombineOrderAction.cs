using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Shopify.Content
{
    /// <summary>
    /// Combination action that is specific to Shopify
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Shopify)]
    public class ShopifyCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            ShopifyOrderEntity shopifyCombinedOrder = combinedOrder as ShopifyOrderEntity;

            var recordCreator = new SearchRecordMerger<IShopifyOrderEntity>(shopifyCombinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ShopifyOrderSearchFields.OrderID, x => CreateShopifyOrderSearchEntry(x, shopifyCombinedOrder.OrderID));
        }

        /// <summary>
        /// Create a ShopifyOrderSearchEntity for the given Shopify order and combined order id
        /// </summary>
        private ShopifyOrderSearchEntity CreateShopifyOrderSearchEntry(IShopifyOrderEntity shopifyOrder, long orderID)
        {
            return new ShopifyOrderSearchEntity
            {
                OrderID = orderID,
                ShopifyOrderID = shopifyOrder.ShopifyOrderID,
                OriginalOrderID = shopifyOrder.OrderID
            };
        }
    }
}