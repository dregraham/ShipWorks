using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.PersonName
{
    /// <summary>
    /// Condition that compares against the shipments's Middle name
    /// </summary>
    [ConditionElement("Ship To Middle Name", "Shipment.ShipTo.MiddleName")]
    public class ShipToMiddleNameCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipMiddleName), context);
        }
    }
}
