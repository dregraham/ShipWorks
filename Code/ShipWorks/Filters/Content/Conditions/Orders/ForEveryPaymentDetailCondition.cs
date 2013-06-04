using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("For every payment detail in the order...", "Order.ForEveryPaymentDetail")]
    public class ForEveryPaymentDetailCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.PaymentDetail;
        }

        /// <summary>
        /// Generat the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.OrderPaymentDetailEntity, SqlGenerationScopeType.EveryChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        }
    }
}
