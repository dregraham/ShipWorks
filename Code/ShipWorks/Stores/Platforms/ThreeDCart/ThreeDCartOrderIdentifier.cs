using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Uniquely identifies an ThreeDCart order in the database
    ///
    /// Since 3D Cart orders can have an invoice prefix, when using the regular OrderNumberIdentifier, orders would not be found
    /// What was happening:
    /// - Download order 1100, with prefix AB- for the first time
    /// - OrderNumberIdentifier builds a bucket looking for order number and order number complete
    /// - Create and persist the order to the db.
    /// - Order gets modified on website, we re-download it
    /// - OrderNumberIdentifier builds a bucket looking for order number and order number complete
    /// - Order number complete doesn't have the prefix since OrderNumberIdentifier doesn't know about it
    /// - Since we couldn't pass the prefix, this search would not find order number 1100, and a new order entity was created
    /// </summary>
    public class ThreeDCartOrderIdentifier : OrderNumberIdentifier
    {
        // the prefix on the order number
        private readonly string orderPrefix;
        // the postFix on the order number.  This is only used for "sub" orders when an order has multiple shipping addresses.
        private readonly string orderPostfix;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invoiceNumber">The 3D Cart invoice number (without prefix), which will be stored as OrderNumber</param>
        /// <param name="prefix">The prefix for the invoice number.</param>
        /// <param name="postfix">The postfix for the invoice number.</param>
        public ThreeDCartOrderIdentifier(long invoiceNumber, string prefix, string postfix)
            : base(invoiceNumber)
        {
            orderPrefix = prefix;
            orderPostfix = postfix;
        }

        /// <summary>
        /// Returns the order prefix used by this ThreeDCartOrderIdentifier
        /// </summary>
        public string OrderPrefix => orderPrefix;

        /// <summary>
        /// Returns the order postfix used by this ThreeDCartOrderIdentifier
        /// </summary>
        public string OrderPostfix => orderPostfix;

        /// <summary>
        /// Apply the order number and order prefix to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            base.ApplyTo(order);

            order.ApplyOrderNumberPrefix(orderPrefix);
            order.ApplyOrderNumberPostfix(orderPostfix);
        }

        /// <summary>
        /// Returns a string version of the identifier
        /// </summary>
        public override string ToString() => $"{OrderPrefix}{OrderNumber}{orderPostfix}";
    }
}
