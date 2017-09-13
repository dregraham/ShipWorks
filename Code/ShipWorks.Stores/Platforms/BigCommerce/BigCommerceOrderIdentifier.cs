using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Uniquely identifies an BigCommerce order in the database
    /// </summary>
    public class BigCommerceOrderIdentifier : OrderNumberIdentifier
    {
        // The postfix on the order number
        // This is used for multiple shipment orders.  It does not come from BigCommerce
        private readonly string orderPostfix;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderNumber">The BigCommerce OrderNumber</param>
        /// <param name="postfix">The postfix for the order number.  (in case we are creating orders for multiple shipping addresses)</param>
        public BigCommerceOrderIdentifier(long orderNumber, string postfix)
            : base(orderNumber)
        {
            orderPostfix = postfix;
        }

        /// <summary>
        /// Returns the order postfix used by this BigCommerceOrderIdentifier
        /// </summary>
        public string OrderPostfix
        {
            get
            {
                return orderPostfix;
            }
        }

        /// <summary>
        /// Apply the order number and order postfix to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            base.ApplyTo(order);

            order.ApplyOrderNumberPostfix(orderPostfix);
        }
    }
}
