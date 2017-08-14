using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Combined order search provider for ClickCartProOrderID
    /// </summary>
    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.ClickCartPro)]
    public class ClickCartProCombineOrderNumberCompleteSearchProvider : CombineOrderNumberCompleteSearchProvider
    {
        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers(order as OrderEntity, 
                "ClickCartProOrderSearch",
                ClickCartProOrderSearchFields.OrderID == order.OrderID, 
                () => ClickCartProOrderSearchFields.ClickCartProOrderID.ToValue<string>()).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the ClickCartPro online order identifier
        /// </summary>
        protected override string GetOnlineOrderIdentifier(IOrderEntity order)
        {
            return ((IClickCartProOrderEntity) order).ClickCartProOrderID;
        }
    }
}
