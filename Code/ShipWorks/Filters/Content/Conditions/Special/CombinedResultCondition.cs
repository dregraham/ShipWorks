using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Content.Conditions.Special
{
    [ConditionElement("The combined result of...", "Special.NestedConditions")]
    public class CombinedResultCondition : ContainerCondition
    {
        /// <summary>
        /// Get the entity target of all children - which will be the same as us.
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return GetScopedEntityTarget();
        }
    }
}
