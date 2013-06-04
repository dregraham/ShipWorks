using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition that compares against the UPC of an OrderItem
    /// </summary>
    [ConditionElement("Item UPC", "OrderItem.UPC")]
    public class OrderItemUpcCondition : StringCondition
    {
        /// <summary>
        /// Create the SQL for filtering on UPC
        /// </summary>
        public override string GenerateSql(SqlGeneration.SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.UPC), context);
        }
    }
}
