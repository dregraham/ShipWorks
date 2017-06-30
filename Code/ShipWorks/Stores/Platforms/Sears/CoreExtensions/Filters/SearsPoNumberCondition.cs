using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Sears.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Sears PO Number field
    /// </summary>
    [ConditionElement("Sears PO Number", "SearsOrder.PoNumber")]
    [ConditionStoreType(StoreTypeCode.Sears)]
    public class SearsPoNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // first get from Order -> SearsOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, SearsOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(SearsOrderFields.PoNumber), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderSearchFields.OrderID, SearsOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(SearsOrderSearchFields.PoNumber), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
