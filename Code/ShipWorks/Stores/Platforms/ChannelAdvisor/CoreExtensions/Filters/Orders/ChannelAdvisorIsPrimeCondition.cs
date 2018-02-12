using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    [ConditionElement("ChannelAdvisor Is Amazon Prime", "ChannelAdvisorOrder.IsPrime")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorIsPrimeCondition : EnumCondition<AmazonIsPrime>
    {
        public ChannelAdvisorIsPrimeCondition()
        {
            Value = AmazonIsPrime.Yes;
            SelectedValues = new[] { Value };
        }

        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.IsPrime), context));
            }
        }
    }
}
