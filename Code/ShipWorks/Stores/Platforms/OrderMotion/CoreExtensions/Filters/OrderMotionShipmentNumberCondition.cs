using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Filters
{
    /// <summary>
    /// An OrderMotion filter to search orders based on the OrderMotionOrder.ShipmentNumber value.
    /// </summary>
    [ConditionElement("OrderMotion Shipment Number", "OrderMotionOrder.ShipmentNumber")]
    [ConditionStoreType(StoreTypeCode.OrderMotion)]
    public class OrderMotionShipmentNumberCondition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the condition element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            // First we have to get from Order -> OrderMotionOrder           
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OrderMotionOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(GenerateSql(context.GetColumnReference(OrderMotionOrderFields.OrderMotionShipmentNumber), context));
            }
        }
    }
}
