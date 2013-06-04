using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("For every note in the order...", "Order.ForEveryNote")]
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
