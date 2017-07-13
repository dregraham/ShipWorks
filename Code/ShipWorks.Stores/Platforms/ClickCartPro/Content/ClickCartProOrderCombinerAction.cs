using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.ClickCartPro.Content
{
    /// <summary>
    /// Combination action that is specific to ClickCartPro
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.ClickCartPro)]
    public class ClickCartProOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<ClickCartProOrderSearchEntity> orderSearches = orders.Cast<IClickCartProOrderEntity>()
                .Select(x => new ClickCartProOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ClickCartProOrderID = x.ClickCartProOrderID
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
