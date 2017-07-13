using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.PayPal.Content
{/// <summary>
    /// Combination action that is specific to PayPal
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.PayPal)]
    public class PayPalOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<PayPalOrderSearchEntity> orderSearches = orders.Cast<IPayPalOrderEntity>()
                .Select(x => new PayPalOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    //CustomOrderIdentifier = x.CustomOrderIdentifier
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
