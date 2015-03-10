using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.PersonName
{
    /// <summary>
    /// Condition that compares against the order's last name
    /// </summary>
    [ConditionElement("Last Name", "Order.Name.Last")]
    public class OrderLastNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillLastName), context.GetColumnReference(OrderFields.ShipLastName), context);
        }
    }
}
