using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Condition for the ChannelAdvisor CustomOrderID field
    /// </summary>
    [ConditionElement("ChannelAdvisor Order ID", "ChannelAdvisorOrder.CustomOrderIdentifier")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorOrderIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // First we have to get from Order -> ChannelAdvisorOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.CustomOrderIdentifier), context));
            }

            // Add any combined order OrderID entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderSearchFields.CustomOrderIdentifier), context));
            }

            // OR the two together.
            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
