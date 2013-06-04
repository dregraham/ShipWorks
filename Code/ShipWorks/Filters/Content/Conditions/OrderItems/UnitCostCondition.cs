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
    /// Condition base on the unit cost of an item
    /// </summary>
    [ConditionElement("Unit Cost", "OrderItem.UnitCost")]
    public class UnitCostCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitCostCondition()
        {
            // Format as currency
            format = "C";
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderItemFields.UnitCost), context);
        }
    }
}
