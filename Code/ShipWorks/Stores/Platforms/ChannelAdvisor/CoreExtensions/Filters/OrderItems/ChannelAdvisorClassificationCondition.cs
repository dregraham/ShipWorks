using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    [ConditionElement("ChannelAdvisor Classification", "ChannelAdvisorOrderItem.Classification")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorClassificationCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, ChannelAdvisorOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderItemFields.Classification), context));
            }
        }
    }
}
