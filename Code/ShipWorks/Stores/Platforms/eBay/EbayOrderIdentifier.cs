using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Locates
    /// </summary>
    public class EbayOrderIdentifier : OrderIdentifier
    {
        long ebayOrderId = 0;
        long ebayItemId = 0;
        long transactionId = 0;

        /// <summary>
        /// The Order ID if it's a combined payment in ebay's system
        /// </summary>
        public long EbayOrderID
        {
            get { return ebayOrderId; }
        }

        /// <summary>
        /// Ebay's Item ID
        /// </summary>
        public long EbayItemID
        {
            get { return ebayItemId; }
        }

        /// <summary>
        /// EBay's transaction ID
        /// </summary>
        public long TransactionID
        {
            get { return transactionId; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOrderIdentifier(long ebayOrderId, long ebayItemId, long transactionId)
        {
            this.ebayOrderId = ebayOrderId;
            this.ebayItemId = ebayItemId;
            this.transactionId = transactionId;
        }

        /// <summary>
        /// Apply the identifier properties to the provided order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            EbayOrderEntity ebayOrder = order as EbayOrderEntity;
            if (ebayOrder == null)
            {
                throw new InvalidOperationException("A non eBay Order was passd to the EbayOrderIdentifier.");
            }

            ebayOrder.EbayOrderID = ebayOrderId;
        }

        /// <summary>
        /// Add details to the downloadDetail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            // apply details 
            downloadDetail.ExtraBigIntData1 = ebayOrderId;
            downloadDetail.ExtraBigIntData2 = ebayItemId;
            downloadDetail.ExtraBigIntData3 = transactionId;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("eBay:{0} ({1}:{2})", ebayItemId, ebayOrderId, transactionId);
        }
    }
}
