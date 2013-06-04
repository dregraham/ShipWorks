using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("For every order...", "Customer.ForEveryOrder")]
    public class ForEveryOrderCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Order;
        }

        /// <summary>
        /// Generate the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.OrderEntity, SqlGenerationScopeType.EveryChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }
    }
}
