using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.PersonName
{
    /// <summary>
    /// Condition that compares against the order's middle name
    /// </summary>
    [ConditionElement("Middle Name", "Order.Name.Middle")]
    public class OrderMiddleNameCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillMiddleName), context.GetColumnReference(OrderFields.ShipMiddleName), context);
        }
    }
}
