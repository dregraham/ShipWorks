using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Class for identifying orders that are from Magento.  When an order is edited
    /// on the Magento website, the existing order is marked as cancelled and a new one 
    /// is created with a postfix (-1, -2, -3...).  This identifier handles these.
    /// </summary>
    public class MagentoOrderIdentifier : OrderNumberIdentifier
    {
        // the postfix on the order number
        readonly string postfix = "";
        readonly string prefix = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOrderIdentifier(long orderNumber, string prefix, string postfix) : base (orderNumber)
        {
            this.postfix = postfix;
            this.prefix = prefix;
        }
        
        /// <summary>
        /// Apply the order number and postfix value to the supplied order.
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            base.ApplyTo(order);

            if (postfix.Length > 0)
            {
                order.ApplyOrderNumberPostfix(postfix);
            }

            if (prefix.Length>0)
            {
                order.ApplyOrderNumberPrefix(prefix);
            }
        }

        /// <summary>
        /// Value to use when auditing
        /// </summary>
        public override string AuditValue => prefix + OrderNumber.ToString() + postfix;
    }
}
