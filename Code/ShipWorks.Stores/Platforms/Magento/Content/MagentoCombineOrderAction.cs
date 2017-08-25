using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.Magento.Content
{
    /// <summary>
    /// Combination action that is specific to Magento
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Magento)]
    public class MagentoCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IMagentoOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(MagentoOrderSearchFields.OrderID,
                x => new MagentoOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    MagentoOrderID = x.MagentoOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}