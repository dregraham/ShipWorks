using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombinedOrderSearchProviders
{
    /// <summary>
    /// Combined order search provider for order number complete
    /// </summary>
    [Component(RegistrationType.ImplementedInterfaces | RegistrationType.Self)]
    public class CombineOrderNumberCompleteSearchProvider : CombineOrderSearchBaseProvider<string>, ICombineOrderNumberCompleteSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderNumberCompleteSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<OrderSearchEntity>(
                OrderSearchFields.OrderID == order.OrderID & OrderSearchFields.IsManual == false,
                OrderSearchFields.OrderNumberComplete).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override string GetOnlineOrderIdentifier(IOrderEntity order) => order.OrderNumberComplete;
    }
}
