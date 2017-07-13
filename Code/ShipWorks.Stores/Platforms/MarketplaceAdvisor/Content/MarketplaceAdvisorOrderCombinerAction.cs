using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.Content
{
    /// <summary>
    /// Combination action that is specific to MarketplaceAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.MarketplaceAdvisor)]
    public class MarketplaceAdvisorOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<MarketplaceAdvisorOrderSearchEntity> orderSearches = orders.Cast<IMarketplaceAdvisorOrderEntity>()
                .Select(x => new MarketplaceAdvisorOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    SellerOrderNumber = x.SellerOrderNumber,
                    InvoiceNumber = x.InvoiceNumber
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
