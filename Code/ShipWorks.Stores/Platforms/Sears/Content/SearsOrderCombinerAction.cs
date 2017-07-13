using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.Sears.Content
{
    /// <summary>
    /// Combination action that is specific to Sears
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.Sears)]
    public class SearsOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<SearsOrderSearchEntity> orderSearches = orders.Cast<ISearsOrderEntity>()
                .Select(x => new SearsOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    PoNumber = x.PoNumber
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
