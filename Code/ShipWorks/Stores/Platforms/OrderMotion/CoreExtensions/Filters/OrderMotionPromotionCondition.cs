using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Filters
{
    /// <summary>
    /// Condition for the OrderMotion Promotion field
    /// </summary>
    [ConditionElement("OrderMotion Promotion", "OrderMotionOrder.Promotion")]
    [ConditionStoreType(StoreTypeCode.OrderMotion)]
    public class OrderMotionOrderPromotionCondition : StringCondition
    {
        /// <summary>
        /// Generate the filter SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> OrderMotionOrder           
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OrderMotionOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(OrderMotionOrderFields.OrderMotionPromotion), context));
            }
        }
    }
}
