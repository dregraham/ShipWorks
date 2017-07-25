using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Filters
{
    /// <summary>
    /// Walmart PurchaseOrderID Condition
    /// </summary>
    [ConditionElement("Walmart Purchase OrderID", "Walmart.PurchaseOrderID")]
    [ConditionStoreType(StoreTypeCode.Walmart)]
    internal class WalmartPurchaseOrderIDCondition : StringCondition
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
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderFields.PurchaseOrderID), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, WalmartOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderSearchFields.PurchaseOrderID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}