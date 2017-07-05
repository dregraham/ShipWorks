using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.Magento.Content
{
    /// <summary>
    /// Combination action that is specific to Magento
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.Magento)]
    public class MagentoOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<MagentoOrderSearchEntity> orderSearches = orders.Cast<IMagentoOrderEntity>()
                .Select(x => new MagentoOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    StoreID = x.StoreID,
                    OrderNumber = x.OrderNumber,
                    OrderNumberComplete = x.OrderNumberComplete,
                    MagentoOrderID = x.MagentoOrderID
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
