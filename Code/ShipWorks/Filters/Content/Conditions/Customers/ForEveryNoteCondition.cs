using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("For every note in the customer...", "Customer.ForEveryNote")]
    public class ForEveryNoteCondition : ForChildNoteCondition
    {
        /// <summary>
        /// Generat the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return base.GenerateSql(context, SqlGenerationScopeType.EveryChild);
        }
    }
}
