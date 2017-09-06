using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class WalmartCombineOrderSearchProvider : CombineOrderSearchBaseProvider<WalmartCombinedIdentifier>, IWalmartCombineOrderSearchProvider
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<WalmartCombinedIdentifier>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<WalmartOrderSearchEntity>(
                    WalmartOrderSearchFields.OrderID == order.OrderID,
                    () => new WalmartCombinedIdentifier(
                        WalmartOrderSearchFields.OriginalOrderID.ToValue<long>(),
                        WalmartOrderSearchFields.PurchaseOrderID.ToValue<string>()))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override WalmartCombinedIdentifier GetOnlineOrderIdentifier(IOrderEntity order)
        {
            var walmartOrder = order as IWalmartOrderEntity;
            if (order == null)
            {
                return null;
            }

            return new WalmartCombinedIdentifier(walmartOrder.OrderID, walmartOrder.PurchaseOrderID);
        }
    }
}
