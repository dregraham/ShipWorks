using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class NeweggCombineOrderSearchProvider : CombineOrderSearchBaseProvider<OrderUploadDetail>, INeweggCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NeweggCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override Task<IEnumerable<OrderUploadDetail>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return GetCombinedOnlineOrderIdentifiers<OrderSearchEntity>(
                OrderSearchFields.OrderID == order.OrderID,
                () => new OrderUploadDetail(
                    OrderSearchFields.OriginalOrderID.ToValue<long>(),
                    OrderSearchFields.OrderNumber.ToValue<long>(),
                    OrderSearchFields.IsManual.ToValue<bool>()));
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override OrderUploadDetail GetOnlineOrderIdentifier(IOrderEntity order) =>
            new OrderUploadDetail(order.OrderID, order.OrderNumber, order.IsManual);
    }
}
