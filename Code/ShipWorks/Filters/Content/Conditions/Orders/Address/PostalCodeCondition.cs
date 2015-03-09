using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the PostalCode of an order.
    /// </summary>
    [ConditionElement("Postal Code", "Order.Address.PostalCode")]
    public class OrderAddressPostalCodeCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillPostalCode), context.GetColumnReference(OrderFields.ShipPostalCode), context);
        }
    }
}
