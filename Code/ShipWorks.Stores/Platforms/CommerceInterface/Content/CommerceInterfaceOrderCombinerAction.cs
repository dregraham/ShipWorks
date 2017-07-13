using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.CommerceInterface.Content
{
    /// <summary>
    /// Combination action that is specific to CommerceInterface
    /// </summary>
    public class CommerceInterfaceOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<CommerceInterfaceOrderSearchEntity> orderSearches = orders.Cast<ICommerceInterfaceOrderEntity>()
                .Select(x => new CommerceInterfaceOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    CommerceInterfaceOrderNumber = x.CommerceInterfaceOrderNumber
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
