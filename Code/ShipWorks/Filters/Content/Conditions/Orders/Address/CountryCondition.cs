using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the country of an order.
    /// </summary>
    [ConditionElement("Country", "Order.Address.Country")]
    public class OrderAddressCountryCondition : BillShipCountryCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillCountryCode), context.GetColumnReference(OrderFields.ShipCountryCode), context);
        }
    }
}
