using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Shipments.ShipTo.Address
{
    /// <summary>
    /// Condition that compares against the shipments's State\Province
    /// </summary>
    [ConditionElement("Ship To State\\Province", "Shipment.ShipTo.StateProvince")]
    public class ShipToStateProvinceCondition : StateProvinceCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(ShipmentFields.ShipStateProvCode), context);
        }
    }
}
