using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombinedOrderSearchProviders
{
    /// <summary>
    /// Combined order search provider for order number complete
    /// </summary>
    [Component]
    public class CombineOrderNumberSearchProvider : CombineOrderSearchBaseProvider<long>, ICombineOrderNumberSearchProvider
    {
        /// <summary>
        /// Gets the online store's order identifier for orders that are not manual
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<long>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers(order as OrderEntity, "OrderSearch",
                OrderSearchFields.OrderID == order.OrderID & OrderSearchFields.IsManual == false,
                () => OrderSearchFields.OrderNumber.ToValue<long>()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override long GetOnlineOrderIdentifier(IOrderEntity order) => order.OrderNumber;
    }
}
