using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Filters
{
    /// <summary>
    /// Walmart EstimatedShipDate Condition
    /// </summary>
    [ConditionElement("Walmart Estimated Ship Date", "Walmart.EstimatedShipDate")]
    [ConditionStoreType(StoreTypeCode.Walmart)]
    internal class WalmartEstimatedShipDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, WalmartOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(WalmartOrderFields.EstimatedShipDate), context));
            }
        }
    }
}