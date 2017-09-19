using System;
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
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOrderIdentifier(long ebayOrderId, long ebayItemId, long transactionId)
        {
            EbayOrderID = ebayOrderId;
            EbayItemID = ebayItemId;
            TransactionID = transactionId;
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
                EbayItemID = Int64.Parse(match.Groups[1].Value);
                TransactionID = Int64.Parse(match.Groups[2].Value);
            }
            else
            {
                EbayOrderID = Int64.Parse(orderID);
            }
        }

        /// <summary>
        /// Create the identifier based on itemID and transactionID which are provided as strings from eBay
        /// </summary>
        public EbayOrderIdentifier(string itemID, string transactionID)
        {
            EbayItemID = long.Parse(itemID);
            TransactionID = long.Parse(transactionID);
        }

        /// <summary>
        /// The Order ID if it's a combined payment in ebay's system
        /// </summary>
        public long EbayOrderID { get; }

        /// <summary>
        /// Ebay's Item ID
        /// </summary>
        public long EbayItemID { get; }

        /// <summary>
        /// EBay's transaction ID
        /// </summary>
        public long TransactionID { get; }

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

            ebayOrder.EbayOrderID = EbayOrderID;
        }

        /// <summary>
        /// Add details to the downloadDetail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            // apply details
            downloadDetail.ExtraBigIntData1 = EbayOrderID;
            downloadDetail.ExtraBigIntData2 = EbayItemID;
            downloadDetail.ExtraBigIntData3 = TransactionID;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory)
        {
            // For ebay combined orders (EbayOrderID != 0), we can compare the EbayOrderID
            if (EbayOrderID != 0)
            {
                return CreateCombinedSearchQueryInternal(factory,
                    factory.EbayOrderSearch,
                    EbayOrderSearchFields.OriginalOrderID,
                    EbayOrderSearchFields.EbayOrderID == EbayOrderID);
            }

            // For non-ebay combined orders we have to check the order item fields to see if it's been downloaded
            // already.
            var from = factory.EbayOrderSearch
                              .InnerJoin(factory.OrderSearch)
                              .On(EbayOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID)
                              .InnerJoin(factory.EbayOrderItem)
                              .On(EbayOrderItemFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            return factory.Create()
                .From(from)
                .Select(EbayOrderSearchFields.OriginalOrderID)
                .Where(EbayOrderItemFields.EbayTransactionID == TransactionID & EbayOrderItemFields.EbayItemID == EbayItemID);
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("eBay:{0} ({1}:{2})", EbayItemID, EbayOrderID, TransactionID);
        }
    }
}
