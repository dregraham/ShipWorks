using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Filters
{
    /// <summary>
    /// Walmart Item OnlineStatus Condition
    /// </summary>
    [ConditionElement("Walmart Item Status", "WalmartItem.OnlineStatus")]
    [ConditionStoreType(StoreTypeCode.Walmart)]
    internal class WalmartItemOnlineStatusCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, WalmartOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderItemFields.LocalStatus), context));
            }
        }
    }
}