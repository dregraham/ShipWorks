using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Uniquely identifies an Amazon order in the database
    /// </summary>
    public class AmazonOrderIdentifier : OrderIdentifier
    {
        // Amazon's Order ID
        string amazonOrderID = "";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="amazonOrderID"></param>
        public AmazonOrderIdentifier(string amazonOrderID)
        {
            this.amazonOrderID = amazonOrderID;
        }

        /// <summary>
        /// Apply the order number to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            AmazonOrderEntity amazonOrder = order as AmazonOrderEntity;

            if (amazonOrder == null)
            {
                throw new InvalidOperationException("A non Amazon order was passed to the Amazon order identifier.");
            }

            amazonOrder.AmazonOrderID = amazonOrderID;
        }

        /// <summary>
        /// Apply the order number to the download log entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = amazonOrderID;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("AmazonOrderID:{0}", amazonOrderID);
        }
    }
}
