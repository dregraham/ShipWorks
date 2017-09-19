using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the ChannelAdvisor distribution center name
    /// </summary>
    [ConditionElement("ChannelAdvisor Distribution Center Name", "ChannelAdvisorOrderItem.DistributionCenterName")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorDistributionCenterNameCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from OrderItem -> ChannelAdvisorOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, ChannelAdvisorOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderItemFields.DistributionCenterName), context));
            }
        }
    }
}