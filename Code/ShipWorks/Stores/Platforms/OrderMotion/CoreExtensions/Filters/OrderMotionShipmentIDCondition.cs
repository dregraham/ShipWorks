using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Stores.Platforms.OrderMotion.CoreExtensions.Filters
{
    /// <summary>
    /// An OrderMotion filter to search orders based on the OrderMotionOrder.ShipmentID value.
    /// </summary>
    [ConditionElement("OrderMotion Shipment", "OrderMotionOrder.ShipmentID")]
    [ConditionStoreType(StoreTypeCode.OrderMotion)]
    public class OrderMotionShipmentIDCondition : NumericCondition<int>
    {
        /// <summary>
        /// Generate the SQL for the condition element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            string orderSql = String.Empty;
            string orderSearchSql = String.Empty;

            // First we have to get from Order -> OrderMotionOrder
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, OrderMotionOrderFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSql = scope.Adorn(GenerateSql(context.GetColumnReference(OrderMotionOrderFields.OrderMotionShipmentID), context));
            }

            using (SqlGenerationScope scope = context.PushScope(OrderSearchFields.OrderID, OrderMotionOrderSearchFields.OrderID, SqlGenerationScopeType.AnyChild))
            {
                orderSearchSql = scope.Adorn(GenerateSql(context.GetColumnReference(OrderMotionOrderSearchFields.OrderMotionShipmentID), context));
            }

            return $"{orderSql} OR {orderSearchSql}";
        }
    }
}
