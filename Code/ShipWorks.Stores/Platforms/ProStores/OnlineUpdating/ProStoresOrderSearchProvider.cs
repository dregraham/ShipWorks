using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.ProStores.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class ProStoresCombineOrderSearchProvider : CombineOrderSearchBaseProvider<OrderUploadDetails>, IProStoresCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<OrderUploadDetails>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<OrderSearchEntity>(
                    OrderSearchFields.OrderID == order.OrderID,
                    () => new OrderUploadDetails(
                        OrderSearchFields.OrderNumber.ToValue<long>(),
                        OrderSearchFields.IsManual.ToValue<bool>()))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override OrderUploadDetails GetOnlineOrderIdentifier(IOrderEntity order) =>
            new OrderUploadDetails(order.OrderNumber, order.IsManual);
    }
}
