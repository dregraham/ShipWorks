using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments
{
    [ConditionElement("Is Return", "Shipment.ReturnStatus")]
    public class ReturnStatusCondition : EnumCondition<ReturnStatusType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnStatusCondition()
        {
            Value = ReturnStatusType.ReturnShipment;
            SelectedValues = new[] { Value };
        }

        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ReturnShipment), context);
        }
    }
}
