using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the ISBN of an OrderItem
    /// </summary>
    [ConditionElement("Item ISBN", "OrderItem.ISBN")]
    public class OrderItemIsbnCondition : StringCondition
    {
        /// <summary>
        /// Create the SQL for filtering on ISBN
        /// </summary>
        public override string GenerateSql(SqlGeneration.SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.ISBN), context);
        }
    }
}
