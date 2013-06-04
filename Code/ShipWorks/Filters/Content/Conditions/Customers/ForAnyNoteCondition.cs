using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("For any note in the customer...", "Customer.ForAnyNote")]
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
