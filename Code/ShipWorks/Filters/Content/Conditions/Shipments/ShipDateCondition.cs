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
    /// Condition base on the ship date of a shipment
    /// </summary>
    [ConditionElement("Ship Date", "Shipment.ShipDate")]
    public class ShipDateCondition : DateCondition
    {
        /// <summary>
        /// Generate the SQL for the element
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipDate), context);
        }
    }
}
