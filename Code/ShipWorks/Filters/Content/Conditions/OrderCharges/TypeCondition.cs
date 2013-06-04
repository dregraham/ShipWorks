using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.OrderCharges
{
    /// <summary>
    /// Condition that compares against the charge type of an order
    /// </summary>
    [ConditionElement("Charge Type", "OrderCharge.Type")]
    public class OrderChargeTypeCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderChargeFields.Type), context);
        }

        /// <summary>
        /// Provide a list of standard charge types for the user to choose from
        /// </summary>
        public override ICollection<string> GetStandardValues()
        {
            return new string[] { "Shipping", "Tax", "Insurance" };
        }
    }
}
