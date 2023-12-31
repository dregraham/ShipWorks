﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// A Newegg implementation of the StoreDownloader class that interacts with the NeweggWebClient
    /// to download orders from Newegg and import them into ShipWorks.
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.NeweggMarketplace)]
    public class NeweggDownloader : StoreDownloader
    {
        private readonly ILog log;
        private readonly INeweggWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public NeweggDownloader(StoreEntity store, IStoreTypeManager storeTypeManager, INeweggWebClient webClient, Func<Type, ILog> createLogger)
            : base(store, storeTypeManager.GetType(store))
        {
            this.webClient = webClient;
            log = createLogger(GetType());

            if (!(store is NeweggStoreEntity))
            {
                string message = string.Format("An invalid store type was provided to the NeweggDownloader: {0}", Store.GetType().FullName);
                log.ErrorFormat(message);
                throw new NeweggException(message);
            }
        }

        /// <summary>
        /// Must be implemented by derived types to do the actual download
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Syncing existing orders with Newegg...";
                await SyncUnfulfilledShipWorksOrdersWithNewegg().ConfigureAwait(false);

                Progress.Detail = "Checking for new orders...";

                if ((Store as NeweggStoreEntity).ExcludeFulfilledByNewegg)
                {
                    // We don't want to download orders that are being shipped by Newegg, so we have to
                    // download the individual order types one at a time
                    await DownloadNewOrders(NeweggOrderType.ShipBySeller).ConfigureAwait(false);
                    await DownloadNewOrders(NeweggOrderType.MultiChannel).ConfigureAwait(false);
                }
                else
                {
                    // The store is not configured to exclude orders shipped by Neweegg, so
                    // we're downloading all order types
                    await DownloadNewOrders(NeweggOrderType.All).ConfigureAwait(false);
                }
            }
            catch (NeweggException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Newegg doesn't have a last modified date to allow us to pull down existing orders, so
        /// there is a a chance ShipWorks order data could be in an incorrect state if the user
        /// marked an item as shipped via the Newegg portal or some other Newegg process updated
        /// the order.
        ///
        /// This will sync the order status of any unfulfilled orders existing in ShipWorks (those
        /// orders not having a status of Invoiced or Voided) with orders in Newegg to account for
        /// any status changes occurring outside of ShipWorks.
        /// </summary>
        public async Task SyncUnfulfilledShipWorksOrdersWithNewegg()
        {
            // Get all of the Newegg orders that are in an unshipped state
            List<NeweggOrderEntity> unfulfilledShipWorksOrders = GetUnfulfilledShipWorksOrders();

            // Download the order info from Newegg for this specific list of orders
            var typedStore = Store as INeweggStoreEntity;

            DownloadInfo downloadInfo = webClient.GetDownloadInfo(typedStore, unfulfilledShipWorksOrders);

            int pageNumberToDownload = downloadInfo.PageCount;
            if (pageNumberToDownload > 0)
            {
                // We have pages/orders that need to be downloaded
                while (pageNumberToDownload > 0)
                {
                    // Download the orders from Newegg and check for any discrepancies between ShipWorks and Newegg
                    IEnumerable<Order> orders = await webClient.DownloadOrders(typedStore, unfulfilledShipWorksOrders, pageNumberToDownload).ConfigureAwait(false);
                    foreach (Order order in orders)
                    {
                        NeweggOrderEntity orderToUpdate = unfulfilledShipWorksOrders.Find(o => o.OrderNumber == order.OrderNumber);
                        if (order.OrderStatusId != (int) orderToUpdate.OnlineStatusCode)
                        {
                            // There is a discrepancy between the order status that ShipWorks has and
                            // the one that Newegg has record of. Update the local order status data
                            // based on what is in Newegg
                            orderToUpdate.OnlineStatusCode = order.OrderStatusId;
                            orderToUpdate.OnlineStatus = order.OrderStatusDescription;

                            if (order.InvoiceNumber > 0)
                            {
                                // There is an invoice associated with this order
                                orderToUpdate.InvoiceNumber = order.InvoiceNumber;
                            }

                            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "NeweggDownloader.SyncUnfulfilledShipWorksOrdersWithNewegg");
                            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(orderToUpdate)).ConfigureAwait(false);
                        }
                    }

                    pageNumberToDownload--;

                    if (Progress.IsCancelRequested)
                    {
                        // The user canceled the download
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Fetches existing unfulfilled Newegg orders (orders not having an Invoiced or
        /// Voided status) from the ShipWorks database.
        /// </summary>
        /// <returns>A List of NeweggOrderEntity objects.</returns>
        private List<NeweggOrderEntity> GetUnfulfilledShipWorksOrders()
        {
            using (EntityCollection<NeweggOrderEntity> unfulfilledShipWorksOrders = new EntityCollection<NeweggOrderEntity>())
            {
                // A filter to restrict results only to this instance of our Newegg store
                IPredicateExpression storeFilter = new PredicateExpression(NeweggOrderFields.StoreID == Store.StoreID);

                // Build a filter for getting all unfulfilled orders (i.e. orders that are not in the
                // Invoiced or Voided status
                IPredicateExpression statusFilter = new PredicateExpression();
                statusFilter.Add(NeweggOrderFields.OnlineStatusCode == (int) NeweggOrderStatus.Unshipped);
                statusFilter.AddWithOr(NeweggOrderFields.OnlineStatusCode == (int) NeweggOrderStatus.Shipped);
                statusFilter.AddWithOr(NeweggOrderFields.OnlineStatusCode == (int) NeweggOrderStatus.PartiallyShipped);

                // Combine our store filter and status filter so the predicate is the equivalent of
                // (StoreID = [Our Store ID]) AND (OnlineStatusCode IN (Unshipped, Shipped, PartiallyShipped))
                IRelationPredicateBucket filter = new RelationPredicateBucket();
                filter.PredicateExpression.Add(storeFilter);
                filter.PredicateExpression.AddWithAnd(statusFilter);

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(unfulfilledShipWorksOrders, filter);
                }

                return unfulfilledShipWorksOrders.ToList();
            }
        }

        /// <summary>
        /// Downloads the orders from Newegg and imports them into ShipWorks.
        /// </summary>
        private async Task DownloadNewOrders(NeweggOrderType orderType)
        {
            var typedStore = Store as INeweggStoreEntity;

            // Get the initial metadata of the order download to figure out how many pages and
            // the total number of orders we'll be downloading based on our starting point.
            DateTime startingPoint = await GetDownloadStartingPoint().ConfigureAwait(false);
            DownloadInfo downloadInfo = await webClient.GetDownloadInfo(typedStore, startingPoint, orderType).ConfigureAwait(false);
            int pagesToDownload = downloadInfo.PageCount;

            if (pagesToDownload == 0)
            {
                // There's nothing to download, so update the progress accordingly
                Progress.PercentComplete = 100;
                Progress.Detail = "Done - no new orders to download.";
            }
            else
            {
                // There are orders that need to be downloaded, so begin downloading each
                // page and loading the orders.
                Progress.Detail = "Downloading new orders...";

                while (pagesToDownload > 0)
                {
                    // The Newegg API returns orders in descending order by order date, so get the pages in
                    // chronological order by starting from the last page - use the end date of our download
                    // info to ensure the data is consistent with the snapshot of the download info.
                    var downloadRange = downloadInfo.StartDate.To(downloadInfo.EndDate);
                    IEnumerable<Order> orders = await webClient.DownloadOrders(typedStore, downloadRange, pagesToDownload, orderType).ConfigureAwait(false);

                    // All the orders have been downloaded for the current page now load them into ShipWorks
                    // and decrement our page counter for the next iteration
                    await LoadOrders(downloadInfo, orders.ToList()).ConfigureAwait(false);
                    pagesToDownload--;

                    if (Progress.IsCancelRequested)
                    {
                        // The user canceled the download
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Loads a batch of orders into ShipWorks. The download info is used to update the
        /// progress of the total download/import process as there are multiple pages of
        /// orders that need to be downloaded.
        /// </summary>
        /// <param name="downloadInfo">The download info.</param>
        /// <param name="orders">The orders.</param>
        private async Task LoadOrders(DownloadInfo downloadInfo, List<Order> orders)
        {
            // Re-order the list by order date so in the event of the download being canceled
            // in the middle of loading the orders any missing orders get downloaded again.
            orders.Sort((a, b) => a.OrderDateInPacificStandardTime.CompareTo(b.OrderDateInPacificStandardTime));

            foreach (Order order in orders)
            {
                Progress.Detail = String.Format("Processing order {0} of {1}...", QuantitySaved + 1, downloadInfo.TotalOrders);

                await LoadOrder(order).ConfigureAwait(false);
                Progress.PercentComplete = Math.Min(100, 100 * (QuantitySaved) / downloadInfo.TotalOrders);

                if (Progress.IsCancelRequested)
                {
                    // The user canceled the download
                    return;
                }
            }
        }

        /// <summary>
        /// Gets the download starting point.
        /// </summary>
        /// <returns>A DateTime object.</returns>
        private async Task<DateTime> GetDownloadStartingPoint()
        {
            // We're going to have our starting point default to either the initial download days setting or a year back
            int previousDaysToDownload = Store.InitialDownloadDays.HasValue ? Store.InitialDownloadDays.Value : 365;
            DateTime startingPoint = DateTime.UtcNow.AddDays(-1 * previousDaysToDownload);

            DateTime? lastModifiedDate = await GetOnlineLastModifiedStartingPoint();
            if (lastModifiedDate.HasValue)
            {
                // We have a record of the last order date in the system, so
                // we're going to add a second to that value (to prevent
                // downloading a duplicate order) and use that as the starting point
                startingPoint = lastModifiedDate.Value.AddSeconds(1);
            }

            return startingPoint;
        }

        /// <summary>
        /// Loads the order based on the data downloaded from Newegg.
        /// </summary>
        /// <param name="downloadedOrder">The order data downloaded from the Newegg API.</param>
        private async Task LoadOrder(Order downloadedOrder)
        {
            OrderNumberIdentifier orderIdentifier = new OrderNumberIdentifier(downloadedOrder.OrderNumber);
            GenericResult<OrderEntity> result = await InstantiateOrder(orderIdentifier).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", downloadedOrder.OrderNumber, result.Message);
                return;
            }

            NeweggOrderEntity order = (NeweggOrderEntity) result.Value;

            order.OrderDate = downloadedOrder.OrderDateToUtcTime();
            order.OrderTotal = downloadedOrder.OrderTotalAmount;

            LoadAddress(downloadedOrder, order);

            order.OnlineStatusCode = downloadedOrder.OrderStatus;
            order.OnlineStatus = downloadedOrder.OrderStatusDescription;

            // Have seen orders where the shipping service value is null despite docs saying
            // this node should be in the response
            order.RequestedShipping = downloadedOrder.ShippingService ?? string.Empty;

            order.InvoiceNumber = downloadedOrder.InvoiceNumber;
            order.RefundAmount = downloadedOrder.RefundAmount;
            order.IsAutoVoid = downloadedOrder.IsAutoVoid;

            if (order.IsNew)
            {
                foreach (Item neweggItem in downloadedOrder.Items)
                {
                    LoadOrderItem(order, neweggItem);
                }

                LoadCharge(order, "SHIPPING", "Shipping", downloadedOrder.ShippingAmount);
                LoadCharge(order, "DISCOUNT", "Discount", downloadedOrder.DiscountAmount);
            }

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "NeweggDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the order item.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="neweggItem">The newegg item.</param>
        private void LoadOrderItem(NeweggOrderEntity order, Item neweggItem)
        {
            NeweggOrderItemEntity orderItem = InstantiateOrderItem(order) as NeweggOrderItemEntity;
            if (orderItem != null)
            {
                orderItem.Code = neweggItem.SellerPartNumber;
                orderItem.Name = neweggItem.Description;
                orderItem.Description = neweggItem.Description;
                orderItem.Quantity = neweggItem.QuantityOrdered;
                orderItem.SKU = neweggItem.ManufacturerPartNumber;
                orderItem.UnitPrice = neweggItem.UnitPrice;
                orderItem.UPC = neweggItem.UpcCode;

                orderItem.SellerPartNumber = neweggItem.SellerPartNumber;
                orderItem.NeweggItemNumber = neweggItem.NeweggItemNumber;
                orderItem.ManufacturerPartNumber = neweggItem.ManufacturerPartNumber;
                orderItem.ShippingStatusDescription = neweggItem.ShippingStatusDescription;
                orderItem.ShippingStatusID = neweggItem.ShippingStatusID;
                orderItem.QuantityShipped = neweggItem.QuantityShipped;
            }
        }

        /// <summary>
        /// Loads the address.
        /// </summary>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="order">The order.</param>
        private void LoadAddress(Order downloadedOrder, NeweggOrderEntity order)
        {
            order.BillUnparsedName = downloadedOrder.CustomerName;
            order.BillPhone = downloadedOrder.CustomerPhoneNumber;
            order.BillEmail = downloadedOrder.CustomerEmailAddress;
            order.BillFirstName = downloadedOrder.ShipToFirstName;
            order.BillLastName = downloadedOrder.ShipToLastName.Trim();
            order.BillStreet1 = downloadedOrder.ShipToAddress1;
            order.BillStreet2 = downloadedOrder.ShipToAddress2;
            order.BillCity = downloadedOrder.ShipToCity;
            order.BillCompany = downloadedOrder.ShipToCompany;
            order.BillStateProvCode = Geography.GetStateProvCode(downloadedOrder.ShipToState);
            order.BillPostalCode = downloadedOrder.ShipToZipCode;
            order.BillCountryCode = FormatUSACountryCode(downloadedOrder.ShipToCountryCode);


            order.ShipUnparsedName = downloadedOrder.CustomerName;
            order.ShipFirstName = downloadedOrder.ShipToFirstName;
            order.ShipLastName = downloadedOrder.ShipToLastName.Trim();
            order.ShipCompany = downloadedOrder.ShipToCompany;
            order.ShipStreet1 = downloadedOrder.ShipToAddress1;
            order.ShipStreet2 = downloadedOrder.ShipToAddress2;
            order.ShipCity = downloadedOrder.ShipToCity;
            order.ShipStateProvCode = Geography.GetStateProvCode(downloadedOrder.ShipToState);
            order.ShipCountryCode = FormatUSACountryCode(downloadedOrder.ShipToCountryCode);
            order.ShipPostalCode = downloadedOrder.ShipToZipCode;
            order.ShipPhone = downloadedOrder.CustomerPhoneNumber;
        }

        /// <summary>
        /// Formats the USA country code.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        private static string FormatUSACountryCode(string countryCode)
        {
            // Newegg is not using the ISO country code for the United States, so
            // check for it and convert it to "US"
            return countryCode == "USA" ? "US" : Geography.GetCountryCode(countryCode);
        }

        /// <summary>
        /// A helper method to record a charge associated an order
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="chargeType">Type of the charge.</param>
        /// <param name="chargeDescription">The charge description.</param>
        /// <param name="amount">The amount.</param>
        private void LoadCharge(OrderEntity order, string chargeType, string chargeDescription, decimal amount)
        {
            if (chargeType.ToLower() == "discount")
            {
                // Make sure the discount amount is deducted when the order
                // utility is verifying the order totals
                amount = amount * -1;
            }

            if (amount != 0M)
            {
                OrderChargeEntity charge = InstantiateOrderCharge(order);
                charge.Amount = amount;
                charge.Type = chargeType;
                charge.Description = chargeDescription;
            }
        }

    }
}
