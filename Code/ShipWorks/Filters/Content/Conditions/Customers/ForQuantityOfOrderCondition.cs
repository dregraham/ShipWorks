using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Customers
{
    [ConditionElement("The number of orders matching the following", "Customer.QuantityOfOrder")]
    public class ForQuantityOfOrderCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Type of entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get { return ShipWorks.Data.Model.EntityType.OrderEntity; }
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Order;
        }
    }
}
