using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.Content
{
    /// <summary>
    /// Combination action that is specific to MarketplaceAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.MarketplaceAdvisor)]
    public class MarketplaceAdvisorCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IMarketplaceAdvisorOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(MarketplaceAdvisorOrderSearchFields.OrderID,
                x => new MarketplaceAdvisorOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    SellerOrderNumber = x.SellerOrderNumber,
                    InvoiceNumber = x.InvoiceNumber
                });
        }
    }
}