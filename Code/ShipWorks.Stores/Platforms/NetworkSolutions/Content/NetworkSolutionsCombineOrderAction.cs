using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.NetworkSolutionss.Content
{
    /// <summary>
    /// Combination action that is specific to NetworkSolutions
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.NetworkSolutions)]
    public class NetworkSolutionssCombinerAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            combinedOrder.OnlineStatusCode = orders.FirstOrDefault(o => !o.IsManual && o.OnlineStatusCode != null)?.OnlineStatusCode;
            combinedOrder.OnlineStatus = orders.FirstOrDefault(o => !o.IsManual && !string.IsNullOrWhiteSpace(o.OnlineStatus))?.OnlineStatus;

            var recordCreator = new SearchRecordMerger<INetworkSolutionsOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(NetworkSolutionsOrderSearchFields.OrderID,
                x => new NetworkSolutionsOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    NetworkSolutionsOrderID = x.NetworkSolutionsOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}
