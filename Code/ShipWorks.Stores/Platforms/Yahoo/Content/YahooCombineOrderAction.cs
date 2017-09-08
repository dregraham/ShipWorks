using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Yahoo.Content
{
    /// <summary>
    /// Combination action that is specific to Yahoo
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Yahoo)]
    public class YahooCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            YahooOrderEntity order = (YahooOrderEntity) combinedOrder;

            if (string.IsNullOrWhiteSpace(order.YahooOrderID))
            {
                order.YahooOrderID = orders.Where(o => o is YahooOrderEntity).Cast<YahooOrderEntity>().FirstOrDefault()?.YahooOrderID;
            }

            var recordCreator = new SearchRecordMerger<IYahooOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(YahooOrderSearchFields.OrderID,
                x => new YahooOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    YahooOrderID = x.YahooOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}