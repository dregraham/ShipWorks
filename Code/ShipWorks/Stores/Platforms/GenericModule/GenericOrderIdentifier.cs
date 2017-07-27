using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Class for identifying orders that are from Magento.  When an order is edited
    /// on the Magento website, the existing order is marked as cancelled and a new one 
    /// is created with a postfix (-1, -2, -3...).  This identifier handles these.
    /// </summary>
    public class GenericOrderIdentifier : OrderNumberIdentifier
    {
        // the postfix on the order number
        readonly string orderPostfix = "";

        // the prefix on the order number
        readonly string orderPrefix = "";

        // the complete order number, including pre- and post- fix
        readonly string orderNumberString = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericOrderIdentifier(long orderNumber, string prefix, string postfix)
            : base(orderNumber)
        {
            orderNumberString = orderNumber.ToString();
            orderPrefix = prefix;
            orderPostfix = postfix;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericOrderIdentifier(string orderNumber, string prefix, string postfix)
            : this(long.MinValue, prefix, postfix)
        {
            orderNumberString = orderNumber;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericOrderIdentifier(long orderNumber, string orderNumberComplete)
            : base(orderNumber)
        {
            orderNumberString = orderNumberComplete;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericOrderIdentifier(string orderNumbercomplete)
            : base(long.MinValue)
        {
            orderNumberString = orderNumbercomplete;
        }

        /// <summary>
        /// Apply the order number and postfix value to the supplied order.
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            base.ApplyTo(order);

            order.ChangeOrderNumber(orderNumberString, orderPrefix, orderPostfix);
        }

        /// <summary>
        /// Apply the order number and postfix to the supplied download detail entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            // using the base behavior
            base.ApplyTo(downloadDetail);

            OrderEntity prototype = new OrderEntity();
            ApplyTo(prototype);

            downloadDetail.ExtraStringData1 = prototype.OrderNumberComplete;
        }
    }
}
