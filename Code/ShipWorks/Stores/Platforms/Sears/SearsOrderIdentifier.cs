using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Class for uniquely identifiying a sears order
    /// </summary>
    public class SearsOrderIdentifier : OrderIdentifier
    {
        long confirmationNumber;
        string poNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsOrderIdentifier(long confirmationNumber, string poNumber)
        {
            this.confirmationNumber = confirmationNumber;
            this.poNumber = poNumber;
        }

        /// <summary>
        /// Apply the unique order information to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            SearsOrderEntity searsOrder = order as SearsOrderEntity;

            if (searsOrder == null)
            {
                throw new InvalidOperationException("A non Sears order was passed to the Sears order identifier.");
            }

            searsOrder.OrderNumber = confirmationNumber;
            searsOrder.PoNumber = poNumber;
        }
        
        /// <summary>
        /// Apply the unique order information to the download detail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.OrderNumber = confirmationNumber;
            downloadDetail.ExtraStringData1 = poNumber;
        }
    }
}
