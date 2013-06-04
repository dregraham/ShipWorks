using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the ChannelAdvisor sales source
    /// </summary>
    [ConditionElement("ChannelAdvisor Sales ID", "ChannelAdvisorOrderItem.SalesSourceID")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorMarketplaceSalesIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from OrderItem -> ChannelAdvisorOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, ChannelAdvisorOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderItemFields.MarketplaceSalesID), context));
            }
        }
    }
}
