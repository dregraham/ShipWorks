using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the Quantity of an OrderItem
    /// </summary>
    [ConditionElement("Item Quantity", "OrderItem.Quantity")]
    public class OrderItemQuantityCondition : NumericCondition<int>
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.Quantity), context);
        }
    }
}
