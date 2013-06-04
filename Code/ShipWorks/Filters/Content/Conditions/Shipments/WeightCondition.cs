using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Condition that compares against the shipments's  weight
    /// </summary>
    [ConditionElement("Weight", "Shipment.Weight")]
    public class ShipmentWeightCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentWeightCondition()
        {
            // Format as general
            format = "G";
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.TotalWeight), context);
        }
    }
}
