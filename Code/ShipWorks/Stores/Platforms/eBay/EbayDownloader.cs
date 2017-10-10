using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Downloader for eBay
    /// </summary>
    [NDependIgnoreLongTypes]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Ebay)]
    public class EbayDownloader : StoreDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EbayDownloader));
        private readonly IEbayWebClient webClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        // The current time according to eBay
        DateTime eBayOfficialTime;

        // Total number of orders expected during this download
        int expectedCount = -1;

        /// <summary>
        /// Create the new eBay downloader
        /// </summary>
        public EbayDownloader(StoreEntity store, IEbayWebClient webClient,
            IStoreTypeManager storeTypeManager, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager.GetType(store), configurationData, sqlAdapterFactory)
        {
            this.webClient = webClient;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Begin the order download process
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Connecting to eBay...";

                // Get the official eBay time in UTC
                var token = EbayToken.FromStore((EbayStoreEntity) Store);
                eBayOfficialTime = webClient.GetOfficialTime(token);

                bool morePages = await DownloadOrders().ConfigureAwait(false);
                if (!morePages)
                {
                    return;
                }

                if (!DownloadFeedback())
                {
                    return;
                }

                // Done
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

        #region Orders

        /// <summary>
        /// Download all orders from eBay
        /// </summary>
        private async Task<bool> DownloadOrders()
        {
            // Controls whether we download using eBay paging, or using our typical sliding method where we always just adjust the start time and ask for page 1.
            //bool usePagedDownload = true;

            // Get the date\time to start downloading from
            DateTime rangeStart = (await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false)) ??
                DateTime.UtcNow.AddDays(-7);
            DateTime rangeEnd = eBayOfficialTime.AddMinutes(-5);

            // Ebay only allows going back 30 days
            if (rangeStart < eBayOfficialTime.AddDays(-30))
            {
                rangeStart = eBayOfficialTime.AddDays(-30).AddMinutes(5);
            }

            var token = EbayToken.FromStore((EbayStoreEntity) Store);
            int page = 1;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                GetOrdersResponseType response = webClient.GetOrders(token, rangeStart.To(rangeEnd), page);

                // Grab the total expected account from the first page
                if (expectedCount < 0)
                {
                    expectedCount = response.PaginationResult.TotalNumberOfEntries;
                }

                // Process all of the downloaded orders
                foreach (OrderType orderType in response.OrderArray)
                {
                    // Check for user cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }

                    //DateTime lastModified = orderType.CheckoutStatus.LastModifiedTime;

                    // Skip any that are out of range.  We take any that are a day out of range back, b\c eBay's API sometimes returns stuff in the next page that should have been on the previous.
                    // I have open bug reports with them.  12/18/13
                    // Comented out because hopefully we can add this back...
                    //if (!usePagedDownload)
                    //{
                    //    if (lastModified < rangeStart.AddDays(-1) || lastModified > rangeEnd)
                    //    {
                    //        log.WarnFormat("Skipping eBay order {0} since it's out of our date range {1}", orderType.OrderID, orderType.CheckoutStatus.LastModifiedTime);
                    //        continue;
                    //    }
                    //}

                    await ProcessOrder(orderType).ConfigureAwait(false);

                    Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved, expectedCount);
                    Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);

                    // Update the range for the next time around.  Should always be ascending
                    //if (!usePagedDownload)
                    //{
                    //    if (lastModified > rangeStart)
                    //    {
                    //        rangeStart = lastModified;
                    //    }
                    //}
                }

                // Quit if eBay says there aren't any more
                if (!response.HasMoreOrders)
                {
                    return true;
                }

                // Increment the page, if that's the method we are using
                //if (usePagedDownload)
                //{
                page++;
                //}
            }
        }

        /// <summary>
        /// Process the given eBay order
        /// </summary>
        private async Task ProcessOrder(OrderType orderType)
        {
            // Get the ShipWorks order.  This ends up calling our overridden FindOrder implementation
            GenericResult<OrderEntity> result = await InstantiateOrder(new EbayOrderIdentifier(orderType.OrderID)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderType.OrderID, result.Message);
                return;
            }

            EbayOrderEntity order = (EbayOrderEntity) result.Value;

            // Special processing for canceled orders. If we'd never seen it before, there's no reason to do anything - just ignore it.
            if (orderType.OrderStatus == OrderStatusCodeType.Cancelled && order.IsNew)

            {
                log.WarnFormat("Skipping eBay order {0} due to we've never seen it and it's canceled.", orderType.OrderID);
                return;
            }

            await PopulateOrderDetails(orderType, order).ConfigureAwait(false);

            // Load all the transactions (line items) for the order
            List<OrderItemEntity> abandonedItems = LoadTransactions(order, orderType);

            // Update PayPal information
            await UpdatePayPal(order, orderType).ConfigureAwait(false);

            // Charges
            if (!order.CombinedLocally)
            {
                UpdateCharges(order, orderType);
            }

            // Notes
            await UpdateNotes(order, orderType).ConfigureAwait(false);

            // Make sure we have the latest GSP data
            UpdateGlobalShippingProgramInfo(order, orderType.IsMultiLegShipping, orderType.MultiLegShippingDetails);

            // If all items are shipped, and the local status isn't set yet, set it to shipped
            if (string.IsNullOrWhiteSpace(order.LocalStatus) && order.OrderItems.OfType<EbayOrderItemEntity>().All(item => item.MyEbayShipped))
            {
                order.LocalStatus = "Shipped";
            }

            // Need to
            MergeOrderItemsFromDb(order);

            // Make totals adjustments
            BalanceOrderTotal(order, orderType);

            await SaveOrder(order, abandonedItems).ConfigureAwait(false);
        }

        /// <summary>
        /// Populate details of the order
        /// </summary>
        private async Task PopulateOrderDetails(OrderType orderType, EbayOrderEntity order)
        {
            // If its new it needs a ShipWorks order number
            if (order.IsNew)
            {
                order.OrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);

                // We use the oldest auction date as the order date
                order.OrderDate = DetermineOrderDate(orderType);
            }

            // Update last modified
            order.OnlineLastModified = orderType.CheckoutStatus.LastModifiedTime;

            // Online status
            order.OnlineStatusCode = (int) orderType.OrderStatus;
            order.OnlineStatus = EbayUtility.GetOrderStatusName(orderType.OrderStatus);

            // SellingManager Pro
            order.SellingManagerRecord = orderType.ShippingDetails.SellingManagerSalesRecordNumberSpecified ?
                orderType.ShippingDetails.SellingManagerSalesRecordNumber :
                (int?) null;

            // Buyer , email, and address
            order.EbayBuyerID = orderType.BuyerUserID;
            order.ShipEmail = order.BillEmail = DetermineBuyerEmail(orderType);
            UpdateOrderAddress(order, orderType.ShippingAddress);

            // Requested shipping (but only if we actually have an address)
            if (!string.IsNullOrWhiteSpace(order.ShipLastName) || !string.IsNullOrWhiteSpace(order.ShipCity) || !string.IsNullOrWhiteSpace(order.ShipCountryCode))
            {
                order.RequestedShipping = EbayUtility.GetShipmentMethodName(orderType.ShippingServiceSelected.ShippingService);
            }
            else
            {
                order.RequestedShipping = "";
            }
        }

        /// <summary>
        /// Fetches the combined order lines.
        /// </summary>
        private static void MergeOrderItemsFromDb(EbayOrderEntity order)
        {
            List<OrderItemEntity> updatedLinesFromEbay = order.OrderItems.ToList();
            order.OrderItems.Clear();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            // We just fetched these order items and now they are in memory
            EntityCollection<OrderItemEntity> orderLinesFromDb = order.OrderItems;

            for (int index = 0; index < orderLinesFromDb.Count; index++)
            {
                OrderItemEntity orderItemFromDb = orderLinesFromDb[index];
                OrderItemEntity updatedLineFromEbay =
                    updatedLinesFromEbay.SingleOrDefault(line => line.OrderItemID == orderItemFromDb.OrderItemID);

                // Replace the order item from DB with potentially updated Ebay line.
                if (updatedLineFromEbay != null)
                {
                    order.OrderItems[index] = updatedLineFromEbay;
                }
            }
            updatedLinesFromEbay
                .Where(fromEbay => orderLinesFromDb.All(fromDb => fromDb.OrderItemID != fromEbay.OrderItemID)) // select new items from ebay
                .ToList()
                .ForEach(fromEbay => order.OrderItems.Add(fromEbay)); // add them to the database
        }

        /// <summary>
        /// Save the given order, handling all the given abandoned items that have now moved to the new order
        /// </summary>
        private Task SaveOrder(EbayOrderEntity order, List<OrderItemEntity> abandonedItems)
        {
            List<OrderEntity> affectedOrders = new List<OrderEntity>();

            // Build a distinct list of all the orders affected by the abandoned items
            foreach (long orderID in abandonedItems.Select(i => i.OrderID).Distinct())
            {
                // Load the order and all of it's items (which, will duplicate any abandoned item entities)
                OrderEntity affectedOrder = (OrderEntity) DataProvider.GetEntity(orderID);
                affectedOrder.OrderItems.AddRange(DataProvider.GetRelatedEntities(orderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>());

                affectedOrders.Add(affectedOrder);
            }

            SqlAdapterRetry<SqlDeadlockException> sqlDeadlockRetry = new SqlAdapterRetry<SqlDeadlockException>(5, -5, string.Format("EbayDownloader.ProcessOrder for entity {0}", order.OrderID));

            return sqlDeadlockRetry.ExecuteWithRetryAsync(
                retryNumber => PerformSave(order, abandonedItems, retryNumber, affectedOrders),
                ex => ex is ORMEntityOutOfSyncException);
        }

        /// <summary>
        /// Perform the actual save
        /// </summary>
        private async Task PerformSave(EbayOrderEntity order, List<OrderItemEntity> abandonedItems, int retryNumber, List<OrderEntity> affectedOrders)
        {
            var clonedOrder = EntityUtility.CloneEntity(order);
            var clonedAffectedOrders = affectedOrders.Select(EntityUtility.CloneEntity).ToList();
            var clonedAbandonedItems = abandonedItems.Select(EntityUtility.CloneEntity).ToList();

            await connection.WithTransaction(async (transaction, adapter) =>
            {
                // Save the new order
                var postAction = await SaveDownloadedOrderWithoutPostAction(clonedOrder, transaction).ConfigureAwait(false);

                // Remove the abandoned items
                DeleteAbandonedItems(clonedAbandonedItems, clonedAffectedOrders, adapter);

                // We've seen an entity out of sync exceptions in ShipSense when getting the store ID
                if (retryNumber == 0)
                {
                    adapter.FetchEntity(clonedOrder);
                }

                // Copy Notes, Shipments from affected orders into the combined order
                // delete the affected orders
                await ConsolidateOrderResources(clonedOrder, clonedAffectedOrders, adapter).ConfigureAwait(false);

                transaction.Commit();

                postAction();
            }).ConfigureAwait(false);

            await UpdateOrderStatusesAfterSave(clonedOrder, sqlAdapterFactory.Create(connection)).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete abandoned items from orders that are being combined
        /// </summary>
        private void DeleteAbandonedItems(IEnumerable<OrderItemEntity> abandonedItems, IEnumerable<OrderEntity> affectedOrders, ISqlAdapter adapter)
        {
            // Go through each abandoned item and delete it
            foreach (OrderItemEntity item in abandonedItems)
            {
                // Detach it from the order
                // This is to get the appropriate orderitem instance
                OrderItemEntity orderItem = affectedOrders.Single(o => o.OrderID == item.OrderID).OrderItems.SingleOrDefault(i => i.OrderItemID == item.OrderItemID);
                if (orderItem != null)
                {
                    orderItem.Order = null;
                }
                else
                {
                    log.Info($"Item {item.OrderItemID} does not belong to an order.");
                }

                // Make sure the detachment works both ways
                Debug.Assert(affectedOrders.Single(o => o.OrderID == item.OrderID).OrderItems.All(i => i.OrderItemID != item.OrderItemID));

                // Deleted all the attributes
                foreach (OrderItemAttributeEntity attribute in item.OrderItemAttributes)
                {
                    adapter.DeleteEntity(attribute);
                }

                // And delete it
                adapter.DeleteEntity(item);
            }
        }

        /// <summary>
        /// Copy Notes and Shipments from old orders into the newly created combined order and remove the old orders
        /// </summary>
        /// <param name="order">The new order that the old</param>
        /// <param name="affectedOrders">Old orders to copy from and delete</param>
        /// <param name="adapter">The adapter to use</param>
        private async Task ConsolidateOrderResources(OrderEntity order, IEnumerable<OrderEntity> affectedOrders, ISqlAdapter adapter)
        {
            // Find all the orders that have no items.  We're going to have to delete them, since they are now empty and pointless.  But before
            // we delete them, we need to migrate their shipments and notes so they don't just get lost.
            foreach (OrderEntity fromOrder in affectedOrders.Where(o => o.OrderItems.Count == 0))
            {
                // Copy the notes from the old order
                await CopyNotes(fromOrder.OrderID, order, adapter).ConfigureAwait(false);

                // Copy the shipments from the old order
                await CopyShipments(fromOrder.OrderID, order, adapter).ConfigureAwait(false);

                // Delete the old order
                DeletionService.DeleteOrder(fromOrder.OrderID, adapter);
            }
        }

        /// <summary>
        /// Copies any shipment entities from one order to another
        /// </summary>
        private static async Task CopyShipments(long fromOrderID, OrderEntity toOrder, ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<ShipmentEntity> query = factory.Shipment
                .Where(ShipmentFields.OrderID == fromOrderID);
            query = FullShipmentPrefetchPath(query);

            IEntityCollection2 shipments = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);

            // Copy any existing shipments
            foreach (ShipmentEntity shipment in shipments)
            {
                // this is now a new shipment to be inserted
                EntityUtility.MarkAsNew(shipment);
                shipment.Order = toOrder;

                // Mark all the carrier-specific stuff as new
                foreach (IEntityCore entity in ((IEntityCore)shipment).GetDependingRelatedEntities())
                {
                    EntityUtility.MarkAsNew(entity);
                }

                // And all the customers stuff as new
                foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
                {
                    EntityUtility.MarkAsNew(customsItem);
                }

                shipment.OrderID = toOrder.OrderID;

                await sqlAdapter.SaveEntityAsync(shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create the pre-fetch path used to load a shipment
        /// </summary>
        private static EntityQuery<ShipmentEntity> FullShipmentPrefetchPath(EntityQuery<ShipmentEntity> entityQuery)
        {
            return entityQuery.WithPath(ShipmentEntity.PrefetchPathPostal.WithSubPath(PostalShipmentEntity.PrefetchPathUsps).WithSubPath(PostalShipmentEntity.PrefetchPathEndicia))
                .WithPath(ShipmentEntity.PrefetchPathUps.WithSubPath(UpsShipmentEntity.PrefetchPathPackages))
                .WithPath(ShipmentEntity.PrefetchPathFedEx.WithSubPath(FedExShipmentEntity.PrefetchPathPackages))
                .WithPath(ShipmentEntity.PrefetchPathIParcel.WithSubPath(IParcelShipmentEntity.PrefetchPathPackages))
                .WithPath(ShipmentEntity.PrefetchPathCustomsItems)
                .WithPath(ShipmentEntity.PrefetchPathReturnItems)
                .WithPath(ShipmentEntity.PrefetchPathAmazon)
                .WithPath(ShipmentEntity.PrefetchPathBestRate)
                .WithPath(ShipmentEntity.PrefetchPathOnTrac)
                .WithPath(ShipmentEntity.PrefetchPathOther);
        }

        /// <summary>
        /// Copies any note entities from one order to another.
        /// </summary>
        public static async Task CopyNotes(long fromOrderID, OrderEntity toOrder, ISqlAdapter sqlAdapter)
        {
            var factory = new QueryFactory();
            var query = factory.Note.Where(NoteFields.EntityID == fromOrderID);
            var newNotes = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);

            foreach (NoteEntity note in newNotes)
            {
                EntityUtility.MarkAsNew(note);
                note.Order = toOrder;

                // If its new, we have to increment reference counts
                if (note.IsNew)
                {
                    NoteManager.AdjustNoteCount(sqlAdapter, note.EntityID, 1);
                }

                await sqlAdapter.SaveEntityAsync(note).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load all the transactions (line items) for the given order. Returns a list of items for transactions that are being loaded that used to be apart
        /// of another order.  Those items must be deleted when the new order is saved, as they've now moved to the new order.
        /// </summary>
        private List<OrderItemEntity> LoadTransactions(EbayOrderEntity order, OrderType orderType)
        {
            order.GuaranteedDelivery = orderType.TransactionArray.Any(t => t.GuaranteedDelivery);

            List<OrderItemEntity> abandonedItems = new List<OrderItemEntity>();

            // Go through each transaction in the order
            foreach (TransactionType transaction in orderType.TransactionArray)
            {
                if (transaction.Item == null)
                {
                    log.InfoFormat("Encountered a NULL transaction.Item in combinedPayment.TransactionArray for eBay Order ID {0}.  Skipping.", orderType.OrderID);
                    continue;
                }

                // See if this item exists somewhere already in our database
                EbayOrderItemEntity orderItem = FindItem(new EbayOrderIdentifier(transaction.Item.ItemID, transaction.TransactionID));

                // If it does already exist, let's see if it's actually on a different order, we need to see if it exists on a different order
                if (orderItem != null)
                {
                    // If this order is new, or if the ID's don't match, then it was on a different order and we have to move it
                    if (order.IsNew || order.OrderID != orderItem.OrderID)
                    {
                        abandonedItems.Add(orderItem);

                        // This will force creating a brand new order item that will recreate this one fresh on this order.  The previous item will just be deleted, effectively moving
                        // the item from the old order to the new order
                        orderItem = null;
                    }
                }

                // See if we need to create a band new order item, we need to create it
                if (orderItem == null)
                {
                    orderItem = (EbayOrderItemEntity) InstantiateOrderItem(order);

                    orderItem.LocalEbayOrderID = orderItem.OrderID;
                    orderItem.EbayItemID = long.Parse(transaction.Item.ItemID);
                    orderItem.EbayTransactionID = long.Parse(transaction.TransactionID);
                }
                // Attach it, so any items get saved with the order
                else
                {
                    order.OrderItems.Add(orderItem);
                }

                UpdateTransaction(orderItem, orderType, transaction);
            }

            return abandonedItems;
        }

        /// <summary>
        /// The date we use for an eBay order is the oldest of all the transactions in the order
        /// </summary>
        private DateTime DetermineOrderDate(OrderType orderType)
        {
            DateTime orderDate = orderType.CreatedTime;

            // Exclude invalid (null) transactions by making sure the date is greater than the minimum because SQL Server can't
            // handle this date and it doesn't make sense anyway.
            foreach (var transaction in orderType.TransactionArray.Where(x => x.CreatedDate > DateTime.MinValue))
            {
                orderDate = (transaction.CreatedDate < orderDate) ? transaction.CreatedDate : orderDate;
            }

            return orderDate;
        }

        /// <summary>
        /// Determine the email address to use for the buyer
        /// </summary>
        private string DetermineBuyerEmail(OrderType orderType)
        {
            var buyerNodes = orderType.TransactionArray.Select(t => t.Buyer).Where(b => b != null).ToList();

            // eBay doesn't always send the real email address. First check for the first real email address we find.
            string email = buyerNodes.Select(b => b.Email).FirstOrDefault(e => e != null && e.Contains('@'));

            // Then fall back to the alias.
            if (string.IsNullOrWhiteSpace(email))
            {
                email = buyerNodes.Select(b => b.StaticAlias).FirstOrDefault(e => e != null && e.Contains('@'));
            }

            return email ?? "";
        }

        /// <summary>
        /// Update the address of the ebay order with the address information provided
        /// </summary>
        private void UpdateOrderAddress(EbayOrderEntity order, WebServices.AddressType address)
        {
            // overwrite address with downloaded address
            order.ShipStreet1 = address.Street1 ?? "";
            order.ShipStreet2 = address.Street2 ?? "";
            order.ShipCity = address.CityName ?? "";
            order.ShipStateProvCode = address.StateOrProvince == null ? string.Empty : Geography.GetStateProvCode(address.StateOrProvince) ?? "";
            order.ShipPostalCode = address.PostalCode ?? "";
            order.ShipCountryCode = GetCountry(address);
            order.ShipCompany = address.CompanyName ?? "";
            order.ShipPhone = address.Phone ?? "";

            // Split the name
            PersonName personName = PersonName.Parse(address.Name);

            order.ShipNameParseStatus = (int) personName.ParseStatus;
            order.ShipUnparsedName = personName.UnparsedName;
            order.ShipFirstName = personName.First;
            order.ShipMiddleName = personName.Middle;
            order.ShipLastName = personName.Last;

            // Fill in billing address from the shipping
            PersonAdapter.Copy(order, "Ship", order, "Bill");
        }

        /// <summary>
        /// Get a ShipWorks country from an ebay country
        /// </summary>
        private string GetCountry(WebServices.AddressType address)
        {
            if (!address.CountrySpecified)
            {
                return string.Empty;
            }

            WebServices.CountryCodeType country = address.Country;

            return Enum.GetName(typeof(WebServices.CountryCodeType),
                country == WebServices.CountryCodeType.AA ? WebServices.CountryCodeType.US : country);
        }

        /// <summary>
        /// Gets the largest last modified time we have in our database for non-manual orders for this store.
        /// If no such orders exist, and there is an initial download policy, that policy is applied.  Otherwise null is returned.
        /// </summary>
        protected override async Task<DateTime?> GetOnlineLastModifiedStartingPoint()
        {
            DateTime? onlineLastModifiedStartingPoint = await base.GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);

            if (((EbayStoreEntity) Store).DownloadOlderOrders)
            {
                // We need to calculate the starting point for the initial starting point of
                // a download cycle
                return await CalculateStartingPoint(onlineLastModifiedStartingPoint).ConfigureAwait(false);
            }

            // Use the default behavior - the store is not configured to check for older orders
            return onlineLastModifiedStartingPoint;
        }

        /// <summary>
        /// Calculates the DateTime to use as the starting point from the previous four download cycles.
        /// </summary>
        /// <param name="onlineLastModifiedStartingPoint">The DateTime of the most recent online last modified date that ShipWorks is aware of. This is used
        /// in the event that the last four download cycles are more recent (i.e. this is to ensure overlap).</param>
        /// <returns>The DateTime to use as the starting point of a download.</returns>
        private async Task<DateTime?> CalculateStartingPoint(DateTime? onlineLastModifiedStartingPoint)
        {
            // Need to check previous download history for this store to calculate the starting point.
            const int previousNumberOfDownloads = 4;
            List<DateTime> startDates = await GetPreviousDownloadStartTimes(previousNumberOfDownloads).ConfigureAwait(false);

            if (startDates.None())
            {
                return onlineLastModifiedStartingPoint;
            }
            if (startDates.Count < 4)
            {
                // We don't have enough download history, so subtract the initial download days from the
                // earliest download date to mimic the first download cycle
                return startDates.Min().Subtract(TimeSpan.FromDays(Store.InitialDownloadDays ?? 7));
            }

            // Subtract 5 minutes to mimic the buffer used during the previous download
            DateTime downloadIntervalStartTime = startDates.Min().Subtract(TimeSpan.FromMinutes(5));

            // Use ebay official time in case the computer server time is off.
            DateTime twoHoursAgo = eBayOfficialTime.Subtract(TimeSpan.FromMinutes(120));

            // Use the lesser value of two hours ago, downloadIntervalStartTime and the most recent
            // last modified date. This is for the case where the order data has been modified by support
            // (or an action has been previously created by support to manipulate the last modified date
            // to resolve skipped orders).
            // The two hours ago protects from people jamming on the download button
            return new[]
            {
                twoHoursAgo,
                downloadIntervalStartTime,
                onlineLastModifiedStartingPoint
            }.Min();
        }

        /// <summary>
        /// Gets the date/time that the last number of most recent, successful downloads having orders
        /// were started for this store.
        /// </summary>
        /// <param name="previousDownloadCount">The number of previous downloads to use when getting starting point(s).</param>
        /// <returns>A List of DateTime instance representing the time the previous downloads started.</returns>
        private async Task<List<DateTime>> GetPreviousDownloadStartTimes(int previousDownloadCount)
        {
            QueryFactory factory = new QueryFactory();
            DynamicQuery<DateTime> query = factory.Create()
                .Select(() => DownloadFields.Started.ToValue<DateTime>())
                .Where(DownloadFields.StoreID == Store.StoreID)
                .AndWhere(DownloadFields.Result == (int) DownloadResult.Success)
                .AndWhere(DownloadFields.QuantityTotal > 0)
                .OrderBy(DownloadFields.DownloadID.Descending())
                .Limit(previousDownloadCount);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Update all of the charges for the order
        /// </summary>
        private void UpdateCharges(EbayOrderEntity order, OrderType orderType)
        {
            UpdateShippingCharges(order, orderType);
            UpdateAdjustmentCharges(order, orderType);
            UpdateSalesTaxCharges(order, orderType);
        }

        /// <summary>
        /// Update the shipping charges
        /// </summary>
        private void UpdateShippingCharges(EbayOrderEntity order, OrderType orderType)
        {
            // Shipping
            OrderChargeEntity shipping = GetCharge(order, "SHIPPING", "Shipping");

            // Only exists for GSP shipments
            if (orderType.MultiLegShippingDetails != null &&
                orderType.MultiLegShippingDetails.SellerShipmentToLogisticsProvider != null &&
                orderType.MultiLegShippingDetails.SellerShipmentToLogisticsProvider.ShippingServiceDetails != null &&
                orderType.MultiLegShippingDetails.SellerShipmentToLogisticsProvider.ShippingServiceDetails.TotalShippingCost != null)
            {
                shipping.Amount = (decimal) orderType.MultiLegShippingDetails.SellerShipmentToLogisticsProvider.ShippingServiceDetails.TotalShippingCost.Value;
            }
            else if (orderType.ShippingServiceSelected.ShippingServiceCost != null)
            {
                shipping.Amount = (decimal) orderType.ShippingServiceSelected.ShippingServiceCost.Value;
            }
            else
            {
                shipping.Amount = 0;
            }
        }

        /// <summary>
        /// Update adjustment charges
        /// </summary>
        private void UpdateAdjustmentCharges(EbayOrderEntity order, OrderType orderType)
        {
            // Only use the adjustment value if the order is considered complete.  Otherwise ebay seems to put an adjustment on non-complete orders that sets the total to zero.
            decimal adjustment = (order.OnlineStatusCode is int && (int) order.OnlineStatusCode == (int) OrderStatusCodeType.Completed) ?
                (decimal) orderType.AdjustmentAmount.Value :
                0m;

            OrderChargeEntity adjust = GetCharge(order, "ADJUST", "Adjustment", adjustment != 0);

            // Apply the adjustment
            if (adjust != null)
            {
                adjust.Amount = adjustment;
            }
        }

        /// <summary>
        /// Update sales tax charges
        /// </summary>
        private void UpdateSalesTaxCharges(EbayOrderEntity order, OrderType orderType)
        {
            decimal salesTax = 0m;

            if (orderType.ShippingDetails.SalesTax != null && orderType.ShippingDetails.SalesTax.SalesTaxAmount != null)
            {
                salesTax = (decimal) orderType.ShippingDetails.SalesTax.SalesTaxAmount.Value;
            }

            // Tax
            OrderChargeEntity tax = GetCharge(order, "TAX", "Sales Tax");
            tax.Amount = salesTax;
        }

        /// <summary>
        /// Update the notes for the given order
        /// </summary>
        private Task UpdateNotes(EbayOrderEntity order, OrderType orderType) =>
            InstantiateNote(order, orderType.BuyerCheckoutMessage, order.OrderDate, NoteVisibility.Public, true);

        /// <summary>
        /// Update external PayPal information for the order
        /// </summary>
        private async Task UpdatePayPal(EbayOrderEntity order, OrderType orderType)
        {
            string transactionID = "";
            PayPalAddressStatus addressStatus = PayPalAddressStatus.None;

            // Only try to find PayPal details for PayPal orders
            if (orderType.CheckoutStatus.PaymentMethod == BuyerPaymentMethodCodeType.PayPal)
            {
                // See if we can find the transaction ID from the monetary details
                if (orderType.MonetaryDetails != null && orderType.MonetaryDetails.Payments != null && orderType.MonetaryDetails.Payments.Payment != null)
                {
                    transactionID = orderType.MonetaryDetails.Payments.Payment
                        .Where(p => p.ReferenceID != null && p.ReferenceID.Value != null && p.ReferenceID.Value.Length > 10)
                        .Select(p => p.ReferenceID.Value)
                        .FirstOrDefault() ?? "";
                }

                // See if we need to download paypal details
                if (((EbayStoreEntity) Store).DownloadPayPalDetails)
                {
                    // If we don't have the transaction id yet, try to find it
                    if (transactionID.Length == 0)
                    {
                        transactionID = FindPayPalTransactionID(order.OrderDate, order.ShipLastName, orderType.Total.Value);
                    }

                    // If we have it now, load the PayPal details
                    if (transactionID.Length > 0)
                    {
                        try
                        {
                            addressStatus = await LoadPayPalTransactionData(order, transactionID).ConfigureAwait(false);
                        }
                        catch (PayPalException)
                        {
                            // Transaction ID must have been bad, search again
                            transactionID = FindPayPalTransactionID(order.OrderDate, order.ShipLastName, orderType.Total.Value);

                            if (transactionID.Length > 0)
                            {
                                addressStatus = await LoadPayPalTransactionData(order, transactionID).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }

            // For now apply this to each of the items - we need to move this data up to the order level
            foreach (EbayOrderItemEntity item in order.OrderItems.OfType<EbayOrderItemEntity>())
            {
                item.PayPalTransactionID = transactionID;
                item.PayPalAddressStatus = (int) addressStatus;
            }
        }

        /// <summary>
        /// Update the transaction (order item) info
        /// </summary>
        private void UpdateTransaction(EbayOrderItemEntity orderItem, OrderType orderType, TransactionType transaction)
        {
            if (string.IsNullOrWhiteSpace(orderItem.Code))
            {
                orderItem.Code = transaction.Item.ItemID;
            }

            if (string.IsNullOrWhiteSpace(orderItem.Name))
            {
                orderItem.Name = transaction.Item.Title;
            }

            UpdateTransactionSKU(orderItem, transaction.Item.SKU ?? "");

            orderItem.UnitPrice = (decimal) transaction.TransactionPrice.Value;
            orderItem.Quantity = transaction.QuantityPurchased;

            // Checkout (from order - these can be moved up in the database from the item to the order level)
            orderItem.PaymentMethod = (int) orderType.CheckoutStatus.PaymentMethod;
            orderItem.PaymentStatus = (int) orderType.CheckoutStatus.eBayPaymentStatus;
            orderItem.CompleteStatus = (int) orderType.CheckoutStatus.Status;

            // My eBay statuses - we set this at the line-item level, but the API provides them at the order level.
            orderItem.MyEbayPaid = orderType.PaidTimeSpecified;
            orderItem.MyEbayShipped = orderType.ShippedTimeSpecified || HasTrackingNumber(transaction);

            // Load variation information
            UpdateTransactionVariationDetail(orderItem, transaction);

            // SellingManager Pro
            orderItem.SellingManagerRecord = transaction.ShippingDetails.SellingManagerSalesRecordNumberSpecified ? transaction.ShippingDetails.SellingManagerSalesRecordNumber : 0;

            // Update the item with extra details that must be downloaded externally
            UpdateTransactionExtraDetails(orderItem, transaction);
        }

        /// <summary>
        /// Variations allow single auctions to have items that vary in size, color, etc.
        /// </summary>
        private void UpdateTransactionVariationDetail(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            if (transaction.Variation == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(transaction.Variation.VariationTitle))
            {
                orderItem.Name = transaction.Variation.VariationTitle;
            }

            // Overwrite the SKU if a variation SKU is provided.
            if (!string.IsNullOrWhiteSpace(transaction.Variation.SKU))
            {
                UpdateTransactionSKU(orderItem, transaction.Variation.SKU);
            }

            // If this is a new item and one with variations, we can add attributes now
            if (orderItem.IsNew)
            {
                if (transaction.Variation.VariationSpecifics != null)
                {
                    // create attributes for the variations
                    foreach (NameValueListType variation in transaction.Variation.VariationSpecifics)
                    {
                        OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);
                        attribute.Name = variation.Name;
                        attribute.Description = String.Join("; ", variation.Value);
                        attribute.UnitPrice = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Update the SKU of the given item with the given SKU
        /// </summary>
        private void UpdateTransactionSKU(EbayOrderItemEntity orderItem, string sku)
        {
            // Don't overwrite it if it's already there.  One important scenario is if the user applies their own SKUs and doesn't want eBay's to overwrite, such as SKUVault
            if (!string.IsNullOrWhiteSpace(orderItem.SKU) && !orderItem.IsNew)
            {
                return;
            }

            // FreshDesk #517760 - SKU was coming down literally as "null"
            if (string.Compare(sku, "null", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                orderItem.SKU = sku;
            }
        }

        /// <summary>
        /// Update the weight of the given item based on the detail in the transaction
        /// </summary>
        private void UpdateWeight(EbayOrderItemEntity orderItem, ItemType item)
        {
            if (item.ShippingPackageDetails != null)
            {
                WebServices.MeasureType weightMajor = item.ShippingPackageDetails.WeightMajor;
                WebServices.MeasureType weightMinor = item.ShippingPackageDetails.WeightMinor;

                if (weightMajor != null && weightMinor != null)
                {
                    if (weightMajor.unit == "POUNDS" || weightMajor.unit == "lbs")
                    {
                        orderItem.Weight = (double) (weightMajor.Value + weightMinor.Value / 16.0m);
                    }
                }
            }
        }

        /// <summary>
        /// Update the details of the transaction with information that we have to make an extra call for
        /// </summary>
        private void UpdateTransactionExtraDetails(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            // If updating external stuff is turned off, don't do it
            if (!((EbayStoreEntity) Store).DownloadItemDetails)
            {
                return;
            }

            // If this item isn't new, or we already have the extra info, then we don't need to waste time on this
            if (!orderItem.IsNew || !string.IsNullOrWhiteSpace(orderItem.Image))
            {
                return;
            }

            try
            {
                var token = EbayToken.FromStore((EbayStoreEntity) Store);
                ItemType ebayItem = webClient.GetItem(token, transaction.Item.ItemID);
                UpdateWeight(orderItem, ebayItem);
                UpdateTransactionImages(orderItem, ebayItem);
            }
            catch (EbayException exception)
            {
                // Check if we get error code 17 (the item has been deleted). eBay deletes items in certain
                // situations where the items falls outside of their user agreements (counterfeit, selling illegal/trademarked items, etc.)
                if (exception.ErrorCode == "17" || exception.ErrorCode == "21917182")
                {
                    // Just log this exception otherwise the download process will not complete
                    log.WarnFormat("eBay returned an error when trying to download item details for item {0} (item ID {1}, transaction ID {2}). {3}", orderItem.Name, orderItem.EbayItemID, orderItem.EbayTransactionID, exception.Message);
                    log.WarnFormat("The seller may need to contact eBay support to determine why the order item may have been deleted.");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update transaction images
        /// </summary>
        private void UpdateTransactionImages(EbayOrderItemEntity orderItem, ItemType ebayItem)
        {
            PictureDetailsType pictureDetails = ebayItem.PictureDetails;

            // The first picture in PictureURL is the default
            if (pictureDetails?.PictureURL?.Any() == true)
            {
                orderItem.Image = pictureDetails.PictureURL[0] ?? "";
                orderItem.Thumbnail = orderItem.Image;
            }

            // If still no image, see if there is a stock image
            if (string.IsNullOrWhiteSpace(orderItem.Image))
            {
                if (ebayItem.ProductListingDetails != null && ebayItem.ProductListingDetails.IncludeStockPhotoURL)
                {
                    orderItem.Image = ebayItem.ProductListingDetails.StockPhotoURL ?? "";
                    orderItem.Thumbnail = orderItem.Image;
                }
            }

            // See if there are other details we can use
            if (ebayItem.ProductListingDetails != null)
            {
                orderItem.UPC = ebayItem.ProductListingDetails.UPC ?? "";
                orderItem.ISBN = ebayItem.ProductListingDetails.ISBN ?? "";
            }
        }

        /// <summary>
        /// Indicates if the transaction has a shipment tracking number attached
        /// </summary>
        private bool HasTrackingNumber(TransactionType transaction)
        {
            return
                transaction.ShippingDetails.ShipmentTrackingDetails != null &&
                transaction.ShippingDetails.ShipmentTrackingDetails.Any() &&
                !string.IsNullOrWhiteSpace(transaction.ShippingDetails.ShipmentTrackingDetails.First().ShipmentTrackingNumber);
        }

        /// <summary>
        /// Reconciles the ShipWorks order total with what eBay has on record.
        /// Any adjustment are done via an OTHER charge
        /// </summary>
        private void BalanceOrderTotal(EbayOrderEntity order, OrderType orderType)
        {
            double amountPaid = orderType.AmountPaid != null ? orderType.AmountPaid.Value : orderType.Total.Value;

            if (orderType.MonetaryDetails != null &&
                orderType.MonetaryDetails.Payments != null &&
                orderType.MonetaryDetails.Payments.Payment != null &&
                orderType.MonetaryDetails.Payments.Payment.Any(payment => payment.Payee != null &&
                                                                          payment.Payee.type == UserIdentityCodeType.eBayPartner &&
                                                                          payment.PaymentAmount != null))
            {
                amountPaid -= orderType.MonetaryDetails.Payments.Payment.Where(payment => payment.Payee != null && payment.Payee.type == UserIdentityCodeType.eBayPartner).Sum(payment => payment.PaymentAmount.Value);
            }

            order.OrderTotal = OrderUtility.CalculateTotal(order);

            if (order.OrderTotal != Convert.ToDecimal(amountPaid) &&
                order.OnlineStatusCode is int && (int) order.OnlineStatusCode == (int) OrderStatusCodeType.Completed && // only make adjustments if it's considered complete
                !order.CombinedLocally) // Don't bother trying to reconcile a locally combined order
            {
                OrderChargeEntity otherCharge = GetCharge(order, "OTHER", "Other");
                otherCharge.Description = "Other";
                otherCharge.Amount += Convert.ToDecimal(amountPaid) - order.OrderTotal;
            }
        }

        /// <summary>
        /// Updates the global shipping program info.
        /// </summary>
        /// <param name="order">The ebay order.</param>
        /// <param name="isGsp">if set to <c>true</c> [is global shipping program order].</param>
        /// <param name="gspDetails">The multi leg shipping details.</param>
        /// <exception cref="EbayException">eBay did not provide a reference ID for an order designated for the Global Shipping Program.</exception>
        private void UpdateGlobalShippingProgramInfo(EbayOrderEntity order, bool isGsp, MultiLegShippingDetailsType gspDetails)
        {
            order.GspEligible = isGsp;

            if (order.GspEligible)
            {
                UpdateGspInfoWhenEligible(order, gspDetails);
            }
            else
            {
                UpdateGspInfoWhenNotEligible(order);
            }
        }

        /// <summary>
        /// Update the GSP information when the order is not eligible
        /// </summary>
        /// <param name="order"></param>
        private static void UpdateGspInfoWhenNotEligible(EbayOrderEntity order)
        {
            // This isn't a GSP order, so we're going to wipe the GSP data from the order in the event that
            // an order was previously marked as a GSP, but is no longer for some reason
            order.GspFirstName = string.Empty;
            order.GspLastName = string.Empty;

            // Address info
            order.GspStreet1 = string.Empty;
            order.GspStreet2 = string.Empty;
            order.GspCity = string.Empty;
            order.GspStateProvince = string.Empty;
            order.GspPostalCode = string.Empty;
            order.GspCountryCode = string.Empty;

            // Reset the reference ID and the shipping method to standard
            order.GspReferenceID = string.Empty;

            if (order.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
            {
                // Only change the status if it has not been previously overridden; due to the individual transactions being downloaded
                // first then the combined orders being downloaded, this would inadvertently get set back to GSP if the combined order is
                // a GSP order (if the same buyer purchases one item that is GSP and another that isn't, the GSP settings get applied
                // to the combined order).
                order.SelectedShippingMethod = (int) EbayShippingMethod.DirectToBuyer;
            }
        }

        /// <summary>
        /// Update the GSP when the order is eligible
        /// </summary>
        private static void UpdateGspInfoWhenEligible(EbayOrderEntity order, MultiLegShippingDetailsType gspDetails)
        {
            // This is part of the global shipping program, so we need to pull out the address info
            // of the international shipping provider but first make sure there aren't any null
            // objects in the address hierarchy
            if (gspDetails != null &&
                gspDetails.SellerShipmentToLogisticsProvider != null &&
                gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress != null)
            {
                // Pull out the name of the international shipping provider
                PersonName name = PersonName.Parse(gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Name);
                order.GspFirstName = name.First;
                order.GspLastName = name.Last;

                // Address info

                // eBay includes "Suite 400" as part of street1 which some shipping carriers (UPS) don't recognize as a valid address.
                // So, we'll try to split the street1 line (1850 Airport Exchange Blvd, Suite 400) into separate addresses based on
                // the presence of a comma in street 1

                // We're ultimately going to populate the ebayOrder.GspStreet property values based on the elements in th streetLines list
                string[] streetLines = ParseGspStreet(gspDetails);

                PopulateGspAddress(order, gspDetails, streetLines);
            }

            if (string.IsNullOrEmpty(order.GspReferenceID))
            {
                // We can't necessarily reference an ID number here since the ShipWorks order ID may not be assigned yet,
                // so we'll reference the order date and the buyer that made the purchase
                string message = string.Format("eBay did not provide a reference ID for an order designated for the Global Shipping Program. The order was placed on {0} from buyer {1}.",
                                    StringUtility.FormatFriendlyDateTime(order.OrderDate), order.BillUnparsedName);

                throw new EbayMissingGspReferenceException(message);
            }
        }

        /// <summary>
        /// Populate the GSP address
        /// </summary>
        private static void PopulateGspAddress(EbayOrderEntity order, MultiLegShippingDetailsType gspDetails, string[] streetLines)
        {
            order.GspStreet1 = streetLines[0].Trim();
            order.GspStreet2 = streetLines[1].Trim();

            order.GspCity = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.CityName ?? string.Empty;
            order.GspStateProvince = Geography.GetStateProvCode(gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.StateOrProvince) ?? string.Empty;
            order.GspPostalCode = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.PostalCode ?? string.Empty;
            order.GspCountryCode = Enum.GetName(typeof(WebServices.CountryCodeType), gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Country);

            // Pull out the reference ID that will identify the order to the international shipping provider
            order.GspReferenceID = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.ReferenceID;

            if (order.GspPostalCode.Length >= 5)
            {
                // Only grab the first five digits of the postal code; there have been incidents in the past where eBay
                // sends down an invalid 9 digit postal code (e.g. 41018-319) that prevents orders from being shipped
                order.GspPostalCode = order.GspPostalCode.Substring(0, 5);
            }

            if (order.IsNew || order.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
            {
                // The seller has the choice to NOT ship GSP, so only default the shipping program to GSP for new orders
                // or orders if the selected shipping method has not been manually overridden
                order.SelectedShippingMethod = (int) EbayShippingMethod.GlobalShippingProgram;
            }
        }

        /// <summary>
        /// Parse the GSP street
        /// </summary>
        private static string[] ParseGspStreet(MultiLegShippingDetailsType gspDetails)
        {
            // Default the list to empty strings for the case where the Street1 and Street2
            // properties of the shipping address are null
            string[] streetLines =
            {
                gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 ?? string.Empty,
                gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street2 ?? string.Empty
            };

            if (gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 != null)
            {
                // Try to split the Street1 property based on comma
                string[] splitStreetInfo = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1.Split(new[] { ',' });

                // We'll always have at least one value in the result of the split which will be our value for street1
                streetLines[0] = splitStreetInfo[0];

                if (splitStreetInfo.Length > 1)
                {
                    // There were multiple components to the original Street1 address provided by eBay; this second
                    // component will be the value we use for our street 2 address instead of the value provided by eBay
                    streetLines[1] = splitStreetInfo[1].Trim();
                }
            }

            return streetLines;
        }

        /// <summary>
        /// Get the specified charge for the order
        /// </summary>
        private OrderChargeEntity GetCharge(OrderEntity order, string type, string description, bool autoCreate = true)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Type == type);

            if (orderCharge == null)
            {
                if (autoCreate)
                {
                    // create a new one
                    orderCharge = InstantiateOrderCharge(order);
                    orderCharge.Type = type;
                    orderCharge.Description = description;
                }
            }

            return orderCharge;
        }

        /// <summary>
        /// Locate an order with an OrderIdentifier
        /// </summary>
        protected override async Task<OrderEntity> FindOrder(OrderIdentifier orderIdentifier)
        {
            EbayOrderIdentifier identifier = orderIdentifier as EbayOrderIdentifier;
            if (identifier == null)
            {
                throw new InvalidOperationException("OrderIdentifier of type EbayOrderIdentifier expected.");
            }

            return await FindOrder(identifier).ConfigureAwait(false);
        }

        /// <summary>
        /// Find and load an order of the given identifier, optionally including child charges
        /// </summary>
        private async Task<EbayOrderEntity> FindOrder(EbayOrderIdentifier identifier)
        {
            QueryFactory factory = new QueryFactory();

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return identifier.EbayOrderID == 0 ?
                    await FindOrderWitoutEbayID(identifier, factory, sqlAdapter).ConfigureAwait(false) :
                    await FindOrderWithEbayID(identifier, factory, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Find an ebay order without an ebay id
        /// </summary>
        private async Task<EbayOrderEntity> FindOrderWitoutEbayID(EbayOrderIdentifier identifier, QueryFactory factory, ISqlAdapter sqlAdapter)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.EbayOrderItemEntity);

            DynamicQuery query = factory.Create()
                .Select(EbayOrderItemFields.OrderID)
                .From(Joins.Left(OrderEntity.Relations.OrderItemEntityUsingOrderID)
                    .LeftJoin(OrderItemEntity.Relations.GetSubTypeRelation(entityName)))
                .Where(EbayOrderItemFields.EbayItemID == identifier.EbayItemID)
                .AndWhere(EbayOrderItemFields.EbayTransactionID == identifier.TransactionID)
                .AndWhere(OrderFields.StoreID == Store.StoreID)
                .AndWhere(OrderFields.IsManual == false);

            long? orderId = await sqlAdapter.FetchScalarAsync<long?>(query).ConfigureAwait(false);

            if (!orderId.HasValue)
            {
                // order does not exist
                return null;
            }

            EntityQuery<EbayOrderEntity> orderQuery = factory.EbayOrder
                .Where(OrderFields.OrderID == orderId.Value)
                .WithPath(OrderEntity.PrefetchPathOrderCharges);

            return await sqlAdapter.FetchFirstAsync(orderQuery).ConfigureAwait(false);
        }

        /// <summary>
        /// Find an order with an ebay id
        /// </summary>
        private async Task<EbayOrderEntity> FindOrderWithEbayID(EbayOrderIdentifier identifier, QueryFactory factory, ISqlAdapter sqlAdapter)
        {
            EntityQuery<EbayOrderEntity> orderQuery = factory.EbayOrder
                .Where(EbayOrderFields.EbayOrderID == identifier.EbayOrderID)
                .AndWhere(EbayOrderFields.StoreID == Store.StoreID)
                .WithPath(OrderEntity.PrefetchPathOrderCharges);

            return await sqlAdapter.FetchFirstAsync(orderQuery).ConfigureAwait(false) ??
                await GetCombinedOrder(identifier, factory, sqlAdapter).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the locally combined order if it exists.
        /// </summary>
        private async Task<EbayOrderEntity> GetCombinedOrder(EbayOrderIdentifier identifier, QueryFactory factory, ISqlAdapter sqlAdapter)
        {
            EntityQuery<EbayCombinedOrderRelationEntity> orderQuery = factory.EbayCombinedOrderRelation
                .Where(EbayCombinedOrderRelationFields.EbayOrderID == identifier.EbayOrderID)
                .AndWhere(EbayCombinedOrderRelationFields.StoreID == Store.StoreID)
                .WithPath(EbayCombinedOrderRelationEntity.PrefetchPathEbayOrder
                    .WithSubPath(OrderEntity.PrefetchPathOrderCharges));

            EbayCombinedOrderRelationEntity relationEntity = await sqlAdapter.FetchFirstAsync(orderQuery).ConfigureAwait(false);
            return relationEntity?.EbayOrder;
        }

        /// <summary>
        /// Locate an item with the given identifier
        /// </summary>
        private EbayOrderItemEntity FindItem(EbayOrderIdentifier identifier)
        {
            if (identifier.EbayOrderID != 0)
            {
                throw new InvalidOperationException("FindItem not valid for identifiers representing combined orders.");
            }

            object objItemID = SqlAdapter.Default.GetScalar(EbayOrderItemFields.OrderItemID,
                null, AggregateFunction.None,
                EbayOrderItemFields.EbayItemID == identifier.EbayItemID &
                    EbayOrderItemFields.EbayTransactionID == identifier.TransactionID,
                null);

            if (objItemID == null)
            {
                // item does not exist
                return null;
            }

            // return the item entity
            long itemID = (long) objItemID;

            EbayOrderItemEntity item = new EbayOrderItemEntity(itemID);

            PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderItemEntity)
                {
                    OrderItemEntity.PrefetchPathOrderItemAttributes,
                    OrderItemEntity.PrefetchPathOrder
                };

            SqlAdapter.Default.FetchEntity(item, prefetch);

            return item.Order.StoreID == Store.StoreID ? item : null;
        }

        /// <summary>
        /// Search for a PayPal transaction that matches up with these payment details
        /// </summary>
        private string FindPayPalTransactionID(DateTime start, string payerLastName, double amount)
        {
            TransactionSearchRequestType search = new TransactionSearchRequestType
            {
                StartDate = start,
                PayerName = new PersonNameType { LastName = payerLastName }
            };

            // Perform the search
            TransactionSearchResponseType response;
            try
            {
                PayPalWebClient paypalClient = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));

                response = (TransactionSearchResponseType) paypalClient.ExecuteRequest(search);
                if (response.PaymentTransactions == null)
                {
                    // no transaction found that matches the criteria
                    return "";
                }

                // consider only those with a payer specified
                List<PaymentTransactionSearchResultType> candidates = response.PaymentTransactions.Where(p => p.Payer.Length > 0).ToList();

                // single match, must be the one we're looking for
                if (candidates.Count == 1)
                {
                    // single result
                    return candidates[0].TransactionID;
                }

                // try to further narrow it down, filter out Updates
                candidates.RemoveAll(p => p.Type != "Payment");

                // Did we remove them all
                if (candidates.Count == 0)
                {
                    return "";
                }

                // again return if there is only one
                if (candidates.Count == 1)
                {
                    return candidates[0].TransactionID;
                }

                // saw a case where we had multiple, but all were the same transaction ID
                if (candidates.All(p => p.TransactionID == candidates[0].TransactionID))
                {
                    return candidates[0].TransactionID;
                }

                if (candidates.Count > 1)
                {
                    // Pick the first one that matches the gross amount, if there's only one
                    var matches = candidates.Where(p => Math.Abs(Convert.ToDecimal(p.GrossAmount.Value)) == (decimal) amount).ToList();
                    if (matches.Any())
                    {
                        return matches.First().TransactionID;
                    }
                }

                return "";
            }
            catch (PayPalException ex)
            {
                // log the error
                log.Error("Failed to retrieve PayPal transactions for buyer " + payerLastName, ex);
                return "";
            }
        }

        /// <summary>
        /// Updates the PayPal address status, and adds any PayPal-sourced notes to the order.
        /// </summary>
        private async Task<PayPalAddressStatus> LoadPayPalTransactionData(EbayOrderEntity order, string transactionID)
        {
            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = transactionID;

            try
            {
                PayPalWebClient client = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));
                GetTransactionDetailsResponseType response = (GetTransactionDetailsResponseType) client.ExecuteRequest(request);

                // TODO: need to specify which item it's for
                await InstantiateNote(order, response.PaymentTransactionDetails.PaymentItemInfo.Memo,
                    response.PaymentTransactionDetails.PaymentInfo.PaymentDate, NoteVisibility.Public, true)
                    .ConfigureAwait(false);

                return (PayPalAddressStatus) (int) response.PaymentTransactionDetails.PayerInfo.Address.AddressStatus;
            }
            catch (PayPalException ex)
            {
                // 10007 means you didn't have permission to get the details of the transaction
                if (ex.Errors.Any(e => e.Code == "10007"))
                {
                    log.ErrorFormat("eBay had a correct transaction ({0}) but insufficient PayPal privileges to get data for it.", transactionID);

                    return PayPalAddressStatus.None;
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region Feedback

        /// <summary>
        /// Download all feedback
        /// </summary>
        private bool DownloadFeedback()
        {
            Progress.Detail = "Checking for feedback...";
            Progress.PercentComplete = 0;

            DateTime? downloadThrough = ((EbayStoreEntity) Store).FeedbackUpdatedThrough;

            // The date to stop looking at feedback, even if more exists
            if (downloadThrough == null)
            {
                downloadThrough = GetOldestOrderDate();
                if (downloadThrough == null)
                {
                    return true;
                }
            }

            // Go back a max
            if (downloadThrough < DateTime.UtcNow.AddDays(-14))
            {
                downloadThrough = DateTime.UtcNow.AddDays(-14);
            }

            int feedbackCount = 0;

            // First download all feedback received
            DateTime newestRecieved = DownloadFeedback(null, downloadThrough.Value, ref feedbackCount);

            // Check for user cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Then download all feedback left
            DownloadFeedback(FeedbackTypeCodeType.FeedbackLeft, downloadThrough.Value, ref feedbackCount);

            // Check for user cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Use the latest feedback received date as our ending point for next time. Since we queried it first, this is safe.
            SaveFeedbackCheckpoint(newestRecieved);

            return true;
        }

        /// <summary>
        /// Download feedback of the given time, through the specified date
        /// </summary>
        private DateTime DownloadFeedback(FeedbackTypeCodeType? feedbackType, DateTime downloadThrough, ref int count)
        {
            int page = 1;

            DateTime newestFeedback = downloadThrough;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                var token = EbayToken.FromStore((EbayStoreEntity) Store);
                GetFeedbackResponseType response = webClient.GetFeedback(token, feedbackType, page);

                // Quit if eBay says there aren't any more
                if (response.FeedbackDetailItemTotal == 0 || page > response.PaginationResult.TotalNumberOfPages)
                {
                    return newestFeedback;
                }

                // Process all of the downloaded feedback
                foreach (FeedbackDetailType feedback in response.FeedbackDetailArray)
                {
                    DateTime feedbackDate = feedback.CommentTime.ToUniversalTime();

                    // Record this as the newest we've seen if necessary
                    if (feedbackDate > newestFeedback)
                    {
                        newestFeedback = feedbackDate;
                    }

                    // If this goes back prior to when we want to look for feedback, or the user has canceled, we are doing
                    if (feedbackDate < downloadThrough || Progress.IsCancelRequested)
                    {
                        return newestFeedback;
                    }

                    ProcessFeedback(feedback);

                    Progress.Detail = string.Format("Processing feedback {0}...", count++);
                }

                // Next page
                page++;
            }
        }

        /// <summary>
        /// Save the checkpoint for how far back to go looking for feedback
        /// </summary>
        private void SaveFeedbackCheckpoint(DateTime newestFeedback)
        {
            EbayStoreEntity eBayStore = (EbayStoreEntity) Store;

            eBayStore.FeedbackUpdatedThrough = newestFeedback;
            SqlAdapter.Default.SaveAndRefetch(eBayStore);
        }

        /// <summary>
        /// Process the given feedback
        /// </summary>
        private void ProcessFeedback(FeedbackDetailType feedback)
        {
            SqlAdapterRetry<SqlException> sqlDeadlockRetry = new SqlAdapterRetry<SqlException>(5, -5, string.Format("EbayDownloader.ProcessFeedback for feedback.OrderLineItemID {0}", feedback.OrderLineItemID));
            sqlDeadlockRetry.ExecuteWithRetry(adapter =>
            {
                EbayOrderItemEntity item = FindItem(new EbayOrderIdentifier(feedback.OrderLineItemID));

                if (item == null)
                {
                    return;
                }

                log.DebugFormat("FEEDBACK: {0} - {1} - {2}", feedback.CommentTime, feedback.ItemID, feedback.CommentText);

                // Feedback we've received
                if (feedback.Role == TradingRoleCodeType.Seller)
                {
                    item.FeedbackReceivedType = (int) feedback.CommentType;
                    item.FeedbackReceivedComments = feedback.CommentText;
                }
                else
                {
                    item.FeedbackLeftType = (int) feedback.CommentType;
                    item.FeedbackLeftComments = feedback.CommentText;
                }

                adapter.SaveEntity(item);
            });
        }

        /// <summary>
        /// Gets the smallest order date, combined or not, for this store in the database
        /// </summary>
        private DateTime? GetOldestOrderDate()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderDate,
                    null, AggregateFunction.Min,
                    OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result is DBNull ? null : (DateTime?) result;

                log.InfoFormat("MIN(OrderDate) = {0:u}", dateTime);

                return dateTime;
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

        #endregion
    }
}
