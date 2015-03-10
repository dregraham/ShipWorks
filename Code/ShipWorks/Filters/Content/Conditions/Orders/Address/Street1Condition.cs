using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the Street1 of an order.
    /// </summary>
    [ConditionElement("Street1", "Order.Address.Street1")]
    public class OrderAddressStreet1Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillStreet1), context.GetColumnReference(OrderFields.ShipStreet1), context);
        }
    }
}
