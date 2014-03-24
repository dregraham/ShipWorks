using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderCharges
{
    /// <summary>
    /// Condition that compares against the Amount of an OrderCharge
    /// </summary>
    [ConditionElement("Charge Amount", "OrderCharge.Amount")]
    public class OrderChargeAmountCondition : MoneyCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderChargeFields.Amount), context);
        }
    }
}
