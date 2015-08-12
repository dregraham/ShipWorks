using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the MarketplaceAdvisor Invoice Number field
    /// </summary>
    [ConditionElement("Groupon Order ID", "Groupon.OrderID")]
    [ConditionStoreType(StoreTypeCode.Groupon)]
    public class GrouponOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GrouponOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(GrouponOrderFields.GrouponOrderID), context));
            }
        }
    }
}
