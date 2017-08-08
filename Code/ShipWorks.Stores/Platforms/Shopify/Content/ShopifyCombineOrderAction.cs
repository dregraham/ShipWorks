using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

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
            var recordCreator = new SearchRecordMerger<IShopifyOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ShopifyOrderSearchFields.OrderID,
                x => new ShopifyOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ShopifyOrderID = x.ShopifyOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}