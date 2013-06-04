using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderCharges
{
    /// <summary>
    /// Condition that compares against the charge of an order
    /// </summary>
    [ConditionElement("Charge Description", "OrderCharge.Description")]
    public class OrderChargeDescriptionCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderChargeFields.Description), context);
        }
    }
}
