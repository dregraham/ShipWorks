using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the Street2 of an order.
    /// </summary>
    [ConditionElement("Street2", "Order.Address.Street2")]
    public class OrderAddressStreet2Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillStreet2), context.GetColumnReference(OrderFields.ShipStreet2), context);
        }
    }
}
