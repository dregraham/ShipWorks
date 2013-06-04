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
    /// Condition based on the processed date of a shipment
    /// </summary>
    [ConditionElement("Processed Date", "Shipment.ProcessedDate")]
    public class ProcessedDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return string.Format("({0} = 1 AND {1})",
                context.GetColumnReference(ShipmentFields.Processed),
                GenerateSql(context.GetColumnReference(ShipmentFields.ProcessedDate), context));
        }
    }
}
