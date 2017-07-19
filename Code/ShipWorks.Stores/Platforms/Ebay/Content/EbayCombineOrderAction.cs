using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.Ebay.Content
{
    /// <summary>
    /// Combination action that is specific to Ebay
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Ebay)]
    public class EbayCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IEbayOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(EbayOrderSearchFields.OrderID,
                x => new EbayOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    EbayOrderID = x.EbayOrderID,
                    EbayBuyerID = x.EbayBuyerID
                });
        }
    }
}