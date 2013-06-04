using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo
{
    /// <summary>
    /// Condition that compares against the shipments's email
    /// </summary>
    [ConditionElement("Ship To Email Address", "Shipment.ShipTo.EmailAddress")]
    public class ShipToEmailCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipEmail), context);
        }
    }
}
