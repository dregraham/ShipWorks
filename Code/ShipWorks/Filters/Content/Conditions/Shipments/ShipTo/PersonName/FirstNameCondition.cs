using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.PersonName
{
    /// <summary>
    /// Condition that compares against the shipments's first name
    /// </summary>
    [ConditionElement("Ship To First Name", "Shipment.ShipTo.FirstName")]
    public class ShipToFirstNameCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipFirstName), context);
        }
    }
}
