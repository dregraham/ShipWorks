using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// Class for identifying orders that are from generic file import.
    /// </summary>
    public class GenericFileOrderIdentifier : OrderNumberIdentifier
    {
        // the postfix on the order number
        string orderPostfix = "";

        // the prefix on the order number
        string orderPrefix = "";

        // the complete order number, including pre- and post- fix
        string orderNumberComplete = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileOrderIdentifier(long orderNumber, string prefix, string postfix)
            : base(orderNumber)
        {
            this.orderPrefix = prefix;
            this.orderPostfix = postfix;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileOrderIdentifier(long orderNumber, string orderNumberComplete)
            : base(orderNumber)
        {
            this.orderNumberComplete = orderNumberComplete;
        }

        /// <summary>
        /// Apply the order number and postfix value to the supplied order.
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            base.ApplyTo(order);

            if (orderNumberComplete == null)
            {
                order.ApplyOrderNumberPrefix(orderPrefix);
                order.ApplyOrderNumberPostfix(orderPostfix);
            }
            else
            {
                order.OrderNumberComplete = orderNumberComplete;
            }
        }

        /// <summary>
        /// Apply the order number and postfix to the supplied download detail entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            // using the base behavior
            base.ApplyTo(downloadDetail);

            if (orderNumberComplete == null)
            {
                OrderEntity prototype = new OrderEntity();
                prototype.OrderNumber = base.OrderNumber;
                prototype.ApplyOrderNumberPrefix(orderPrefix);
                prototype.ApplyOrderNumberPostfix(orderPostfix);

                downloadDetail.ExtraStringData1 = prototype.OrderNumberComplete;
            }
            else
            {
                downloadDetail.ExtraStringData1 = orderNumberComplete;
            }
        }
    }
}
