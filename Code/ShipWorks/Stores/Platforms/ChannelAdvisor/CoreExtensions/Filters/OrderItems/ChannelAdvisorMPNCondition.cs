using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the ChannelAdvisor MPN
    /// </summary>
    [ConditionElement("ChannelAdvisor MPN", "ChannelAdvisorOrderItem.MPN")]
    [ConditionStoreType(StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorMPNCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) => GenerateSql(context.GetColumnReference(OrderItemFields.MPN), context);
    }
}
