using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Jet.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class JetOrderSearchProvider : CombineOrderSearchBaseProvider<string>, IJetOrderSearchProvider
    {
        public JetOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<JetOrderSearchEntity>(
                    JetOrderSearchFields.OrderID == order.OrderID,
                    JetOrderSearchFields.MerchantOrderID)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override string GetOnlineOrderIdentifier(IOrderEntity order) =>
            (order as IJetOrderEntity)?.MerchantOrderId ?? string.Empty;
    }
}
