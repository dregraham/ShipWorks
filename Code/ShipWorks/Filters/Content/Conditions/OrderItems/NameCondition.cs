using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the Name of an OrderItem
    /// </summary>
    [ConditionElement("Item Name", "OrderItem.Name")]
    public class OrderItemNameCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.Name), context);
        }
    }
}
