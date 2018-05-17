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
    public class OverstockOrderSearchProvider : CombineOrderSearchBaseProvider<string>, IOverstockOrderSearchProvider
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
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.OverstockOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(OverstockOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => OverstockOrderSearchFields.OrderID.ToValue<string>())
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
        protected override string GetOnlineOrderIdentifier(IOrderEntity order) =>
            (order as IOverstockOrderEntity)?.OrderNumberComplete ?? string.Empty;
    }
}