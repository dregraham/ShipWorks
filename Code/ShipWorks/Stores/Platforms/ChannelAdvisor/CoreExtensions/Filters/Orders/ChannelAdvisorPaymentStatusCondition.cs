using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    [ConditionElement("ChannelAdvisor Payment Status", "ChannelAdvisorOrder.PaymentStatus")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorPaymentStatusCondition : EnumCondition<ChannelAdvisorPaymentStatus>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorPaymentStatusCondition()
        {
            Value = ChannelAdvisorPaymentStatus.Cleared;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // We have to get from Order -> ChannelAdvisorOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.OnlinePaymentStatus), context));
            }
        }
    }
}
