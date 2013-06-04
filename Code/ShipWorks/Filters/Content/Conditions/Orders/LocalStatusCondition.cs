using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Condition that compares against the local status of an order
    /// </summary>
    [ConditionElement("Local Status", "Order.LocalStatus")]
    public class LocalStatusCondition : StringCondition
    {
        /// <summary>
        /// Generate the sql
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context.GetColumnReference(OrderFields.LocalStatus), context);
        }

        /// <summary>
        /// Provide a list of standard charge types for the user to choose from
        /// </summary>
        public override ICollection<string> GetStandardValues()
        {
            return StatusPresetManager.GetAllPresets(StatusPresetTarget.Order);
        }
    }
}
