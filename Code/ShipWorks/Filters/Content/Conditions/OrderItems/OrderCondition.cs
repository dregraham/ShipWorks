using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.OrderItems
{
    [ConditionElement("If the order...", "OrderItem.Order")]
    public class OrderCondition : ContainerCondition
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
            using (SqlGenerationScope scope = context.PushScope(EntityType.OrderEntity, SqlGenerationScopeType.Parent))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }
    }
}
