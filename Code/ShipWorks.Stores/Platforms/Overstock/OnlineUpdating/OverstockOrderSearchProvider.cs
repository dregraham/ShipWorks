using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class OverstockOrderSearchProvider : CombineOrderSearchBaseProvider<OverstockOrderDetail>, IOverstockOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override async Task<IEnumerable<OverstockOrderDetail>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.OverstockOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(OverstockOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new OverstockOrderDetail(
                    OrderSearchFields.OrderNumberComplete.ToValue<string>(),
                    OverstockOrderSearchFields.SalesChannelName.ToValue<string>(),
                    OverstockOrderSearchFields.WarehouseCode.ToValue<string>(),
                    OverstockOrderSearchFields.OriginalOrderID.ToValue<long>(),
                    OrderSearchFields.IsManual.ToValue<bool>()))
                .Distinct()
                .Where(OverstockOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override OverstockOrderDetail GetOnlineOrderIdentifier(IOrderEntity order) =>
            GetOnlineOrderIdentifier((IOverstockOrderEntity) order);

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        private OverstockOrderDetail GetOnlineOrderIdentifier(IOverstockOrderEntity order) =>
            new OverstockOrderDetail(order.OrderNumberComplete, order.SalesChannelName, order.WarehouseCode, order.OrderID, order.IsManual);
    }
}