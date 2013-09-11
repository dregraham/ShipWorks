using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("The number of charges matching the following", "Order.QuantityOfCharge")]
    public class ForQuantityOfChargeCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Type of entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get { return ShipWorks.Data.Model.EntityType.OrderChargeEntity; }
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.OrderCharge;
        }
    }
}
