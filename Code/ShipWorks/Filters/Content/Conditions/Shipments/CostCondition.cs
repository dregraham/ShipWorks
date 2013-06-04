using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ShipWorks.Filters.Content.Editors;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    /// <summary>
    /// Condition base on the cost of an shipment
    /// </summary>
    [ConditionElement("Cost", "Shipment.Cost")]
    public class CostCondition : NumericCondition<decimal>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CostCondition()
        {
            // Format as currency
            format = "C";
        }

        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipmentCost), context);
        }
    }
}
