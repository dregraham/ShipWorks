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
    /// Condition for determining whether a ChannelAdvisor order item is fulfilled by Amazon
    /// </summary>
    [ConditionElement("ChannelAdvisor Fullfilled By Amazon", "ChannelAdvisorOrderItem.IsFBA")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorIsFBACondition : BooleanCondition
    {
        public ChannelAdvisorIsFBACondition()
            : base("Yes", "No")
        { }

        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from OrderItem -> ChannelAdvisorOrderItem
            using (SqlGenerationScope scope = context.PushScope(OrderItemFields.OrderItemID, ChannelAdvisorOrderItemFields.OrderItemID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderItemFields.IsFBA), context));
            }
        }
    }
}
