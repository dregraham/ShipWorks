﻿using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the Custom6 of an OrderItem
    /// </summary>
    [ConditionElement("Custom Field 6", "OrderItem.Custom6")]
    public class Custom6Condition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context) =>
            GenerateSql(context.GetColumnReference(OrderItemFields.Custom6), context);
    }
}
