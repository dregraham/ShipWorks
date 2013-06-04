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
    /// Base condition for entering a child print scope
    /// </summary>
    [ConditionElement("For any print...", "Order.ForAnyPrint")]
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
            using (SqlGenerationScope scope = context.PushScope(OrderFields.OrderID, PrintResultFields.RelatedObjectID, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        } 
    }
}
