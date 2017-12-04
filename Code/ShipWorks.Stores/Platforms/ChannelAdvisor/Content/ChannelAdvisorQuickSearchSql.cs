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
        /// Generate SQL lines for a quick search for ChannelAdvisor
        /// </summary>
        public IEnumerable<string> GenerateSql(ISqlGenerationContext context, string searchText)
        {
            context.AddColumnUsed(ChannelAdvisorOrderFields.CustomOrderIdentifier);
            var customOrderIdentifierParam = context.RegisterParameter(searchText);

            context.AddColumnUsed(ChannelAdvisorOrderItemFields.MarketplaceBuyerID);
            var marketplaceBuyerIDParam = context.RegisterParameter(searchText);

            return new[]
            {
                $"SELECT OrderId FROM [ChannelAdvisorOrder] WHERE {ChannelAdvisorOrderFields.CustomOrderIdentifier.Name} LIKE {customOrderIdentifierParam}",
                $"SELECT OrderId FROM [ChannelAdvisorOrderSearch] WHERE {ChannelAdvisorOrderSearchFields.CustomOrderIdentifier.Name} LIKE {customOrderIdentifierParam}",
                $"SELECT OrderId FROM [ChannelAdvisorOrderItem] WHERE {ChannelAdvisorOrderItemFields.MarketplaceBuyerID.Name} LIKE {marketplaceBuyerIDParam}"
            };
        }
    }
}
