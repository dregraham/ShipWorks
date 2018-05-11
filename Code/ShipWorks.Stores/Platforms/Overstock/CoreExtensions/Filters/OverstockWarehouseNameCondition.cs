using System;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Overstock.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the Overstock order warehouse names
    /// </summary>
    [ConditionElement("Overstock Warehouse Name", "Overstock.WarehouseName")]
    [ConditionStoreType(StoreTypeCode.Overstock)]
    public class OverstockOrderNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // Add WarehouseName entries.
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OverstockOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context.GetColumnReference(OverstockOrderFields.WarehouseName), context));
            }
        }
    }
}
