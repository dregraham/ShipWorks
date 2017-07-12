using System;
using System.Linq;
using System.Text.RegularExpressions;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

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
        /// Constructor
        /// </summary>
        public EbayOrderIdentifier(long ebayOrderId, long ebayItemId, long transactionId)
        {
            this.ebayOrderId = ebayOrderId;
            this.ebayItemId = ebayItemId;
            this.transactionId = transactionId;
        }

        /// <summary>
        /// Create the identifier from the given eBay API object
        /// </summary>
        public EbayOrderIdentifier(string orderID)
        {
            // The - prefix is from a case where eBay had a bug that prefixed items with a -
            Match match = Regex.Match(orderID, @"^-?(\d+)-(\d+)$");

            // "Combined" orders have an Int64 ID, whereas single-line auctions are represented by an ItemID-TransactionID hyphenation
            if (match.Success)
            {
                ebayItemId = Int64.Parse(match.Groups[1].Value);
                transactionId = Int64.Parse(match.Groups[2].Value);
            }
            else
            {
                ebayOrderId = Int64.Parse(orderID);
            }
        }

        /// <summary>
        /// Create the identifier based on itemID and transactionID which are provided as strings from eBay
        /// </summary>
        public EbayOrderIdentifier(string itemID, string transactionID)
        {
            this.ebayItemId = long.Parse(itemID);
            this.transactionId = long.Parse(transactionID);
        }

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
        /// Apply the identifier properties to the provided order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            EbayOrderEntity ebayOrder = order as EbayOrderEntity;
            if (ebayOrder == null)
            {
                throw new InvalidOperationException("A non eBay Order was passed to the EbayOrderIdentifier.");
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
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.EbayOrderSearch.Where(EbayOrderSearchFields.EbayOrderID == EbayOrderID);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("eBay:{0} ({1}:{2})", ebayItemId, ebayOrderId, transactionId);
        }
    }
}
