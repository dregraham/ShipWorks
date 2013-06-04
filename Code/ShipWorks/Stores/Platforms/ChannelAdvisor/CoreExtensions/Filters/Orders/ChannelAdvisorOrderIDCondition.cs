using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.CoreExtensions.Filters
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
            // First we have to get from Order -> ChannelAdvisorOrder            
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(ChannelAdvisorOrderFields.CustomOrderIdentifier), context));
            }
        }
    }
}
