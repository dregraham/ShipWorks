using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("For any order...", "Customer.ForAnyOrder")]
    public class ForAnyOrderCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Order;
        }

        /// <summary>
        /// Generate the SQL
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.OrderEntity, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }
    }
}
