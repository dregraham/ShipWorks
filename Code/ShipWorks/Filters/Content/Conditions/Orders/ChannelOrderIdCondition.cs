using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ShipWorks.Filters.Content.Editors;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition base on the date of an Order
    /// </summary>
    [ConditionElement("Channel Order ID", "Order.ChannelOrderID")]
    public class ChannelOrderIdCondition : StringCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.ChannelOrderID), context);
        }
    }
}
