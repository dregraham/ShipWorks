using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    [ConditionElement("The number of payment details matching the following", "Order.QuantityOfPaymentDetail")]
    public class ForQuantityOfPaymentDetailCondition : ChildQuantityCondition
    {
        /// <summary>
        /// Type of entity
        /// </summary>
        protected override EntityType? EntityType
        {
            get { return ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity; }
        }

        /// <summary>
        /// Target scope of this container
        /// </summary>
        public override ConditionEntityTarget GetChildEntityTarget()
        {
            return ConditionEntityTarget.PaymentDetail;
        }
    }
}
