using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

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
            QueryFactory factory = new QueryFactory();

            var from = factory.ProStoresOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(ProStoresOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(
                    () => new OrderUploadDetails(
                        OrderSearchFields.OrderNumber.ToValue<long>(),
                        OrderSearchFields.IsManual.ToValue<bool>()))
                .Distinct()
                .Where(ProStoresOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override OrderUploadDetails GetOnlineOrderIdentifier(IOrderEntity order) =>
            new OrderUploadDetails(order.OrderNumber, order.IsManual);
    }
}
