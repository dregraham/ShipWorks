using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Filters
{
    [ConditionElement("LemonStand Order #", "LemonStand.OrderID")]
    [ConditionStoreType(StoreTypeCode.LemonStand)]
    class LemonStandOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, LemonStandOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(LemonStandOrderFields.LemonStandOrderID), context));
            }
        }
    }
}
