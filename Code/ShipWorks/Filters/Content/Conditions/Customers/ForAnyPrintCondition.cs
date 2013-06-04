using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    /// <summary>
    /// Base condition for entering a child print scope
    /// </summary>
    [ConditionElement("For any print...", "Customer.ForAnyPrint")]
    public class ForAnyPrintCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Printed;
        }

        /// <summary>
        /// Generate the sql for the given child scope type
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = CustomerPrintedCondition.CreateChildScope(context, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }
    }
}
