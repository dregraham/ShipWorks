using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class ThreeDCartCombineOrderSearchProvider : CombineOrderSearchBaseProvider<ThreeDCartOnlineUpdatingOrderDetail>, IThreeDCartCombineOrderSearchProvider
    {
        readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<ThreeDCartOnlineUpdatingOrderDetail>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.ThreeDCartOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(ThreeDCartOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new ThreeDCartOnlineUpdatingOrderDetail
                {
                    OrderNumber = OrderSearchFields.OrderNumber.ToValue<long>(),
                    OrderNumberComplete = OrderSearchFields.OrderNumberComplete.ToValue<string>(),
                    ThreeDCartOrderID = ThreeDCartOrderSearchFields.ThreeDCartOrderID.ToValue<long>(),
                    IsManual = OrderSearchFields.IsManual.ToValue<bool>(),
                    OriginalOrderID = ThreeDCartOrderSearchFields.OriginalOrderID.ToValue<long>(),
                })
                .Where(ThreeDCartOrderSearchFields.OrderID == order.OrderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override ThreeDCartOnlineUpdatingOrderDetail GetOnlineOrderIdentifier(IOrderEntity order)
        {
            IThreeDCartOrderEntity typedOrder = order as IThreeDCartOrderEntity;

            return new ThreeDCartOnlineUpdatingOrderDetail
            {
                OrderNumber = order.OrderNumber,
                OrderNumberComplete = order.OrderNumberComplete,
                ThreeDCartOrderID = typedOrder != null ? typedOrder.ThreeDCartOrderID : 0,
                IsManual = order.IsManual,
                OriginalOrderID = order.OrderID
            };
        }
    }
}
