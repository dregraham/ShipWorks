using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Email;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition for determining what templates have been used to send emails for an order
    /// </summary>
    [ConditionElement("Printed", "Order.Printed")]
    public class OrderPrintedCondition : PrintedCondition
    {
        /// <summary>
        /// Create the scope used to get the the child emails
        /// </summary>
        protected override SqlGenerationScope CreateScope(SqlGenerationContext context, SqlGenerationScopeType scopeType)
        {
            return context.PushScope(OrderFields.OrderID, PrintResultFields.RelatedObjectID, scopeType);
        }
    }
}
