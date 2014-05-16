using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.PersonName
{
    /// <summary>
    /// Condition that compares against the order's first name
    /// </summary>
    [ConditionElement("First Name", "Order.Name.First")]
    public class OrderFirstNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillFirstName), context.GetColumnReference(OrderFields.ShipFirstName), context);
        }
    }
}
