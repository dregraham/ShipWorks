using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using ShipWorks.Stores.Platforms.PayPal;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping;
using System.Transactions;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using EbayDetailLevelCodeType = ShipWorks.Stores.Platforms.Ebay.WebServices.DetailLevelCodeType;
using ShipWorks.Users.Audit;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Order Downloader for pulling completed auctions from eBay.
    /// </summary>
    class EbayDownloader : StoreDownloader
    {
        enum DownloadMode
        {
            // download Page 1 with sliding criteria
            Standard,

            // download using Pages, with a fixed criteria
            Paging
        }


        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EbayDownloader));

        // the current time according to eBay
        DateTime eBayCurrentDate;

        // total download counter
        int transactionCount = 0;

        // Cache for tracking item paid status to determine if all parts of a combine order are paid
        Dictionary<string, bool> combinedOrderItemsPaidStatus = new Dictionary<string, bool>();

        IEbayWebClient webClient;
        string ebayTokenKey;
        EbayDownloadSummary transactionSummary;

        const string PendingDeleteStatus = "ShipWorks Pending Delete";

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayDownloader(StoreEntity store)
            : this(store, new EbayWebClient())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        public EbayDownloader(StoreEntity store, IEbayWebClient webClient)
            : base(store)
        {
            this.webClient = webClient;
            ebayTokenKey = EbayStore.EBayToken;
        }


        /// <summary>
        /// Convenience property for getting the store as an EbayStoreEntity
        /// </summary>
        private EbayStoreEntity EbayStore
        {
            get { return (EbayStoreEntity)Store; }
        }


        /// <summary>
        /// Returns the number of days to go back when retrieving seller status
        /// </summary>
        public static int DownloadSellingDuration
        {
            get
            {
                int batchSize = InterapptiveOnly.Registry.GetValue("eBayDownloadSellingDuration", 30);

                // restrict to 1..60
                return Math.Min(Math.Max(batchSize, 1), 60);
            }
        }

        /// <summary>
        /// Begin the order download process
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Connecting to eBay...";

                //eBayCurrentDate = GetEbayTime();
                // Get the official eBay time in UTC
                eBayCurrentDate = webClient.GetServerTimeInUtc(ebayTokenKey);

                // configure the date ranges for the transaction and order searches
                // Ending date time range. Leave a 5 minute window to account for lag on ebay's side.
                DateTime rangeEnd = eBayCurrentDate.AddMinutes(-5);

                // limit the ranges to be within our imposed caps
                DateTime rangeCap = rangeEnd.AddDays(-7).AddHours(2);

                // default to going the max of the cap
                DateTime transactionRangeBegin = rangeCap;
                DateTime combinedPayRangeBegin = rangeCap;

                // If any orders exist, go back as far as the last known update
                DateTime? tempDate = GetMaxLastModifiedTransactionTime();
                if (tempDate.HasValue)
                {
                    transactionRangeBegin = tempDate.Value;

                    // Get the smallest downloaded order date, combined or not
                    tempDate = GetMinOrderDate();
                    if (tempDate.HasValue)
                    {
                        combinedPayRangeBegin = tempDate.Value;
                    }
                }

                if (rangeCap > transactionRangeBegin) transactionRangeBegin = rangeCap;
                if (rangeCap > combinedPayRangeBegin) combinedPayRangeBegin = rangeCap;

                // Download individual transactions first
                if (!DownloadTransactions(transactionRangeBegin, rangeEnd))
                {
                    return;
                }

                // Download Combined Payments
                if (!DownloadCombinedPayments(combinedPayRangeBegin, rangeEnd))
                {
                    return;
                }

                // Download my ebay selling to get status and feedback we've left
                if (!DownloadMyEbaySelling())
                {
                    return;
                }

                // download feedback we have received
                if (!DownloadFeedback())
                {
                    return;
                }

                // There is the possibility that orders merged into a combined order and marked for deletion did not get deleted 
                // due to potential deadlocks, so make sure we have deleted any merged orders that have been marked for deletion.
                // Worst case is this also fails and the orders marked for deletion are still present, but the next download
                // cycle cleans them up.
                DeleteOrdersPendingDeletion();

                // done
                Progress.Detail = "Done";
                Progress.PercentComplete = 100;
            }
            catch (EbayException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (PayPalException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// There is the possibility that orders merged into a combined order and marked for deletion did not get deleted 
        /// due to potential deadlocks. This will attempt to delete any orders with a status of "Shipworks Pending Delete". 
        /// </summary>
        private void DeleteOrdersPendingDeletion()
        {
            // Delete any orders with a local status of "ShipWorks Pending Delete" - these are orders that were merged into 
            // a combined order but failed to be deleted during that process (most likely due to being a deadlock victim)
            List<EbayOrderEntity> ordersPendingDeletion = new List<EbayOrderEntity>();

            // searching for a combined payment
            using (SqlAdapter adapter = new SqlAdapter())
            {
                using (EntityCollection<EbayOrderEntity> collection = new EntityCollection<EbayOrderEntity>())
                {
                    RelationPredicateBucket bucket = new RelationPredicateBucket(EbayOrderFields.StoreID == Store.StoreID & EbayOrderFields.LocalStatus == PendingDeleteStatus);
                    adapter.FetchEntityCollection(collection, bucket);
                    ordersPendingDeletion = collection.ToList();
                }
            }

            if (ordersPendingDeletion.Any())
            {
                // We have orders that need to be deleted
                DeleteOrders(ordersPendingDeletion);
            }
        }

        #region Feedback

        /// <summary>
        /// Download all feedback
        /// </summary>
        private bool DownloadFeedback()
        {
            Progress.Detail = "Checking for feedback...";
            Progress.PercentComplete = 0;

            int pagesProcessed = 0;
            int feedbackItemsProcessed = 0;

            // Find out how many pages of feedback we need to download
            EbayDownloadSummary feedbackDownloadSummary = webClient.GetFeedbackSummary(this.ebayTokenKey);

            while (pagesProcessed < feedbackDownloadSummary.NumberOfPages)
            {
                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                int pageToDownload = pagesProcessed + 1;
                List<FeedbackDetailType> feedbackDetails = webClient.GetFeedbackDetails(this.ebayTokenKey, pageToDownload);

                // Process
                if (!ProcessFeedback(feedbackDetails.ToArray(), feedbackItemsProcessed, feedbackDownloadSummary.NumberOfTransactions))
                {
                    break;
                }

                // We're done processing this page of feedback, so increment our counters
                feedbackItemsProcessed += feedbackDetails.Count;
                pagesProcessed++;
            }

            return true;
        }

        /// <summary>
        /// Process all of the recieved feedbacks
        /// </summary>
        private bool ProcessFeedback(FeedbackDetailType[] feedbacks, int processedCount, int totalFeedbackItems)
        {
            // The date to stop looking at feedback, even if more exists
            DateTime? minOrderDate = GetMinOrderDate();
            if (!minOrderDate.HasValue)
            {
                return false;
            }

            DateTime stopDate = (DateTime)minOrderDate.Value;

            // Counts how many times we have seen feedback come through that we have already seen.
            int alreadySeenItCount = 0;

            // just the number of iterations in this call
            int localCount = 0;

            // Go through each feedback returned
            foreach (FeedbackDetailType feedback in feedbacks)
            {
                Progress.Detail = String.Format("Feedback {0} of {1}...", processedCount + localCount + 1, totalFeedbackItems);

                // If this goes back prior to when we want to look for feedback,
                // set the flag to not look for any more.
                if (feedback.CommentTime.ToUniversalTime() < stopDate)
                {
                    // Dont keep going
                    return false;
                }

                // We only care about feedback left towards us
                if (feedback.Role == TradingRoleCodeType.Seller)
                {
                    EbayOrderItemEntity orderItem = RetrieveItem(feedback.TransactionID == null ? 0 : Convert.ToInt64(feedback.TransactionID), Convert.ToInt64(feedback.ItemID), false);

                    // If we still didnt find it, move on
                    if (orderItem == null)
                    {
                        // Move progress along
                        Progress.PercentComplete = Math.Min(100, 100 * (processedCount + localCount++) / totalFeedbackItems);

                        continue;
                    }

                    // get the order
                    EbayOrderEntity ebayOrder = (EbayOrderEntity)orderItem.Order;

                    // If this order already has feedback, then we have gone back far enough.  Only look
                    // at single payments, b\c multiple feedbacks can be left for combined orders (b\c they
                    // can be left for reach auction in the order.
                    if (orderItem.FeedbackReceivedComments.Length > 0 && ebayOrder.EbayOrderID == 0)
                    {
                        alreadySeenItCount++;

                        // Has to be two in a row to stop.  Sometimes the paging will have one come as the last
                        // of one page, and the first of the next.  So it looks like we see it twice.  If we stopped
                        // at that first indication, we'd miss a bunch.
                        if (alreadySeenItCount >= 2)
                        {
                            // Dont keep going
                            return false;
                        }
                    }
                    else
                    {
                        alreadySeenItCount = 0;
                    }

                    // If its same buyer \ same item, leave the feedback.  Too bad eBay doesnt
                    // give the actual item transaction id.
                    // This could happen because the buyer changed their ebay user id
                    // after the checkout, but before leaving feedback.
                    if (ebayOrder.EbayBuyerID != feedback.CommentingUser)
                    {
                        log.ErrorFormat(String.Format("Item {0}, Transaction {1}: Commenting user ({2}) did not match eBayBuyerID ({3}).",
                            feedback.ItemID, feedback.TransactionID, feedback.CommentingUser, ebayOrder.EbayBuyerID));
                    }

                    // Save the feedback
                    orderItem.FeedbackReceivedType = (int)feedback.CommentType;
                    orderItem.FeedbackReceivedComments = feedback.CommentText;

                    // save the order item
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveEntity(orderItem);
                    }
                }

                // Move progress along
                Progress.PercentComplete = Math.Min(100, 100 * (processedCount + localCount++) / totalFeedbackItems);
            }

            // Keep processing
            return true;
        }

        #endregion

        #region MyEbayStatus


        /// <summary>
        /// Download data for My eBay Selling
        /// </summary>
        private bool DownloadMyEbaySelling()
        {
            // If there are any sales to download
            Progress.PercentComplete = 0;
            Progress.Detail = "Checking My eBay status...";

            // These are going to be our progress counters
            int pagesProcessed = 0;
            int numberOfSalesProcessed = 0;

            // Fetch data indicating the number of pages and how many transactions there are to download
            EbayDownloadSummary downloadSummary = webClient.GetSoldItemsSummary(this.ebayTokenKey, EbayDownloader.DownloadSellingDuration);

            // Download and process each page of sold items
            while (pagesProcessed < downloadSummary.NumberOfPages)
            {
                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Download the next page of items
                int pageToDownload = pagesProcessed + 1;
                List<OrderTransactionType> orderTransactions = webClient.GetSoldItems(this.ebayTokenKey, EbayDownloader.DownloadSellingDuration, pageToDownload);

                // Process the orders transactions
                foreach (OrderTransactionType orderType in orderTransactions)
                {
                    // update the status
                    Progress.Detail = String.Format("My eBay status {0} of {1}...", numberOfSalesProcessed + 1, downloadSummary.NumberOfTransactions);

                    ProcessMyEbaySellingStatus(orderType);

                    // update the progress
                    Progress.PercentComplete = Math.Min(100, 100 * numberOfSalesProcessed++ / downloadSummary.NumberOfTransactions);
                }


                // Increment the number of pages processed
                pagesProcessed++;
            }

            return true;
        }

        /// <summary>
        /// Update the status of the orders that correspond to the given OrderType
        /// </summary>
        private void ProcessMyEbaySellingStatus(OrderTransactionType orderType)
        {
            TransactionType[] transactions;
            List<long> uniqueOrders = new List<long>();

            // Transactions will be in different containers depending on if combined or not
            if (orderType.Order != null)
            {
                transactions = orderType.Order.TransactionArray;

                long orderID = Convert.ToInt64(orderType.Order.OrderID);

                if (!uniqueOrders.Contains(orderID))
                {
                    uniqueOrders.Add(orderID);
                }
            }
            else
            {
                transactions = new TransactionType[] { orderType.Transaction };
            }


            // Go through and update each transaction
            foreach (TransactionType transaction in transactions)
            {
                long itemID = Convert.ToInt64(transaction.Item.ItemID);
                long transactionID = Convert.ToInt64(transaction.TransactionID);

                EbayOrderItemEntity ebayItem = RetrieveItem(transactionID, itemID, false);
                if (ebayItem == null)
                {
                    continue;
                }

                // get the order
                EbayOrderEntity ebayOrder = (EbayOrderEntity)ebayItem.Order;

                // See if its marked as paid
                if (EbayUtility.IsPaidStatusPaid(transaction.SellerPaidStatus))
                {
                    // If its not a combined order, or each item in the combined order has to be paid
                    if (ebayOrder.EbayOrderID == 0 || AreAllItemsInCombinedOrderPaid(ebayOrder, transaction))
                    {
                        ebayItem.MyEbayPaid = true;
                        ebayItem.SellerPaidStatus = (int)transaction.SellerPaidStatus;
                    }
                }

                // See if we have left feedback (and didnt know about it)
                if (transaction.FeedbackLeft != null)
                {
                    if (ebayItem.FeedbackLeftType == (int)EbayFeedbackType.None ||
                        ebayItem.FeedbackLeftType == (int)EbayFeedbackType.Unknown)
                    {
                        ebayItem.FeedbackLeftType = (int)EbayUtility.GetShipWorksFeedbackType(transaction.FeedbackLeft.CommentType);
                    }
                }

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveEntity(ebayItem);
                }
            }
        }

        /// <summary>
        /// Determine if each order item in the given combined order has been paid.
        /// </summary>
        private bool AreAllItemsInCombinedOrderPaid(EbayOrderEntity order, TransactionType transaction)
        {
            // Add this item as one we have seen
            combinedOrderItemsPaidStatus[transaction.Item.ItemID + "_" + transaction.TransactionID] = EbayUtility.IsPaidStatusPaid(transaction.SellerPaidStatus);

            // Go through each item in the order
            foreach (EbayOrderItemEntity item in order.OrderItems)
            {
                bool paid = false;
                if (combinedOrderItemsPaidStatus.TryGetValue(item.EbayItemID + "_" + item.EbayTransactionID, out paid))
                {
                    if (!paid)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Combined Payments

        /// <summary>
        /// Moves any Note entities from one order to another.  No saving is performed.
        /// </summary>
        private static List<NoteEntity> GetNoteCopies(OrderEntity fromOrder, OrderEntity toOrder)
        {
            List<NoteEntity> newNotes = null;

            // find the notes to be moved
            using (SqlAdapter adapter = new SqlAdapter())
            {
                using (EntityCollection<NoteEntity> oldOrderNotes = new EntityCollection<NoteEntity>())
                {
                    adapter.FetchEntityCollection(oldOrderNotes, new RelationPredicateBucket(NoteFields.ObjectID == fromOrder.OrderID));

                    // make clones
                    newNotes = EntityUtility.CloneEntityCollection(oldOrderNotes);

                    // make adjustments to the clones
                    foreach (NoteEntity note in newNotes)
                    {
                        MarkAsNew(note);
                        note.Order = toOrder;
                    }
                }
            }

            return newNotes;
        }

        /// <summary>
        /// Moves any Shipment entities from one order to another, but doesn't save back to the db
        /// </summary>
        private static void CopyShipments(OrderEntity oldOrder, OrderEntity newOrder, List<ShipmentEntity> shipmentsToDelete, List<ShipmentEntity> newShipments)
        {
            // Copy any existing shipments
            foreach (ShipmentEntity shipment in ShippingManager.GetShipments(oldOrder.OrderID, false))
            {
                // load all carrier and customs data
                ShippingManager.EnsureShipmentLoaded(shipment);

                // clone the entity tree
                ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment, true);

                // this is now a new shipment to be inserted
                MarkAsNew(clonedShipment);

                // this is now a shipment for the new order
                clonedShipment.Order = newOrder;

                // all carrier data is New as well
                clonedShipment.GetDependingRelatedEntities().ForEach(e => MarkAsNew(e));
                foreach (ShipmentCustomsItemEntity customsItem in clonedShipment.CustomsItems)
                {
                    MarkAsNew(customsItem);
                }

                // track the shipment to be deleted 
                shipmentsToDelete.Add(shipment);

                // track the shipment to be added
                newShipments.Add(clonedShipment);
            }
        }

        private bool DownloadCombinedPayments(DateTime beginDate, DateTime endDate)
        {
            Progress.PercentComplete = 0;
            Progress.Detail = "Downloading combined payments...";

            List<OrderType> allCombined = new List<OrderType>();
            TimeSpan totalDuration = endDate.Subtract(beginDate);

            // For performance purposes on eBay's side, we don't want to download the entire data set for the given date range
            // so we're going to only download two day's of data at a time
            DateTime currentBegin = beginDate;
            DateTime currentEnd = currentBegin;

            while (currentEnd < endDate)
            {
                // Increment our date range
                currentBegin = currentEnd;
                currentEnd = currentEnd.AddDays(1);

                if (currentEnd > endDate)
                {
                    // We've gone too far. Back up to the end date - this will be our last increment
                    currentEnd = endDate;
                }

                // Download the current increment and update the progress based on the total number of 
                // days we've downloaded so far
                allCombined.AddRange(DownloadPaymentIncrement(currentBegin, currentEnd));
                Progress.PercentComplete = Math.Min(100 * currentEnd.Subtract(beginDate).Days / Math.Max(totalDuration.Days, 1), 100);

                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }
            }

            // We get all orders every time.  Most of them probably will not have changed.  So the first
            // thing we do is remove all orders that have not changed since we last saw them.
            RemoveUnchangedCombinedPayments(allCombined);

            // Set the progress based on how many we have to look at
            Progress.Detail = "Processing combined payments...";
            Progress.PercentComplete = 0;

            // Go through each combined payment order and find all sales
            // that were not downloaded.
            for (int i = 0; i < allCombined.Count; i++)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                Progress.Detail = string.Format("Combined payment {0} of {1}...", i + 1, allCombined.Count);
                
                if (!ProcessCombinedPayment(allCombined[i]))
                {
                    return false;
                }

                Progress.PercentComplete = Math.Min(100, 100 * i / allCombined.Count);
            }

            return true;
        }

        /// <summary>
        /// Download and process all combined payments in the given range
        /// </summary>
        private List<OrderType> DownloadPaymentIncrement(DateTime beginDate, DateTime endDate)
        {
            List<OrderType> allCombined = new List<OrderType>();

            // Get the summary information so we know how many pages to request
            EbayDownloadSummary downloadSummary = webClient.GetPaymentSummary(this.ebayTokenKey, beginDate, endDate);
            int pagesDownloaded = 0;

            while (pagesDownloaded < downloadSummary.NumberOfPages && !Progress.IsCancelRequested)
            {
                // Download the next page of items
                int pageToDownload = pagesDownloaded + 1;

                // Make one service call to get all the orders rather than two separate calls; eBay docs say getting all orders returns 
                // only orders in an active or completed state, but filter out any that are not in an active or completed status as a precaution
                List<OrderType> payments = webClient.GetAllPayments(this.ebayTokenKey, beginDate, endDate, pageToDownload)
                                                    .Where(p => p.OrderStatus == OrderStatusCodeType.Active || p.OrderStatus == OrderStatusCodeType.Completed).ToList();

                foreach (OrderType order in payments)
                {
                    // On 11/8/2011 hell broke loose when eBay started sending down single transactions in the GetOrders response
                    if (order.OrderID != "0" && order.TransactionArray.Length > 1)
                    {
                        allCombined.Add(order);
                    }
                }

                pagesDownloaded++;
            }

            return allCombined;
        }

        /// <summary>
        /// Handles the processing of Combined Payments (Orders) from eBay
        /// </summary>
        private bool ProcessCombinedPayment(OrderType combinedPayment)
        {
            // Get the order for this combined order
            EbayOrderEntity order = (EbayOrderEntity)InstantiateOrder(new EbayOrderIdentifier(Convert.ToInt64(combinedPayment.OrderID), 0, 0));

            // assign an order number
            AssignOrderNumber(order);

            // Keep track of all orders that need deleted
            List<EbayOrderEntity> oldOrders = new List<EbayOrderEntity>();

            // Keep track of shipments that have been moved to thew new order from the old order
            List<ShipmentEntity> shipmentsToDelete = new List<ShipmentEntity>();
            List<ShipmentEntity> shipmentsToSave = new List<ShipmentEntity>();

            // Keep track of notes tha thave been moved to the new order from the old order
            List<NoteEntity> notesToSave = new List<NoteEntity>();

            // track if any presumed half.com transactions have been seen
            // accounts have been seen
            bool halfComTransactions = false;
            bool suspendedAccounts = false;

            // Go through each sale in the order
            foreach (TransactionType transaction in combinedPayment.TransactionArray)
            {
                if (transaction.Item == null)
                {
                    log.InfoFormat("Encountered a NULL transaction.Item in combinedPayment.TransactionArray for eBay Order ID {0}.  Skipping.", combinedPayment.OrderID);
                    continue;
                }

                // Ensure this item exists in the DB first
                EbayOrderItemEntity orderItem = RetrieveItem(Convert.ToInt64(transaction.TransactionID), Convert.ToInt64(transaction.Item.ItemID), true);
                if (orderItem == null)
                {
                    TransactionType fullTransaction = null;
                    try
                    {
                        fullTransaction = webClient.GetTransactionDetails(this.ebayTokenKey, transaction.Item.ItemID, transaction.TransactionID);
                    }
                    catch (EbayException ex)
                    {
                        // Item not found, we just ignore it in the combining
                        if (ex.ErrorCode == "17" || ex.ErrorCode == "21917182")
                        {
                            continue;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    if (fullTransaction == null)
                    {
                        log.InfoFormat("Skipping eBay TransactionID {0}, ItemID {1} for eBay OrderID {2}, presumed to be a Half.com order.", transaction.TransactionID, transaction.Item.ItemID, order.EbayOrderID);
                        halfComTransactions = true;
                        continue;
                    }

                    if (fullTransaction.Buyer.Status == UserStatusCodeType.Suspended)
                    {
                        log.InfoFormat("Skipping eBay TransactionID {0}, ItemID {1} for eBay OrderID {2}; the account of buyer {3} is suspended.", transaction.TransactionID, transaction.Item.ItemID, order.EbayOrderID, fullTransaction.Buyer.UserID);
                        suspendedAccounts = true;
                        continue;
                    }

                    try
                    {
                        // GetTransactionDetails does not fill in the item
                        fullTransaction.Item = webClient.GetItemDetails(this.ebayTokenKey, transaction.Item.ItemID);
                    }
                    catch (EbayException ex)
                    {
                        // The item details could also throw an eBay exception, so we need to catch it here as well
                        // Item not found, we just ignore it in the combining
                        if (ex.ErrorCode == "17" || ex.ErrorCode == "21917182")
                        {
                            continue;
                        }
                        else
                        {
                            throw;
                        }
                    }

                    try
                    {
                        if (!ProcessTransaction(fullTransaction))
                        {
                            return false;
                        }
                    }
                    catch (EbayMissingGspReferenceException exception)
                    {
                        // Skip any orders that failed to missing a GSP reference number, so the user isn't
                        // stuck waiting on eBay to fix the problem and can continue downloading other orders 
                        log.WarnFormat("Skipping eBay transaction {0}. {1}", fullTransaction.TransactionID, exception.Message);
                    }

                    // Now this will exist
                    orderItem = RetrieveItem(Convert.ToInt64(transaction.TransactionID), Convert.ToInt64(transaction.Item.ItemID), true);
                }

                // See if this item currently belongs to a different order
                if (orderItem.OrderID != order.OrderID)
                {
                    #region Merge old order into this one

                    EbayOrderEntity oldOrder = (EbayOrderEntity)orderItem.Order;

                    // Ensure the item now goes with this order
                    // clone the old order item
                    CopyOrderItemToOrder(orderItem, order);

                    // If we have already seen and dealt with this old order, just move on.  This happens
                    // when multiple old items actually belong to the same old order.
                    if (oldOrders.Any(o => o.OrderID == oldOrder.OrderID))
                    {
                        continue;
                    }

                    // We have to get the order items to calculate the weight
                    List<OrderItemEntity> orderItems = GetManualOrderItems(oldOrder.OrderID);
                    orderItems.ForEach(i => { CopyOrderItemToOrder(i, order); });

                    // Update the order properties with the older-order's info
                    order.EbayBuyerID = oldOrder.EbayBuyerID;
                    order.ShipEmail = oldOrder.ShipEmail;
                    order.BillEmail = oldOrder.BillEmail;

                    order.BuyerFeedbackScore = oldOrder.BuyerFeedbackScore;
                    order.BuyerFeedbackPrivate = oldOrder.BuyerFeedbackPrivate;

                    // move any notes associated with the old order
                    notesToSave.AddRange(GetNoteCopies(oldOrder, order));

                    // move any shipments associated with the old order
                    CopyShipments(oldOrder, order, shipmentsToDelete, shipmentsToSave);

                    // The __blank is necessary to determine if two status' dont match b\c one was blank
                    string oldStatus = (oldOrder.LocalStatus.Length == 0) ? "__blank" : oldOrder.LocalStatus;

                    // If not set yet, just set it
                    if (order.LocalStatus.Length == 0)
                    {
                        order.LocalStatus = oldStatus;
                    }
                    // If it doesnt match a previous old order, set to combined
                    else if (order.LocalStatus != oldOrder.LocalStatus)
                    {
                        order.LocalStatus = "Combined";
                    }

                    // IF all previous status was blank, then set the new one to blank
                    if (order.LocalStatus == "__blank")
                    {
                        order.LocalStatus = "";
                    }

                    // track the seen order
                    oldOrders.Add(oldOrder);

                    #endregion
                }

                // Update checkout details for the order
                UpdateCheckoutDetails(orderItem, combinedPayment.CheckoutStatus);
            }

            // The only known cause of this would be if all of the transactions are half.com or transactions were skipped because accounts were suspended
            if (order.OrderItems.Count == 0 && (halfComTransactions || suspendedAccounts))
            {
                log.InfoFormat("Skipping saving eBay Order ID {0}, there were no eBay items associated (appeared to be all Half.com) or the buyer's account was suspended", order.EbayOrderID);
                return true;
            }


            // Update the info for the order
            UpdateOrderAddress(order, combinedPayment.ShippingAddress);
            UpdateOrder(order, combinedPayment);

            // Update charges for the order
            UpdateCharges(order, combinedPayment);

            // Make sure we have the latest GSP data
            UpdateGlobalShippingProgramInfo(order, combinedPayment.IsMultiLegShipping, combinedPayment.MultiLegShippingDetails);

            // Make totals adjustments
            double amount = combinedPayment.AmountPaid != null ? combinedPayment.AmountPaid.Value : combinedPayment.Total.Value;
            BalanceOrderTotal(order, amount);

            // Save everything
            SaveCombinedOrder(order, oldOrders, shipmentsToDelete, shipmentsToSave, notesToSave);

            return true;
        }

        /// <summary>
        /// Gets the manual order items, fully loaded, for a given order id
        /// </summary>
        private List<OrderItemEntity> GetManualOrderItems(long orderID)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                using (EntityCollection<OrderItemEntity> collection = new EntityCollection<OrderItemEntity>())
                {
                    RelationPredicateBucket bucket = new RelationPredicateBucket(OrderFields.StoreID == EbayStore.StoreID);
                    bucket.Relations.Add(OrderEntity.Relations.OrderItemEntityUsingOrderID);
                    bucket.PredicateExpression.Add(OrderItemFields.OrderID == orderID & OrderItemFields.IsManual == true);

                    // include attributes because we are going to do a deep copy
                    PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderItemEntity);
                    prefetch.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

                    adapter.FetchEntityCollection(collection, bucket, prefetch);

                    return collection.ToList();
                }
            }
        }

        /// <summary>
        /// Performs the saving of all combined order data
        /// </summary>
        private void SaveCombinedOrder(EbayOrderEntity order, List<EbayOrderEntity> oldOrders, List<ShipmentEntity> shipmentsToDelete, List<ShipmentEntity> shipmentsToSave, List<NoteEntity> notesToSave)
        {
            // retrieve configuration for auditing settings
            ConfigurationEntity config = ConfigurationData.Fetch();

            // first mark all of the old orders as pending delete
            Dictionary<EbayOrderEntity, string> statusMap = new Dictionary<EbayOrderEntity, string>();
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // quickly update all statuses, remembering the old statuses
                foreach (EbayOrderEntity oldOrder in oldOrders)
                {
                    // save the old order status in case of failure
                    statusMap[oldOrder] = oldOrder.LocalStatus;

                    // update status
                    oldOrder.LocalStatus = PendingDeleteStatus;

                    // save it
                    adapter.SaveEntity(oldOrder, false, false);
                }

                // commit the transaction
                adapter.Commit();
            }

            try
            {
                // Only audit new orders if new order auditing is turned on.  This also turns off auditing of creating of new customers if the order is not new.
                using (AuditBehaviorScope auditScope = new AuditBehaviorScope((config.AuditNewOrders || !order.IsNew) ? AuditBehaviorDisabledState.Default : AuditBehaviorDisabledState.Disabled))
                {
                    // Save everything we've modified about the order and related entities
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // delete old shipments
                        shipmentsToDelete.ForEach(s => ShippingManager.DeleteShipment(s));

                        // Synch this order with the database
                        SaveDownloadedOrder(order);

                        // save copied notes
                        notesToSave.ForEach(n => NoteManager.SaveNote(n));

                        // save copied shipments
                        shipmentsToSave.ForEach(s => ShippingManager.SaveShipment(s));

                        // commit the transaction
                        adapter.Commit();
                    }
                }
            }
            catch
            {
                // revert oldOrder statuses
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    foreach (EbayOrderEntity oldOrder in oldOrders)
                    {
                        // revert its status
                        oldOrder.LocalStatus = statusMap[oldOrder];

                        // save
                        adapter.SaveEntity(oldOrder, false, false);
                    }

                    // commit the transaction
                    adapter.Commit();
                }

                // Rethrow the exception now that the statuses have been reverted (and bypass deleting the old orders)
                throw;
            }

            // Now delete the old orders outside of the transaction for saving the combined orders to avoid deadlocks
            DeleteOrders(oldOrders);
        }

        /// <summary>
        /// Since we're deleting orders in multiple places.A method to encapsulate the deletion of orders along 
        /// with logging any orders that failed to be deleted
        /// </summary>
        /// <param name="ordersToDelete">The orders to delete.</param>
        private static void DeleteOrders(List<EbayOrderEntity> ordersToDelete)
        {
            List<EbayOrderEntity> failedDeletes = DeleteOrdersWithRetries(ordersToDelete, 10);
            if (failedDeletes.Count > 0)
            {
                // Note: those orders that failt to be deleted here will be left having a local status of "ShipWorks Pending Delete"
                string idList = String.Join(",", failedDeletes.Select(o => o.OrderID).ToArray());
                string orderNumberList = String.Join(",", failedDeletes.Select(o => o.OrderNumberComplete).ToArray());

                log.ErrorFormat("The following Order IDs failed to delete after 10 retries: {0},=", idList);
                log.ErrorFormat("These correspond to Order Numbers: {0}", orderNumberList);
            }
        }

        /// <summary>
        /// Repeatedly attempts to delete the orders in the ordersToDelete list.  There are retryCount attempts made.
        /// </summary>
        private static List<EbayOrderEntity> DeleteOrdersWithRetries(List<EbayOrderEntity> ordersToDelete, int retryCount)
        {
            List<EbayOrderEntity> failedDeletes = new List<EbayOrderEntity>();

            foreach (EbayOrderEntity order in ordersToDelete)
            {
                try
                {
                    DeletionService.DeleteOrder(order.OrderID);
                }
                catch (TransactionAbortedException)
                {
                    log.WarnFormat("Transaction aborted while deleting order id '{0}', most likely due to deadlock.", order.OrderID);
                    failedDeletes.Add(order);
                }
            }

            if (retryCount > 0 && failedDeletes.Count > 0)
            {
                return DeleteOrdersWithRetries(failedDeletes, --retryCount);
            }
            else
            {
                return failedDeletes;
            }
        }

        /// <summary>
        /// Copies an order item to a new order.
        /// </summary>
        private static void CopyOrderItemToOrder(OrderItemEntity orderItem, EbayOrderEntity order)
        {
            OrderItemEntity clonedItem = EntityUtility.CloneEntity(orderItem, true);
            MarkAsNew(clonedItem);

            // mark the cloned items as New
            foreach (OrderItemAttributeEntity attribute in clonedItem.OrderItemAttributes)
            {
                MarkAsNew(attribute);
            }

            // assign the item to this order
            clonedItem.Order = order;
        }

        /// <summary>
        /// Mark all fields as changed for LLBL's sake
        /// </summary>
        private static void MarkAsNew(IEntity2 entity)
        {
            entity.IsNew = true;

            foreach (IEntityField2 field in entity.Fields)
            {
                field.IsChanged = true;
            }

            entity.GetDependingRelatedEntities().ForEach(e => MarkAsNew(e));

            entity.GetMemberEntityCollections().ForEach(c =>
            {
                foreach (IEntity2 e2 in c)
                {
                    MarkAsNew(e2);
                }
            });
        }

        /// <summary>
        /// Returns the array necessary to request full detail from eBay.  Fully qualified because PayPal has an identical class.
        /// </summary>
        private EbayDetailLevelCodeType[] GetFullDetailLevel()
        {
            return new EbayDetailLevelCodeType[] { EbayDetailLevelCodeType.ReturnAll };
        }

        /// <summary>
        /// Update charges for the eBay Order
        /// </summary>
        private void UpdateCharges(EbayOrderEntity order, OrderType combinedPayment)
        {
            #region Shipping

            // Shipping
            OrderChargeEntity shipping = GetCharge(order, "SHIPPING", true);
            shipping.Description = "Shipping";

            // Calculation for shipping depends on if this is a combined payment or not
            decimal shippingAmount = 0;

            if (combinedPayment.ShippingServiceSelected != null && combinedPayment.ShippingServiceSelected.ShippingServiceCost != null)
            {
                shippingAmount = (decimal)combinedPayment.ShippingServiceSelected.ShippingServiceCost.Value;
            }
            else
            {
                // Saw this null for linens & laurel
                AmountType cost = null;

                if (combinedPayment.ShippingDetails.ShippingServiceOptions != null && combinedPayment.ShippingDetails.ShippingServiceOptions.Count() > 0)
                {
                    cost = combinedPayment.ShippingDetails.ShippingServiceOptions[0].ShippingServiceCost;
                }
                else if (combinedPayment.ShippingDetails.InternationalShippingServiceOption != null && combinedPayment.ShippingDetails.InternationalShippingServiceOption.Count() > 0)
                {
                    cost = combinedPayment.ShippingDetails.InternationalShippingServiceOption[0].ShippingServiceCost;
                }

                if (cost != null)
                {
                    shippingAmount = (decimal)cost.Value;
                }
            }

            // Store the new shipping amount
            shipping.Amount = shippingAmount;

            #endregion

            #region Adjustment

            // Adjustment
            decimal adjustment = (decimal)combinedPayment.AdjustmentAmount.Value;

            // Apply the adjustment
            if (adjustment != 0)
            {
                OrderChargeEntity adjust = GetCharge(order, "ADJUST", true);
                adjust.Description = "Adjustment";
                adjust.Amount = adjustment;
            }

            #endregion

            #region Insurance

            decimal insuranceTotal = 0;

            // Use insurance
            if (combinedPayment.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required ||
                combinedPayment.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Optional)
            {
                if ((combinedPayment.ShippingDetails.InsuranceWanted || combinedPayment.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required))
                {
                    if (combinedPayment.ShippingServiceSelected != null && combinedPayment.ShippingServiceSelected.ShippingInsuranceCost != null)
                    {
                        insuranceTotal = (decimal)combinedPayment.ShippingServiceSelected.ShippingInsuranceCost.Value;
                    }
                    else if (combinedPayment.ShippingDetails.InsuranceFee != null)
                    {
                        insuranceTotal = (decimal)combinedPayment.ShippingDetails.InsuranceFee.Value;
                    }
                }
            }

            if (insuranceTotal != 0)
            {
                OrderChargeEntity insurance = GetCharge(order, "INSURANCE", true);
                insurance.Description = "Insurance";
                insurance.Amount = insuranceTotal;
            }

            #endregion

            #region Sales Tax

            decimal salesTax = 0m;

            if (combinedPayment.ShippingDetails.SalesTax != null && combinedPayment.ShippingDetails.SalesTax.SalesTaxAmount != null)
            {
                salesTax = (decimal)combinedPayment.ShippingDetails.SalesTax.SalesTaxAmount.Value;
            }

            // Tax
            OrderChargeEntity tax = GetCharge(order, "TAX", true);
            tax.Description = "Sales Tax";
            tax.Amount = salesTax;

            #endregion
        }

        /// <summary>
        /// Updates the Charges for an order
        /// </summary>
        private void UpdateCharges(EbayOrderEntity order, TransactionType transaction)
        {
            // Only do the line items once the checkout is complete
            #region Shipping

            // Shipping
            OrderChargeEntity shipping = GetCharge(order, "SHIPPING", true);
            shipping.Description = "Shipping";

            // Store the new shipping amount
            shipping.Amount = IsShippingInfoValid(transaction) ? (decimal)transaction.ShippingServiceSelected.ShippingServiceCost.Value : 0m;

            #endregion

            #region Adjustment

            // Adjustment
            decimal adjustment = (decimal)transaction.AdjustmentAmount.Value;

            // Apply the adjustment
            if (adjustment != 0)
            {
                OrderChargeEntity adjust = GetCharge(order, "ADJUST", true);
                adjust.Description = "Adjustment";
                adjust.Amount = adjustment;
            }

            #endregion

            #region Insurance

            decimal insuranceTotal = 0;

            // Use insurance
            if (transaction.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required ||
                transaction.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Optional)
            {
                if ((transaction.ShippingDetails.InsuranceWanted || transaction.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required))
                {
                    // Fee may not be specified yet if checkout is not complete
                    if (IsShippingInfoValid(transaction) && transaction.ShippingServiceSelected.ShippingInsuranceCost != null)
                    {
                        insuranceTotal = (decimal)transaction.ShippingServiceSelected.ShippingInsuranceCost.Value;
                    }
                    else if (transaction.ShippingDetails.InsuranceFee != null)
                    {
                        insuranceTotal = (decimal)transaction.ShippingDetails.InsuranceFee.Value;
                    }
                }
            }

            if (insuranceTotal != 0)
            {
                OrderChargeEntity insurance = GetCharge(order, "INSURANCE", true);
                insurance.Description = "Insurance";
                insurance.Amount = insuranceTotal;
            }

            #endregion

            #region Sales Tax

            decimal salesTax = 0m;

            if (transaction.ShippingDetails.SalesTax.SalesTaxAmount != null && transaction.ShippingDetails.SalesTax.SalesTaxAmount != null)
            {
                salesTax = (decimal)transaction.ShippingDetails.SalesTax.SalesTaxAmount.Value;
            }

            // Tax
            OrderChargeEntity tax = GetCharge(order, "TAX", true);
            tax.Description = "Sales Tax";
            tax.Amount = salesTax;

            #endregion
        }

        /// <summary>
        /// Remove all eBayOrders for which we already have an internal order, and the last modified
        /// time has not changed.
        /// </summary>
        private void RemoveUnchangedCombinedPayments(List<OrderType> combinedPayments)
        {
            combinedPayments.RemoveAll(o =>
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    RelationCollection relations = new RelationCollection();
                    relations.Add(EbayOrderEntity.Relations.GetSuperTypeRelation());

                    // query the database to see if the order has changed since we last saw it
                    object result = adapter.GetScalar(OrderFields.OnlineLastModified,
                        null, AggregateFunction.None,
                        EbayOrderFields.EbayOrderID == Convert.ToInt64(o.OrderID) & OrderFields.StoreID == Store.StoreID, null, relations);

                    DateTime? dateTime = result is DBNull ? null : (DateTime?)result;

                    if (dateTime.HasValue && dateTime == o.CheckoutStatus.LastModifiedTime)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };
            });
        }

        /// <summary>
        /// Update order properties from an Order
        /// </summary>
        private void UpdateOrder(EbayOrderEntity order, OrderType combinedPayment)
        {
            order.EbayOrderID = Convert.ToInt64(combinedPayment.OrderID);

            // Update the order properties
            order.OrderTotal = (decimal)combinedPayment.Total.Value;

            // don't re-set the order date if the transaction's date is less because
            // when working with Combined Orders, this happens often because each component of 
            // the combined order has a different Create Date.  Allowing this to be set here will
            // cause lots of audit entries as the value flip-flops on downloads 
            if (order.OrderDate < combinedPayment.CreatedTime)
            {
                order.OrderDate = combinedPayment.CreatedTime;
            }

            // same for OnlineLastModified
            if (order.OnlineLastModified < combinedPayment.CheckoutStatus.LastModifiedTime)
            {
                order.OnlineLastModified = combinedPayment.CheckoutStatus.LastModifiedTime;
            }

            if (order.RequestedShipping == null)
            {
                order.RequestedShipping = "";
            }

            if (combinedPayment.CheckoutStatus.Status == CompleteStatusCodeType.Complete)
            {
                if (combinedPayment.ShippingServiceSelected != null)
                {
                    order.RequestedShipping = EbayUtility.GetShipmentMethodName(combinedPayment.ShippingServiceSelected.ShippingService);
                }
                else
                {
                    if (combinedPayment.ShippingDetails.ShippingServiceOptions != null)
                    {
                        order.RequestedShipping = EbayUtility.GetShipmentMethodName(combinedPayment.ShippingDetails.ShippingServiceOptions[0].ShippingService);
                    }
                    else if (combinedPayment.ShippingDetails.InternationalShippingServiceOption != null)
                    {
                        order.RequestedShipping = EbayUtility.GetShipmentMethodName(combinedPayment.ShippingDetails.InternationalShippingServiceOption[0].ShippingService);
                    }
                }
            }

            foreach (TransactionType transaction in combinedPayment.TransactionArray)
            {
                EbayOrderItemEntity ebayItem = (EbayOrderItemEntity)order.OrderItems.FirstOrDefault(item =>
                {
                    if (item.IsManual)
                    {
                        return false;
                    }
                    else if (transaction.Item == null)
                    {
                        return false;
                    }
                    else
                    {
                        EbayOrderItemEntity eItem = (EbayOrderItemEntity)item;

                        return eItem.EbayTransactionID == Convert.ToInt64(transaction.TransactionID) && eItem.EbayItemID == Convert.ToInt64(transaction.Item.ItemID);
                    }
                });

                if (ebayItem == null)
                {
                    // Half.com transactions could be in here, so they wouldn't have been found in our database.  Skip it and move on.
                    continue;
                }

                // See if the transaction has been shipped and paid for
                if (transaction.ShippedTimeSpecified) ebayItem.MyEbayShipped = true;
                if (transaction.PaidTimeSpecified) ebayItem.MyEbayPaid = true;

                if (combinedPayment.ShippingDetails.SellingManagerSalesRecordNumberSpecified)
                {
                    ebayItem.SellingManagerRecord = combinedPayment.ShippingDetails.SellingManagerSalesRecordNumber;
                }

                // see if we need to get the paypal info
                if (EbayStore.DownloadPayPalDetails && ebayItem.PaymentMethod == (int)BuyerPaymentMethodCodeType.PayPal)
                {
                    // I havce seen a case where TransactionArray had nothing in it.  Not sure why this would happen.
                    if (ebayItem.PayPalTransactionID.Length == 0 && combinedPayment.TransactionArray.Length > 0)
                    {
                        // TODO: may not need to do this every single time.... 
                        ebayItem.PayPalTransactionID = FindPayPalTransactionID(
                            order.OrderDate,
                            combinedPayment.TransactionArray[0].Item.ItemID,
                            order.BillEmail,
                            order.ShipLastName,
                            order.OrderTotal);
                    }

                    if (ebayItem.PayPalTransactionID.Length > 0)
                    {
                        UpdatePayPalDetails(ebayItem);
                    }
                }
            }
        }

        /// <summary>
        /// Update checkout details for the order
        /// </summary>
        private void UpdateCheckoutDetails(EbayOrderItemEntity orderItem, CheckoutStatusType status)
        {
            orderItem.PaymentStatus = (int)status.eBayPaymentStatus;
            orderItem.PaymentMethod = (int)status.PaymentMethod;

            orderItem.CompleteStatus = (int)status.Status;
            orderItem.CheckoutStatus = (int)((status.Status == CompleteStatusCodeType.Complete) ?
                CheckoutStatusCodeType.CheckoutComplete : CheckoutStatusCodeType.CheckoutIncomplete);
        }

        #endregion

        #region Transactions

        /// <summary>
        /// Returns the most recent non-ebay-order ShipWorks order.
        /// </summary>
        private EbayOrderItemEntity GetMostRecentTransactionItem()
        {
            DateTime? lastModified = GetMaxLastModifiedTransactionTime();
            if (lastModified.HasValue)
            {
                // Fetch the order along with the item
                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.EbayOrderItemEntity);
                prefetch.Add(EbayOrderItemEntity.PrefetchPathOrder);

                // configure the criteria
                RelationPredicateBucket bucket = new RelationPredicateBucket();
                bucket.Relations.Add(EbayOrderItemEntity.Relations.OrderEntityUsingOrderID);
                bucket.PredicateExpression.Add(OrderFields.OnlineLastModified == lastModified.Value &
                                                OrderFields.StoreID == Store.StoreID &
                                                EbayOrderFields.EbayOrderID == 0 &
                                                OrderFields.IsManual == false);

                using (EbayOrderItemCollection matching = new EbayOrderItemCollection())
                {
                    SqlAdapter.Default.FetchEntityCollection(matching, bucket, prefetch);

                    return matching.FirstOrDefault();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets whether or not a transaction download cycle can be skipped.
        /// </summary>
        private bool CanSkipDownload(GetSellerTransactionsResponseType countResponse)
        {
            // if we pull a single transaction that matches what we know to be the most recent transaction, it can be skipped
            if (countResponse.PaginationResult.TotalNumberOfPages == 1 && countResponse.TransactionArray != null)
            {
                // peek at the results and see if this 1 transaction exactly matches the most recent transaction in the database.
                string transactionID = countResponse.TransactionArray[0].TransactionID;
                string itemID = countResponse.TransactionArray[0].Item.ItemID;
                DateTime lastModifiedTime = countResponse.TransactionArray[0].Status.LastTimeModified;

                // find the most recent transaction in our database to compare against
                EbayOrderItemEntity lastKnownTransaction = GetMostRecentTransactionItem();
                if (lastKnownTransaction != null)
                {
                    // compare the last known to the data pulled from ebay.  If it all matches, we don't need to download this single transaction
                    if (String.Compare(lastKnownTransaction.EbayTransactionID.ToString(), transactionID, StringComparison.OrdinalIgnoreCase) == 0 &&
                        String.Compare(lastKnownTransaction.EbayItemID.ToString(), itemID, StringComparison.OrdinalIgnoreCase) == 0 &&
                        lastKnownTransaction.Order.OnlineLastModified == lastModifiedTime)
                    {
                        // Everything important matches, so we can skip this transaction download cycle
                        return true;
                    }
                }
            }

            // cannot skip
            return false;
        }

        /// <summary>
        /// Download eBay transactions that fall within the provided range
        /// </summary>
        private bool DownloadTransactions(DateTime beginRange, DateTime rangeEnd)
        {
            Progress.PercentComplete = 0;
            Progress.Detail = string.Format("Checking for orders...");

            // Fetch the metadata for downloading orders with the given date range
            transactionSummary = webClient.GetTransactionSummary(this.ebayTokenKey, beginRange, rangeEnd);
            transactionCount = transactionSummary.NumberOfTransactions;
            int pageNumber = 1;
            int pagesDownloaded = 0;

            // if there are no new transactions to download, just return true so the remainder of the download process continues
            if (transactionCount > 0)
            {
                bool hasMoreTransactions = true;

                // Track which transactions have already been processed in this download cycle
                Dictionary<string, bool> processedCache = new Dictionary<string, bool>();

                // We're going to be moving the start date forward with each download, so we're just
                // going to continue to download until eBay says there aren't any more transactions                
                while (hasMoreTransactions)
                {
                    // Check for cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }

                    // We're going to be sliding the start date forward, so we always want to download the first page
                    GetSellerTransactionsResponseType getTransactionsResponse = webClient.DownloadTransactions(this.ebayTokenKey, beginRange, rangeEnd, pageNumber);
                    hasMoreTransactions = getTransactionsResponse.HasMoreTransactions;
                    pagesDownloaded++;

                    // the transactions returned by the current iteration of the download loop
                    TransactionType[] transactions = getTransactionsResponse.TransactionArray;
                    if (transactions != null)
                    {
                        // IMPORTANT - sort the transactions in ascending order. eBay reps say GetSellerTransactions returns them ascending, but doing this to be safe
                        Array.Sort(transactions, (a, b) => a.Status.LastTimeModified.CompareTo(b.Status.LastTimeModified));
                    }

                    // again due to the ebay quirks, must verify there are transactions
                    if (!ProcessDownloadedTransactions(transactions, processedCache))
                    {
                        return false;
                    }

                    if (transactions != null && transactions.Length > 0)
                    {
                        // Adjust the start date to the time of the last modified date so we don't
                        // miss any transactions due to last modified date being altered after the
                        // initial download process started
                        beginRange = transactions.Last().Status.LastTimeModified.AddSeconds(1);

                        // Make sure the page number is reset to 1 since we're able to use the sliding 
                        // date range since we have orders in the arrays.
                        pageNumber = 1;
                    }
                    else
                    {
                        if (hasMoreTransactions)
                        {
                            // This is to account for a pretty sweet situation where eBay says there are X number of 
                            // transactions, but that number includes empty and non-empty transactions. The empty
                            // transactions are excluded in the response received from eBay which could result in 
                            // the scenario where the transactions array is empty, but there are additional transactions
                            // to be downloaded, so we'll increment the page number in this case since there isn't the 
                            // chance of missing orders due to last modified dates changing.
                            pageNumber++;
                        }
                    }

                    // update progress - we're updating the percentage based on pages downlaoded, since we don't have 
                    // a gaurantee that the number of transaction in the summary are the actual number of transactions 
                    // downloaded processed due to the empty transaction sweetness described above.
                    Progress.PercentComplete = Math.Min(100 * pagesDownloaded / transactionSummary.NumberOfPages, 100);
                }
            }

            return true;
        }


        /// <summary>
        /// Determines if all of the transactions have the same Modified Time
        /// </summary>
        private static bool AllSameTime(TransactionType[] transactions)
        {
            if (transactions == null || transactions.Length == 0)
            {
                return true;
            }

            // just get the first ModTime
            DateTime modTime = transactions.First().Status.LastTimeModified;

            // find out if they all have the same Mod time
            return transactions.All(t => t.Status.LastTimeModified == modTime);
        }

        /// <summary>
        /// Gets the processedCache key for the given transaction
        /// </summary>
        private static string GetHashKey(TransactionType transaction)
        {
            // the key is the ItemID and TransactionID (combined, uniquely identify an auction sale) along with the modified time
            return string.Format("{0}_{1}_{2}", transaction.Item.ItemID, transaction.TransactionID, transaction.Status.LastTimeModified.Ticks);
        }

        /// <summary>
        /// Returns if a transaction has already been processed during the download cycle
        /// </summary>
        private static bool AlreadyProcessed(Dictionary<string, bool> processedCache, TransactionType transaction)
        {
            return processedCache.ContainsKey(GetHashKey(transaction));
        }

        /// <summary>
        /// Record a transaction as having been processed
        /// </summary>
        private static void AddToProcessedCache(Dictionary<string, bool> processedCache, TransactionType transaction)
        {
            processedCache[GetHashKey(transaction)] = true;
        }

        /// <summary>
        /// Process a collection of downloaded eBay transactions
        /// </summary>
        private bool ProcessDownloadedTransactions(TransactionType[] downloadedTransactions, Dictionary<string, bool> processedCache)
        {
            // if there were no transactions, return that they were all successfully processed
            if (downloadedTransactions == null || downloadedTransactions.Length == 0)
            {
                return true;
            }

            foreach (TransactionType transaction in downloadedTransactions)
            {
                // check for user cancellation
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                if (transaction.Buyer.Status != UserStatusCodeType.Suspended)
                {
                    // if this one has been processed, just skip it
                    if (AlreadyProcessed(processedCache, transaction))
                    {
                        continue;
                    }

                    // update the download count
                    Progress.Detail = String.Format("Processing order {0}...", (QuantitySaved + 1));

                    try
                    {
                        if (!ProcessTransaction(transaction))
                        {
                            return false;
                        }
                    }
                    catch (EbayMissingGspReferenceException exception)
                    {
                        // Skip any orders that failed to missing a GSP reference number, so the user isn't
                        // stuck waiting on eBay to fix the problem and  can continue downloading other orders.
                        // The order should be downloaded when the reference number is updated (i.e. the modified 
                        // date is changed)
                        log.WarnFormat("Skipping eBay transaction {0}. {1}", transaction.TransactionID, exception.Message);
                    }

                    // note that this transaction has been processed
                    AddToProcessedCache(processedCache, transaction);
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the shipping information on the transaction is good
        /// </summary>
        private bool IsShippingInfoValid(TransactionType transaction)
        {
            if (transaction.ShippingServiceSelected.ShippingServiceCost == null)
            {
                return false;
            }

            if (transaction.Status.BuyerSelectedShipping)
            {
                return true;
            }

            // If checkout is complete, i guess we have to just assume its valid
            if (transaction.Status.CompleteStatus == CompleteStatusCodeType.Complete)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Update the order entity with information from an ebay transaction
        /// </summary>
        private void UpdateOrder(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            EbayOrderEntity order = (EbayOrderEntity)orderItem.Order;

            order.OrderTotal = Convert.ToDecimal(transaction.AmountPaid.Value);

            // don't re-set the order date if the transaction's date is less because
            // when working with Combined Orders, this happens often because each component of 
            // the combined order has a different Create Date.  Allowing this to be set here will
            // cause lots of audit entries as the value flip-flops on downloads 
            if (order.OrderDate < transaction.CreatedDate)
            {
                order.OrderDate = transaction.CreatedDate;
            }

            // same for Last Modified
            if (order.OnlineLastModified < transaction.Status.LastTimeModified)
            {
                order.OnlineLastModified = transaction.Status.LastTimeModified;
            }

            // Requested shipping, but not if the transaction is a part of a combined payment
            // because ebay can send incorrect shipping values here.  The correct value will get picked up 
            // when processing combined payments.  Always loading Requested Shipping here is known to cause flip/flopping
            // of Requested Shipping values each time the download runs.
            if (order.EbayOrderID == 0)
            {
                order.RequestedShipping = EbayUtility.GetShipmentMethodName(IsShippingInfoValid(transaction) ? transaction.ShippingServiceSelected.ShippingService : "");
            }

            order.EbayBuyerID = transaction.Buyer.UserID;

            // if it has been longer than 14 days, ebay does not give us a real email
            string email = transaction.Buyer.Email;
            if (email == "Invalid Request")
            {
                email = "";
            }

            if (email.Length > 0)
            {
                order.ShipEmail = email;
                order.BillEmail = email;
            }

            order.BuyerFeedbackScore = transaction.Buyer.FeedbackScore;
            order.BuyerFeedbackPrivate = transaction.Buyer.FeedbackPrivate;

            // See if paid or shipped are specified
            if (transaction.ShippedTimeSpecified) orderItem.MyEbayShipped = true;
            if (transaction.PaidTimeSpecified) orderItem.MyEbayPaid = true;

            // Save the selling manager record.
            if (transaction.ShippingDetails.SellingManagerSalesRecordNumberSpecified)
            {
                orderItem.SellingManagerRecord = transaction.ShippingDetails.SellingManagerSalesRecordNumber;
            }

            // If external transaction is present, and its for paypal
            if (transaction.ExternalTransaction != null && transaction.ExternalTransaction.Length > 0)
            {
                if (orderItem.PaymentMethod == (int)BuyerPaymentMethodCodeType.PayPal)
                {
                    foreach (ExternalTransactionType external in transaction.ExternalTransaction)
                    {
                        // SIS is an invalid value we have seen. So we will take the first External Transaction
                        // which isn't "SIS"
                        if (String.CompareOrdinal(external.ExternalTransactionID, "SIS") != 0)
                        {
                            orderItem.PayPalTransactionID = external.ExternalTransactionID;
                        }
                    }
                }
            }

            // see if we need to download paypal details
            if (EbayStore.DownloadPayPalDetails && orderItem.PaymentMethod == (int)BuyerPaymentMethodCodeType.PayPal)
            {
                // if we don't have the transaction id yet, try to find it
                if (orderItem.PayPalTransactionID.Length == 0)
                {
                    orderItem.PayPalTransactionID = FindPayPalTransactionID(order.OrderDate,
                        transaction.Item.ItemID,
                        transaction.Buyer.Email,
                        order.ShipLastName,
                        order.OrderTotal);
                }

                if (orderItem.PayPalTransactionID.Length > 0)
                {
                    try
                    {
                        UpdatePayPalDetails(orderItem);
                    }
                    catch (PayPalException)
                    {
                        // Transaction ID must have been bad, search again
                        orderItem.PayPalTransactionID = FindPayPalTransactionID(order.OrderDate,
                            transaction.Item.ItemID,
                            transaction.Buyer.Email,
                            order.ShipLastName,
                            order.OrderTotal);

                        if (orderItem.PayPalTransactionID.Length > 0)
                        {
                            UpdatePayPalDetails(orderItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the paypal address status, and adds any paypal-sourced notes to the 
        /// order.
        /// </summary>
        private void UpdatePayPalDetails(EbayOrderItemEntity orderItem)
        {
            // If we already have the address status, we dont need to do this
            if (orderItem.PayPalAddressStatus != 0)
            {
                return;
            }

            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = orderItem.PayPalTransactionID;

            try
            {
                PayPalWebClient client = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));
                GetTransactionDetailsResponseType response = (GetTransactionDetailsResponseType)client.ExecuteRequest(request);

                orderItem.PayPalAddressStatus = (int)response.PaymentTransactionDetails.PayerInfo.Address.AddressStatus;

                // TODO: need to specify which item it's for
                string notes = response.PaymentTransactionDetails.PaymentItemInfo.Memo;
                if (notes.Length > 0)
                {
                    InstantiateNote(orderItem.Order, notes, response.PaymentTransactionDetails.PaymentInfo.PaymentDate, NoteVisibility.Public);
                }
            }
            catch (PayPalException ex)
            {
                // 10007 means you didn't have permission to get the details of the transaction
                if (ex.Errors.Any(e => e.Code == "10007"))
                {
                    log.ErrorFormat("eBay had a correct transaction ({0}) but insufficient PayPal priveleges to get data for it.", orderItem.PayPalTransactionID);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the processing of a single eBay transaction
        /// </summary>
        private bool ProcessTransaction(TransactionType transaction)
        {
            long itemId = Convert.ToInt64(transaction.Item.ItemID);
            long transactionId = Convert.ToInt64(transaction.TransactionID);

            EbayOrderEntity order = (EbayOrderEntity)InstantiateOrder(new EbayOrderIdentifier(0, itemId, transactionId));
            EbayOrderItemEntity orderItem = FindItem(order, transactionId, itemId, true);

            // give it an order number
            AssignOrderNumber(order);

            // update general item details
            if (orderItem.IsNew)
            {
                // only update item details if it's new.  V2 checked the Allow Edit setting
                // but we don't have such a concept in V3.
                UpdateOrderItem(orderItem, transaction);
                LoadNotes(order, transaction);
            }

            if (!IsCombinedPayment(order))
            {
                UpdateCheckoutDetails(orderItem, transaction.Status);
                UpdateOrderAddress(order, transaction.Buyer.BuyerInfo.ShippingAddress);

                // update general order properties
                UpdateCharges(order, transaction);
            }


            UpdateOrder(orderItem, transaction);
            BalanceOrderTotal(order, transaction.AmountPaid.Value);

            // Make sure we have the latest GSP data
            UpdateGlobalShippingProgramInfo(order, transaction.IsMultiLegShipping, transaction.MultiLegShippingDetails);

            // Save the order
            SaveDownloadedOrder(order);

            return true;
        }

        /// <summary>
        /// Loads the notes from the buyer transaction.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="transaction">The transaction.</param>
        private void LoadNotes(EbayOrderEntity order, TransactionType transaction)
        {
            if (!string.IsNullOrEmpty(transaction.BuyerCheckoutMessage))
            {
                InstantiateNote(order, transaction.BuyerCheckoutMessage, transaction.CreatedDate, NoteVisibility.Public);
            }
        }



        /// <summary>
        /// Reconciles the ShipWorks order total with what eBay has on record.
        /// Any adjustment are done via an OTHER charge
        /// </summary>
        private void BalanceOrderTotal(EbayOrderEntity order, double amountPaid)
        {
            if (order.OrderItems.Where(item => item is EbayOrderItemEntity).All(item =>
            {
                EbayOrderItemEntity ebayItem = (EbayOrderItemEntity)item;
                return ebayItem.MyEbayShipped;
            }))
            {
                bool resetStatus = order.IsNew;

                if (!resetStatus)
                {
                    // only assing LocalStatus if there are no processed shipments
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        RelationPredicateBucket attBucket = new RelationPredicateBucket(ShipmentFields.OrderID == order.OrderID & ShipmentFields.Processed == true);

                        // only allow the status to be reset if there are no processed shipments
                        resetStatus = (adapter.GetDbCount(new ShipmentEntityFactory().CreateFields(), attBucket)) == 0;
                    }
                }

                // reset the LocalStatus if allowed
                if (resetStatus)
                {
                    order.LocalStatus = "Shipped";
                }
            }

            order.OrderTotal = OrderUtility.CalculateTotal(order);

            if (order.OrderTotal != Convert.ToDecimal(amountPaid))
            {
                // only make adjustments if all items are Complete
                if (order.OrderItems.Where(item => item is EbayOrderItemEntity).All(item => ((EbayOrderItemEntity)item).CompleteStatus == (int)CompleteStatusCodeType.Complete))
                {
                    OrderChargeEntity otherCharge = GetCharge(order, "OTHER", true);
                    otherCharge.Description = "Other";
                    otherCharge.Amount += Convert.ToDecimal(amountPaid) - order.OrderTotal;
                }
            }
        }

        /// <summary>
        /// Gets the shipping address from the downloaded order.
        /// </summary>
        /// <param name="transaction">The downloaded order.</param>
        /// <returns>An AddressType object.</returns>
        private WebServices.AddressType GetShippingAddress(OrderType downloadedOrder)
        {
            WebServices.AddressType shippingAddress = downloadedOrder.ShippingAddress;

            if (downloadedOrder.IsMultiLegShipping)
            {
                // This is to be shipped via eBay's Global Shipping Program, so we 
                // need to pull the shipping address out of here
                shippingAddress = downloadedOrder.MultiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress;
            }

            return shippingAddress;
        }


        /// <summary>
        /// Updates the global shipping program info.
        /// </summary>
        /// <param name="ebayOrder">The ebay order.</param>
        /// <param name="isGlobalShippingProgramOrder">if set to <c>true</c> [is global shipping program order].</param>
        /// <param name="multiLegShippingDetails">The multi leg shipping details.</param>
        /// <exception cref="EbayException">eBay did not provide a reference ID for an order designated for the Global Shipping Program.</exception>
        private void UpdateGlobalShippingProgramInfo(EbayOrderEntity ebayOrder, bool isGlobalShippingProgramOrder, MultiLegShippingDetailsType multiLegShippingDetails)
        {
            ebayOrder.GspEligible = isGlobalShippingProgramOrder;

            if (ebayOrder.GspEligible)
            {
                // This is part of the global shipping program, so we need to pull out the address info 
                // of the international shipping provider but first make sure there aren't any null 
                // objects in the address heirarchy
                if (multiLegShippingDetails != null && multiLegShippingDetails.SellerShipmentToLogisticsProvider != null
                    && multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress != null)
                {
                    // Pull out the name of the international shipping provider
                    PersonName name = PersonName.Parse(multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Name);
                    ebayOrder.GspFirstName = name.First;
                    ebayOrder.GspLastName = name.Last;

                    // Address info                    

                    // eBay includes "Suite 400" as part of street1 which some shipping carriers (UPS) don't recognize as a valid address.
                    // So, we'll try to split the street1 line (1850 Airport Exchange Blvd, Suite 400) into separate addresses based on 
                    // the presence of a comma in street 1

                    // We're ultimately going to populate the ebayOrder.GspStreet property values based on the elements in th streetLines list
                    List<string> streetLines = new List<string>
                    {
                        // Default the list to empty strings for the case where the Street1 and Street2 
                        // properties of the shipping address are null
                        multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 ?? string.Empty,
                        multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street2 ?? string.Empty
                    };

                    if (multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 != null)
                    {
                        // Try to split the Street1 property based on comma
                        List<string> splitStreetInfo = multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1.Split(new char[] { ',' }).ToList();

                        // We'll always have at least one value in the result of the split which will be our value for street1
                        streetLines[0] = splitStreetInfo[0];

                        if (splitStreetInfo.Count > 1)
                        {
                            // There were multiple components to the original Street1 address provided by eBay; this second
                            // component will be the value we use for our street 2 address instead of the value provided by eBay
                            streetLines[1] = splitStreetInfo[1].Trim();
                        }
                    }

                    ebayOrder.GspStreet1 = streetLines[0].Trim();
                    ebayOrder.GspStreet2 = streetLines[1].Trim();

                    ebayOrder.GspCity = multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.CityName ?? string.Empty;
                    ebayOrder.GspStateProvince = Geography.GetStateProvCode(multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.StateOrProvince) ?? string.Empty;
                    ebayOrder.GspPostalCode = multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.PostalCode ?? string.Empty;
                    ebayOrder.GspCountryCode = Enum.GetName(typeof(ShipWorks.Stores.Platforms.Ebay.WebServices.CountryCodeType), multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Country);

                    // Pull out the reference ID that will identify the order to the international shipping provider
                    ebayOrder.GspReferenceID = multiLegShippingDetails.SellerShipmentToLogisticsProvider.ShipToAddress.ReferenceID;


                    if (ebayOrder.GspPostalCode.Length >= 5)
                    {
                        // Only grab the first five digits of the postal code; there have been incidents in the past where eBay 
                        // sends down an invalid 9 digit postal code (e.g. 41018-319) that prevents orders from being shipped
                        ebayOrder.GspPostalCode = ebayOrder.GspPostalCode.Substring(0, 5);
                    }



                    if (ebayOrder.IsNew || ebayOrder.SelectedShippingMethod != (int)EbayShippingMethod.DirectToBuyerOverridden)
                    {
                        // The seller has the choice to NOT ship GSP, so only default the shipping program to GSP for new orders
                        // or orders if the selected shipping method has not been manually overridden
                        ebayOrder.SelectedShippingMethod = (int)EbayShippingMethod.GlobalShippingProgram;
                    }
                }

                if (string.IsNullOrEmpty(ebayOrder.GspReferenceID))
                {
                    // We can't necessarily reference an ID number here since the ShipWorks order ID may not be assigned yet,
                    // so we'll reference the order date and the buyer that made the purchase
                    string message = string.Format("eBay did not provide a reference ID for an order designated for the Global Shipping Program. The order was placed on {0} from buyer {1}.",
                                        StringUtility.FormatFriendlyDateTime(ebayOrder.OrderDate), ebayOrder.BillUnparsedName);

                    throw new EbayMissingGspReferenceException(message);
                }
            }
            else
            {
                // This isn't a GSP order, so we're going to wipe the GSP data from the order in the event that
                // an order was previously marked as a GSP, but is no longer for some reason

                ebayOrder.GspFirstName = string.Empty;
                ebayOrder.GspLastName = string.Empty;

                // Address info
                ebayOrder.GspStreet1 = string.Empty;
                ebayOrder.GspStreet2 = string.Empty;
                ebayOrder.GspCity = string.Empty;
                ebayOrder.GspStateProvince = string.Empty;
                ebayOrder.GspPostalCode = string.Empty;
                ebayOrder.GspCountryCode = string.Empty;

                // Reset the reference ID and the shipping method to standard
                ebayOrder.GspReferenceID = string.Empty;

                if (ebayOrder.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
                {
                    // Only change the status if it has not been previously overridden; due to the individual transactions being downloaded 
                    // first then the combined orders being downloaded, this would inadvertently get set back to GSP if the combined order is
                    // a GSP order (if the same buyer purchases one item that is GSP and another that isn't, the GSP settings get applied
                    // to the combined order).
                    ebayOrder.SelectedShippingMethod = (int)EbayShippingMethod.DirectToBuyer;
                }
            }
        }

        /// <summary>
        /// Update the address of the ebay order with the address infomration provided
        /// </summary>
        private void UpdateOrderAddress(EbayOrderEntity order, ShipWorks.Stores.Platforms.Ebay.WebServices.AddressType address)
        {
            // Split the name
            PersonName personName = PersonName.Parse(address.Name);

            order.ShipNameParseStatus = (int)personName.ParseStatus;
            order.ShipUnparsedName = personName.UnparsedName;
            order.ShipCompany = address.CompanyName ?? "";
            order.ShipFirstName = personName.First;
            order.ShipMiddleName = personName.Middle;
            order.ShipLastName = personName.Last;
            order.ShipStreet1 = address.Street1 ?? "";
            order.ShipStreet2 = address.Street2 ?? "";
            order.ShipCity = address.CityName ?? "";
            order.ShipStateProvCode = address.StateOrProvince == null ? string.Empty : Geography.GetStateProvCode(address.StateOrProvince) ?? ""; 
            order.ShipPostalCode = address.PostalCode ?? "";
            order.ShipPhone = address.Phone ?? "";
            order.ShipCountryCode = Enum.GetName(typeof(ShipWorks.Stores.Platforms.Ebay.WebServices.CountryCodeType), address.Country);


            // Fill in billing address from the shipping
            PersonAdapter.Copy(order, "Ship", order, "Bill");
        }

        /// <summary>
        /// Get the specified charge for the order
        /// </summary>
        private OrderChargeEntity GetCharge(OrderEntity order, string type, bool autoCreate)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Type == type);

            if (orderCharge == null)
            {
                if (autoCreate)
                {
                    // create a new one
                    orderCharge = new OrderChargeEntity();
                    orderCharge.Order = order;
                    orderCharge.Type = type;
                }
            }

            return orderCharge;
        }



        /// <summary>
        /// Updates the various ebay checkout status on the order
        /// </summary>
        private void UpdateCheckoutDetails(EbayOrderItemEntity orderItem, TransactionStatusType status)
        {
            orderItem.PaymentStatus = (int)status.eBayPaymentStatus;
            orderItem.PaymentMethod = (int)status.PaymentMethodUsed;

            orderItem.CheckoutStatus = (int)status.CheckoutStatus;
            orderItem.CompleteStatus = (int)status.CompleteStatus;
        }

        /// <summary>
        /// Determines if this order is a combined ebay payment or not
        /// </summary>
        private bool IsCombinedPayment(EbayOrderEntity order)
        {
            return order.EbayOrderID != 0;
        }

        /// <summary>
        /// Locates an item
        /// </summary>
        private EbayOrderItemEntity FindItem(EbayOrderEntity order, TransactionType transaction, bool autoCreate)
        {
            return FindItem(order, Convert.ToInt64(transaction.TransactionID), Convert.ToInt64(transaction.Item.ItemID), autoCreate);
        }

        /// <summary>
        /// Locates an item
        /// </summary>
        private EbayOrderItemEntity RetrieveItem(long transactionID, long itemID, bool includeOrderItemAttributes)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                EntityCollection<EbayOrderItemEntity> collection = new EntityCollection<EbayOrderItemEntity>();

                RelationPredicateBucket bucket = new RelationPredicateBucket(OrderFields.StoreID == EbayStore.StoreID);
                bucket.Relations.Add(OrderEntity.Relations.OrderItemEntityUsingOrderID);

                if (itemID > 0)
                {
                    bucket.PredicateExpression.Add(EbayOrderItemFields.EbayItemID == itemID);
                }

                if (transactionID > 0)
                {
                    bucket.PredicateExpression.Add(EbayOrderItemFields.EbayTransactionID == transactionID);
                }

                // sometimes we need want to retrieve the order at the same time
                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderItemEntity);

                // always including the order
                prefetch.Add(OrderItemEntity.PrefetchPathOrder);

                // conditionally include the attributes
                if (includeOrderItemAttributes)
                {
                    prefetch.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);
                }

                adapter.FetchEntityCollection(collection, bucket, prefetch);

                return collection.FirstOrDefault();
            }
        }

        /// <summary>
        /// Locates an item
        /// </summary>
        private EbayOrderItemEntity FindItem(EbayOrderEntity order, long transactionID, long itemID, bool autoCreate)
        {
            EbayOrderItemEntity orderItem = order.OrderItems.Where(i => i is EbayOrderItemEntity).Cast<EbayOrderItemEntity>().Where(i => i.EbayItemID == itemID && i.EbayTransactionID == transactionID).FirstOrDefault();

            if (orderItem == null && autoCreate)
            {
                orderItem = (EbayOrderItemEntity)InstantiateOrderItem(order);
                orderItem.LocalEbayOrderID = order.OrderID;
                orderItem.EbayItemID = itemID;
                orderItem.EbayTransactionID = transactionID;
            }

            return orderItem;
        }

        /// <summary>
        /// Update an order item
        /// </summary>
        private void UpdateOrderItem(EbayOrderItemEntity item, TransactionType transaction)
        {
            // Set the important properties
            item.Code = item.EbayItemID.ToString();
            item.UnitPrice = (decimal)transaction.TransactionPrice.Value;
            item.Quantity = transaction.QuantityPurchased;
            item.Name = transaction.Item.Title;

            // See if SKU is present
            if (transaction.Item.SKU != null)
            {
                item.SKU = transaction.Item.SKU;
            }

            // an eBay change no longer placed variation information in the item title.  We need to pull it from the Variation node if possible.
            if (transaction.Variation != null)
            {
                if (!String.IsNullOrEmpty(transaction.Variation.VariationTitle))
                {
                    item.Name = transaction.Variation.VariationTitle;
                }

                // Overwrite the SKU if a variation SKU is provided
                if (!string.IsNullOrEmpty(transaction.Variation.SKU))
                {
                    item.SKU = transaction.Variation.SKU;
                }

                // if this is a new item and one with variations, we can add attributes now
                if (item.IsNew)
                {
                    if (transaction.Variation.VariationSpecifics != null)
                    {
                        // create attributes for the variations
                        foreach (NameValueListType variation in transaction.Variation.VariationSpecifics)
                        {
                            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                            attribute.Name = variation.Name;
                            attribute.Description = String.Join("; ", variation.Value);
                            attribute.UnitPrice = 0;
                        }
                    }
                }
            }

            // See if selling manager stuff is present
            if (transaction.SellingManagerProductDetails != null)
            {
                item.SellingManagerProductName = transaction.SellingManagerProductDetails.ProductName ?? "";
                item.SellingManagerProductPart = transaction.SellingManagerProductDetails.ProductID.ToString();
            }

            // See if calc shipping is present for weight
            if (transaction.ShippingDetails.CalculatedShippingRate != null)
            {
                ShipWorks.Stores.Platforms.Ebay.WebServices.MeasureType weightMajor = transaction.ShippingDetails.CalculatedShippingRate.WeightMajor;
                ShipWorks.Stores.Platforms.Ebay.WebServices.MeasureType weightMinor = transaction.ShippingDetails.CalculatedShippingRate.WeightMinor;

                if (weightMajor != null && weightMinor != null)
                {
                    if (weightMajor.unit == "POUNDS" || weightMajor.unit == "lbs")
                    {
                        item.Weight = (double)(weightMajor.Value + weightMinor.Value / 16.0m);
                    }
                }
            }

            // Save the selling manager record.
            if (transaction.ShippingDetails.SellingManagerSalesRecordNumberSpecified)
            {
                item.SellingManagerRecord = transaction.ShippingDetails.SellingManagerSalesRecordNumber;
            }

            // If we don't have the image yet, we have to do GetItem
            if (item.Image.Length == 0 && EbayStore.DownloadItemDetails)
            {
                try
                {
                    ItemType eBayItem = webClient.GetItemDetails(this.ebayTokenKey, item.EbayItemID.ToString());
                    PictureDetailsType pictureDetails = eBayItem.PictureDetails;

                    if (pictureDetails != null && pictureDetails.PictureURL != null && pictureDetails.PictureURL.Length > 0)
                    {
                        item.Image = eBayItem.PictureDetails.PictureURL[0] ?? "";
                        item.Thumbnail = item.Image;
                    }

                    // If still no image, see if there is a stock image
                    if (item.Image.Length == 0)
                    {
                        if (eBayItem.ProductListingDetails != null)
                        {
                            if (eBayItem.ProductListingDetails.IncludeStockPhotoURL)
                            {
                                item.Image = eBayItem.ProductListingDetails.StockPhotoURL ?? "";
                                item.Thumbnail = item.Image;
                            }
                        }
                    }
                }
                catch (EbayException exception)
                {
                    // Check if we get error code 17 (the item has been deleted). eBay deletes items in certain
                    // situations where the items falls outside of their user agreeements (counterfeit, selling illegal/trademarked items, etc.)
                    if (exception.ErrorCode == "17" || exception.ErrorCode == "21917182")
                    {
                        // Just log this exception otherwise the download process will not complete
                        log.WarnFormat("eBay returned an error when trying to download item details for item {0} (item ID {1}, transaction ID {2}). {3}", item.Name, item.EbayItemID, item.EbayTransactionID, exception.Message);
                        log.WarnFormat("The seller may need to contact eBay support to determine why the order item may have been deleted.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        #endregion

        #region PayPal

        /// <summary>
        /// Search for a paypal transaction that matches up with these payment details
        /// </summary>
        private string FindPayPalTransactionID(DateTime start, string auction, string payerEmail, string payerLast, decimal amount)
        {
            TransactionSearchRequestType search = new TransactionSearchRequestType();

            search.StartDate = start;
            search.AuctionItemNumber = auction;

            if (payerEmail.Length > 0)
            {
                search.Payer = payerEmail;
            }

            // Perform the search
            TransactionSearchResponseType response;
            try
            {
                PayPalWebClient paypalClient = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));

                response = (TransactionSearchResponseType)paypalClient.ExecuteRequest(search);

                if (response.PaymentTransactions == null)
                {
                    // no transaction found that matches the criteria
                    return "";
                }

                // consider only those with a payer specified
                List<PaymentTransactionSearchResultType> candidates = response.PaymentTransactions.Where(p => p.Payer.Length > 0).ToList();
                List<PaymentTransactionSearchResultType> matches = new List<PaymentTransactionSearchResultType>();

                // now find a match based on amount and email
                foreach (PaymentTransactionSearchResultType candidate in candidates)
                {
                    decimal grossAmount = Math.Abs(Convert.ToDecimal(candidate.GrossAmount.Value));

                    if (grossAmount == amount)
                    {
                        if (candidate.Payer == payerEmail)
                        {
                            matches.Add(candidate);
                        }
                        else if (payerLast.Length > 0 && candidate.PayerDisplayName.IndexOf(" " + payerLast, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            matches.Add(candidate);
                        }
                    }
                }

                // single match, must be the one we're looking for
                if (matches.Count == 1)
                {
                    // single reuslt
                    return matches[0].TransactionID;
                }

                // try to further narrow it down, filter out Updates
                matches.RemoveAll(p => p.Type != "Payment");

                // Did we remove them all
                if (matches.Count == 0)
                {
                    return "";
                }

                // again return if there is only one
                if (matches.Count == 1)
                {
                    return matches[0].TransactionID;
                }

                // saw a case where we had multiple, but all were the same transaction ID
                if (matches.All(p => p.TransactionID == matches[0].TransactionID))
                {
                    return matches[0].TransactionID;
                }

                if (matches.Count > 1)
                {
                    // log it
                    log.ErrorFormat("Unable to determine PayPal transaction ID, multiple PayPal transactions for {0}", payerEmail);
                }

                return "";
            }
            catch (PayPalException ex)
            {
                // log the error
                log.Error("Failed to retrieve PayPal transactions for buyer " + payerEmail, ex);
                return "";
            }
        }

        #endregion

        /// <summary>
        /// Gets the largest last modified time we have in our database for non-manual orders for this store.
        /// If no such orders exist, null is returned.
        /// </summary>
        protected DateTime? GetMaxLastModifiedTransactionTime()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OnlineLastModified,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false &
                    EbayOrderFields.EbayOrderID == 0,
                    null,
                    new RelationCollection(OrderEntity.Relations.GetSubTypeRelation("EbayOrderEntity"))
                    );

                DateTime? dateTime = result is DBNull ? null : (DateTime?)result;

                log.InfoFormat("MAX(LastModifiedTransactionTime) = {0:u}", dateTime);

                // If we don't have a max, but do have a days-back policy, use the days back
                if (dateTime == null && Store.InitialDownloadDays != null)
                {
                    // Also add on 2 hours just to make sure we are in range
                    dateTime = DateTime.UtcNow.AddDays(-Store.InitialDownloadDays.Value).AddHours(2);
                    log.InfoFormat("MAX(LastModifiedTransactionTime) adjusted by download policy = {0:u}", dateTime);
                }

                return dateTime;
            }
        }

        /// <summary>
        /// Gets the smallest order date, combined or not, for this store in the database
        /// </summary>
        private DateTime? GetMinOrderDate()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderDate,
                    null, AggregateFunction.Min,
                    OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result is DBNull ? null : (DateTime?)result;

                log.InfoFormat("MIN(OrderDate) = {0:u}", dateTime);

                return dateTime;
            }
        }

        /// <summary>
        /// Locate an order with an OrderIdentifier
        /// </summary>
        protected override OrderEntity FindOrder(OrderIdentifier orderIdentifier)
        {
            EbayOrderIdentifier ebayOrderIdentifier = orderIdentifier as EbayOrderIdentifier;
            if (ebayOrderIdentifier == null)
            {
                throw new InvalidOperationException("OrderIdentifier of type EbayOrderIdentifier expected.");
            }

            long ebayItemID = ebayOrderIdentifier.EbayItemID;
            long transactionID = ebayOrderIdentifier.TransactionID;
            long ebayOrderID = ebayOrderIdentifier.EbayOrderID;

            if (ebayOrderID == 0)
            {
                // Look for any existing EbayOrderItem with matching ebayItemID and TransactionID
                // it's parent order will be the order we're looking for
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // doing a few joins, give llbl the information
                    RelationCollection relations = new RelationCollection(OrderEntity.Relations.OrderItemEntityUsingOrderID);
                    relations.Add(OrderItemEntity.Relations.GetSubTypeRelation("EbayOrderItemEntity"));

                    // get the order id of an the order having 
                    object objOrderID = adapter.GetScalar(EbayOrderItemFields.OrderID,
                        null, AggregateFunction.None,
                        EbayOrderItemFields.EbayItemID == ebayItemID & EbayOrderItemFields.EbayTransactionID == transactionID &
                            OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false,
                        null,
                        relations);

                    if (objOrderID == null)
                    {
                        // order does not exist
                        return null;
                    }
                    else
                    {
                        // return the order entity, with associated items, charges, etc
                        long orderID = (long)objOrderID;

                        // grab order items, charges, shipments since they'll be needed during processing
                        PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderEntity);
                        prefetch.Add(OrderEntity.PrefetchPathOrderItems).SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);
                        prefetch.Add(OrderEntity.PrefetchPathOrderPaymentDetails);
                        prefetch.Add(OrderEntity.PrefetchPathOrderCharges);

                        EbayOrderEntity ebayOrder = new EbayOrderEntity(orderID);
                        adapter.FetchEntity(ebayOrder, prefetch);

                        return ebayOrder;
                    }
                }
            }
            else
            {
                // searching for a combined payment
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    EntityCollection<EbayOrderEntity> collection = new EntityCollection<EbayOrderEntity>();

                    RelationPredicateBucket bucket = new RelationPredicateBucket(EbayOrderFields.EbayOrderID == ebayOrderID & EbayOrderFields.StoreID == Store.StoreID);

                    // grab order items, charges, shipments since they'll be needed during processing
                    PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderEntity);
                    prefetch.Add(OrderEntity.PrefetchPathOrderItems).SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);
                    prefetch.Add(OrderEntity.PrefetchPathOrderPaymentDetails);
                    prefetch.Add(OrderEntity.PrefetchPathOrderCharges);

                    adapter.FetchEntityCollection(collection, bucket, prefetch);
                    return collection.FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Assign the next order number to the provided order
        /// </summary>
        private void AssignOrderNumber(EbayOrderEntity order)
        {
            if (order.IsNew)
            {
                order.OrderNumber = GetNextOrderNumber();
            }
        }

        /// <summary>
        /// Verify order totals are correct.
        /// </summary>
        protected override void VerifyOrderTotal(OrderEntity order)
        {
            // do nothing because during normal ebay operations, there are 
            // times when the orders don't always balance correctly temporarily.
        }
    }
}
