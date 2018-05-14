using System;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Overstock.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Overstock order Channel names
    /// </summary>
    [ConditionElement("Overstock Channel Name", "Overstock.ChannelName")]
    [ConditionStoreType(StoreTypeCode.Overstock)]
    public class OverstockChannelNameCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Add Channel entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OverstockOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(OverstockOrderFields.SalesChannelName), context));
            }
        }
    }
}
