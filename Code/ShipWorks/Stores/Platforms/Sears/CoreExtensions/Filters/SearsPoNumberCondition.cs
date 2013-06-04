using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Sears.CoreExtensions.Filters
{
    [ConditionElement("Sears PO Number", "SearsOrder.PoNumber")]
    [ConditionStoreType(StoreTypeCode.Sears)]
    public class SearsPoNumberCondition : StringCondition
    {
        public override string GenerateSql(SqlGenerationContext context)
        {
            // first get from Order -> SearsOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, SearsOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(SearsOrderFields.PoNumber), context));
            }
        }
    }
}
