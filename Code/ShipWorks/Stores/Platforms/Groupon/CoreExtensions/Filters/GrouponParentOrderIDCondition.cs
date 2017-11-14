using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Groupon.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Groupon parent order ID
    /// </summary>
    [ConditionElement("Groupon Parent Order ID", "Groupon.ParentOrderID")]
    [ConditionStoreType(StoreTypeCode.Groupon)]
    public class GrouponParentOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GrouponOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(GrouponOrderFields.ParentOrderID), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, GrouponOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(GrouponOrderSearchFields.ParentOrderID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
