using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition that compares against the Order ID of an order
    /// </summary>
    [ConditionElement("Order ID", "Order.ID")]
    public class OrderIDCondition : NumericCondition<long>
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.OrderID), context);
        }
    }
}
