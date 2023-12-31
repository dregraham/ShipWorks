﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class YahooCombineOrderSearchProvider : CombineOrderSearchBaseProvider<string>, IYahooCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.YahooOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(YahooOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => YahooOrderSearchFields.YahooOrderID.ToValue<string>())
                .Distinct()
                .Where(YahooOrderSearchFields.OrderID == order.OrderID)
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
            (order as IYahooOrderEntity)?.YahooOrderID ?? string.Empty;
    }
}
