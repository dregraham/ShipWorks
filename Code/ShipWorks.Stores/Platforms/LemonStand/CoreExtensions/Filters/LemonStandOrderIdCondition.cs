using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Lemon Stand CustomOrderID field
    /// </summary>
    [ConditionElement("LemonStand Order ID", "LemonStand.LemonStandOrderID")]
    [ConditionStoreType(StoreTypeCode.LemonStand)]
    internal class LemonStandOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, LemonStandOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(LemonStandOrderFields.LemonStandOrderID), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderSearchFields.OrderID, LemonStandOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(LemonStandOrderSearchFields.LemonStandOrderID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}