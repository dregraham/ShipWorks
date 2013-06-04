using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Orders.Address
{
    /// <summary>
    /// Condition that compares against the state\province of an order.
    /// </summary>
    [ConditionElement("State\\Province", "Order.Address.StateProvince")]
    public class OrderAddressStateProvinceCondition : BillShipStateProvinceCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.BillStateProvCode), context.GetColumnReference(OrderFields.ShipStateProvCode), context);
        }
    }
}
