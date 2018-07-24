using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.Overstock.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Overstock SOFS Created Date
    /// </summary>
    [ConditionElement("SOFS Created Date", "Overstock.SOFSCreatedDate")]
    [ConditionStoreType(StoreTypeCode.Overstock)]
    public class OverstockSofsCreatedDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Add SOFSCreatedDate entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OverstockOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(OverstockOrderFields.SofsCreatedDate), context));
            }
        }
    }
}
