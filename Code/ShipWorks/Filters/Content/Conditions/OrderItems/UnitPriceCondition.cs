using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ShipWorks.Filters.Content.Editors;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    /// <summary>
    /// Condition base on the unit price of an item
    /// </summary>
    [ConditionElement("Unit Price", "OrderItem.UnitPrice")]
    public class UnitPriceCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitPriceCondition()
        {
            // Format as currency
            format = "C";
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.UnitPrice), context);
        }
    }
}
