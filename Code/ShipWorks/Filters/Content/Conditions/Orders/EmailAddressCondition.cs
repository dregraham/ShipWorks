using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition that compares against the email address of an order
    /// </summary>
    [ConditionElement("Email Address", "Order.EmailAddress")]
    public class OrderEmailAddressCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillEmail), context.GetColumnReference(OrderFields.ShipEmail), context);
        }
    }
}
