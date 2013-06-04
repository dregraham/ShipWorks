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
    /// Condition based on the date a shipment was voided
    /// </summary>
    [ConditionElement("Voided Date", "Shipment.VoidedDate")]
    public class VoidedDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return string.Format("({0} = 1 AND {1})",
                context.GetColumnReference(ShipmentFields.Voided),
                GenerateSql(context.GetColumnReference(ShipmentFields.VoidedDate), context));
        }
    }
}
