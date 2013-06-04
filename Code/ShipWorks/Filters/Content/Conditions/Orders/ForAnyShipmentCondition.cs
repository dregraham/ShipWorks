using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("For any shipment in the order...", "Order.ForAnyShipment")]
    public class ForAnyShipmentCondition : ContainerCondition
    {
        /// <summary>
        /// We introduce a new scope target
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Shipment;
        }

        /// <summary>
        /// Generat the SQL for the condition
        /// </summary>
        public override string GenerateSql(SqlGenerationContext context)
        {
            using (SqlGenerationScope scope = context.PushScope(EntityType.ShipmentEntity, SqlGenerationScopeType.AnyChild))
            {
                return scope.Adorn(base.GenerateSql(context));
            }
        } 
    }
}
