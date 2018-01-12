using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class OrderMotionCombineOrderSearchProvider : CombineOrderSearchBaseProvider<OrderDetail>, IOrderMotionCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<OrderDetail>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.OrderMotionOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(OrderMotionOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new OrderDetail(
                    OrderSearchFields.OrderNumber.ToValue<long>(),
                    OrderMotionOrderSearchFields.OrderMotionShipmentID.ToValue<long>(),
                    OrderSearchFields.IsManual.ToValue<bool>()))
                .Distinct()
                .Where(OrderMotionOrderSearchFields.OrderID == order.OrderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override OrderDetail GetOnlineOrderIdentifier(IOrderEntity order) =>
            new OrderDetail(order.OrderNumber, ((IOrderMotionOrderEntity) order).OrderMotionShipmentID, order.IsManual);
    }
}
