using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Base condition for entering a child email scope
    /// </summary>
    [ConditionElement("For every email...", "Order.ForEveryEmail")]
    public class ForEveryEmailCondition : ForChildEmailedCondition
    {
        /// <summary>
        /// Generate the sql for the given child scope type
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            return base.GenerateSql(context, SqlGenerationScopeType.EveryChild);
        } 
    }
}
