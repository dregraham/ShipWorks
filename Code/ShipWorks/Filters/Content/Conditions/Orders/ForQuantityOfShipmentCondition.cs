using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("The number of shipments matching the following", "Order.QuantityOfShipment")]
    public class ForQuantityOfShipmentCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Type of entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get { return ShipWorks.Data.Model.EntityType.ShipmentEntity; }
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.Shipment;
        }
    }
}
