using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("For any note in the order...", "Order.ForAnyNote")]
    public class ForAnyNoteCondition : ForChildNoteCondition
    {
        /// <summary>
        /// Generat the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return GenerateSql(context, SqlGenerationScopeType.AnyChild);
        }
    }
}
