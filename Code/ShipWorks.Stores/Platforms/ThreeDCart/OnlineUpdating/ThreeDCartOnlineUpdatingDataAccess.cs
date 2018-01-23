using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading order details
    /// </summary>
    [Component]
    public class ThreeDCartOnlineUpdatingDataAccess : IThreeDCartOnlineUpdatingDataAccess
    {
        private readonly IDataProvider dataProvider;
        private readonly IThreeDCartCombineOrderSearchProvider searchProvider;
        private readonly ISqlAdapter sqlAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartOnlineUpdatingDataAccess(IDataProvider dataProvider,
            IThreeDCartCombineOrderSearchProvider searchProvider,
            ISqlAdapterFactory sqlAdapterFactory)
        {
            this.searchProvider = searchProvider;
            this.dataProvider = dataProvider;
            this.sqlAdapter = sqlAdapterFactory.Create();
        }

        /// <summary>
        /// Get order details for uploading
        /// </summary>
        public async Task<IEnumerable<ThreeDCartOnlineUpdatingOrderDetail>> GetOrderDetails(long orderID)
        {
            var order = await dataProvider.GetEntityAsync<OrderEntity>(orderID).ConfigureAwait(false);
            return await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
        }

        /// <summary>
        /// Get ThreeDCartShipmentIDs for uploading
        /// </summary>
        public async Task<long> GetFirstItemShipmentIDByOriginalOrderID(long originalOrderID)
        {
            QueryFactory factory = new QueryFactory();

            var orderItems = factory.ThreeDCartOrderItem
                .InnerJoin(factory.OrderItem)
                .On(OrderItemFields.OrderItemID == ThreeDCartOrderItemFields.OrderItemID);

            var query = factory.Create()
                .From(orderItems)
                .Where(ThreeDCartOrderItemFields.OriginalOrderID == originalOrderID)
                .Select<long>(() => ThreeDCartOrderItemFields.ThreeDCartShipmentID.ToValue<long>())
                .Limit(1);

            IEnumerable<long> items = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return items.FirstOrDefault();
        }
    }
}
