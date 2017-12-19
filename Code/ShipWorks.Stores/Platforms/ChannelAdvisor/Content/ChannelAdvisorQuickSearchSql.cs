using System.Collections.Generic;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.QuickSearch;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Content
{
    /// <summary>
    /// Generate SQL lines for a quick search for ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorQuickSearchSql : IQuickSearchStoreSql
    {
        /// <summary>
        /// Store type supported by this IQuickSearchStoreSql
        /// </summary>
        public StoreTypeCode StoreType => StoreTypeCode.ChannelAdvisor;

        /// <summary>
        /// Generate SQL lines for a quick search for ChannelAdvisor
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationBuilder context, string searchText)
        {
            var customOrderIdentifierParam = context.RegisterParameter(ChannelAdvisorOrderFields.CustomOrderIdentifier, searchText);
            var marketplaceBuyerIDParam = context.RegisterParameter(ChannelAdvisorOrderItemFields.MarketplaceBuyerID, searchText);

            return new[]
            {
                $"SELECT OrderId FROM [ChannelAdvisorOrder] WHERE {ChannelAdvisorOrderFields.CustomOrderIdentifier.Name} LIKE {customOrderIdentifierParam}",
                $"SELECT OrderId FROM [ChannelAdvisorOrderSearch] WHERE {ChannelAdvisorOrderSearchFields.CustomOrderIdentifier.Name} LIKE {customOrderIdentifierParam}",
                $@"SELECT oi.OrderID
FROM   [ChannelAdvisorOrderItem] AS caoi INNER JOIN [OrderItem] AS oi ON caoi.OrderItemID = oi.OrderItemID
WHERE  (caoi.{ChannelAdvisorOrderItemFields.MarketplaceBuyerID.Name} LIKE {marketplaceBuyerIDParam})"
            };
        }
    }
}
