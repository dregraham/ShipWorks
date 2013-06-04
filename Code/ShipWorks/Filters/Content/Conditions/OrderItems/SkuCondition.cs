using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the SKU of an OrderItem
    /// </summary>
    [ConditionElement("Item SKU", "OrderItem.SKU")]
    public class OrderItemSkuCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.SKU), context);
        }
    }
}
