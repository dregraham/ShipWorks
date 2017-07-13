using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Content
{
    /// <summary>
    /// Combination action that is specific to ThreeDCart
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<ThreeDCartOrderSearchEntity> orderSearches = orders.Cast<IThreeDCartOrderEntity>()
                .Select(x => new ThreeDCartOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ThreeDCartOrderID = x.ThreeDCartOrderID
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}