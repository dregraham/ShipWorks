using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Filters
{
    /// <summary>
    /// Walmart CustomerOrderID Condition
    /// </summary>
    [ConditionElement("Walmart Customer OrderID", "Walmart.CustomerOrderID")]
    [ConditionStoreType(StoreTypeCode.Walmart)]
    internal class WalmartCustomerOrderIDCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, WalmartOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderFields.CustomerOrderID), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, WalmartOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderSearchFields.CustomerOrderID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}