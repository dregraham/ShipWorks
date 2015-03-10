using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the Street3 of an order.
    /// </summary>
    [ConditionElement("Street3", "Order.Address.Street3")]
    public class OrderAddressStreet3Condition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillStreet3), context.GetColumnReference(OrderFields.ShipStreet3), context);
        }
    }
}
