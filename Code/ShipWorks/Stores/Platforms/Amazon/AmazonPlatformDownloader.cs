using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace ShipWorks.Stores.Platforms.Amazon
{
    // TODO: Update registration to use a Keyed Component to replace the MWS downloader
    /// <summary>
    /// Order downloader for Amazon stores via Platform
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Amazon)]
    public class AmazonPlatformDownloader : PlatformDownloader
    {
        /// <summary>
        /// Count of FBA orders in a Download call.
        /// </summary>
        public int FbaOrdersDownloaded { get; private set; }

        /// <summary>
        /// Gets the Amazon store entity
        /// </summary>
        private AmazonStoreEntity AmazonStore => (AmazonStoreEntity) Store;

        /// <summary>
        /// Object factory for the platform web client
        /// </summary>
        private readonly Func<AmazonStoreEntity, IPlatformOrderWebClient> createWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, Func<AmazonStoreEntity, IPlatformOrderWebClient> createWebClient)
            : base(store, storeTypeManager.GetType(store), storeManager)
        {
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Start the download from Platform for the Amazon store
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                FbaOrdersDownloaded = 0;

                Progress.Detail = "Connecting to Platform...";

                var client = createWebClient(AmazonStore);

                client.Progress = Progress;

                Progress.Detail = "Checking for new orders ";

                var result =
                    await client.GetOrders(Store.OrderSourceID, AmazonStore.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

                while (result.Orders.Data.Any())
                {
                    if (result.Orders.Errors.Count > 0)
                    {
                        foreach (var platformError in result.Orders.Errors)
                        {
                            log.Error(platformError);
                        }

                        return;
                    }

                    if (Progress.IsCancelRequested)
                    {
                        log.Warn("A cancel was requested.");
                        return;
                    }

                    // progress has to be indicated on each pass since we have 0 idea how many orders exists
                    Progress.PercentComplete = 0;

                    // load each order in this result page
                    await LoadOrders(result.Orders.Data).ConfigureAwait(false);

                    // Save the continuation token to the store
                    AmazonStore.ContinuationToken = result.Orders.ContinuationToken;
                    await storeManager.SaveStoreAsync(AmazonStore).ConfigureAwait(false);

                    result = await client.GetOrders(Store.OrderSourceID, AmazonStore.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

                }

                trackedDurationEvent.AddMetric("Amazon.Fba.Order.Count", FbaOrdersDownloaded);

                Progress.PercentComplete = 100;
                Progress.Detail = "Done.";

                // There's an error within the refresh
                if (result.Error)
                {
                    // We only throw at the end to give the import a chance to process any orders that were provided.
                    throw new Exception(
                        "Connection to Amazon failed. Please try again. If it continues to fail, update your credentials in store settings or contact ShipWorks support.");
                }
            }
            catch (Exception ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Load orders from a page of results
        /// </summary>
        private async Task LoadOrders(ICollection<OrderSourceApiSalesOrder> orders)
        {
            foreach (var salesOrder in orders.Where(x => x.Status != OrderSourceSalesOrderStatus.AwaitingPayment))
            {
                var fulfillmentChannel = GetFulfillmentChannel(salesOrder);
                if (!(AmazonStore.ExcludeFBA && fulfillmentChannel == AmazonMwsFulfillmentChannel.AFN))
                {
                    await LoadOrder(salesOrder);
                }
            }
        }

        /// <summary>
        /// Get the fulfillment channel from a sales order
        /// </summary>
        private static AmazonMwsFulfillmentChannel GetFulfillmentChannel(OrderSourceApiSalesOrder salesOrder)
        {
            if(salesOrder.FulfillmentChannel == "MFN")
            {
                return AmazonMwsFulfillmentChannel.MFN;
            } 
            else if(salesOrder.FulfillmentChannel == "AFN")
            {
                return AmazonMwsFulfillmentChannel.AFN;
            }

            return AmazonMwsFulfillmentChannel.Unknown;
        }

        private async Task LoadOrder(OrderSourceApiSalesOrder salesOrder)
        {
            var amazonOrderId = salesOrder.OrderNumber;

            // get the order instance
            var result = await InstantiateOrder(new AmazonOrderIdentifier(amazonOrderId)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", amazonOrderId, result.Message);
                return;
            }

            var order = (AmazonOrderEntity) result.Value;
            order.AmazonOrderID = amazonOrderId;
            order.ChannelOrderID = salesOrder.SalesOrderGuid;

            if (salesOrder.Status == OrderSourceSalesOrderStatus.Cancelled && order.IsNew)
            {
                log.InfoFormat("Skipping order '{0}' due to canceled and not yet seen by ShipWorks.", amazonOrderId);
                return;
            }

            var orderDate = salesOrder.CreatedDateTime?.DateTime ?? DateTime.UtcNow;
            var modifiedDate = salesOrder.ModifiedDateTime?.DateTime ?? DateTime.UtcNow;

            //Basic properties
            order.OrderDate = orderDate;
            order.OnlineLastModified = modifiedDate >= orderDate ? modifiedDate : orderDate;

            // Platform may provide this in the future, but this isn't MVP
            order.EarliestExpectedDeliveryDate = null;
            order.LatestExpectedDeliveryDate = salesOrder.RequestedFulfillments?.Max(f => f?.ShippingPreferences?.DeliverByDate)?.DateTime;

            // set the status
            var orderStatus = GetAmazonStatus(salesOrder.Status, order.OrderNumberComplete);
            order.OnlineStatus = orderStatus;
            order.OnlineStatusCode = orderStatus;

            order.FulfillmentChannel = (int) GetFulfillmentChannel(salesOrder);
            
            // If the order is new and it is of Amazon fulfillment type, increase the FBA count.
            if (order.IsNew && order.FulfillmentChannel == (int) AmazonMwsFulfillmentChannel.AFN)
            {
                FbaOrdersDownloaded++;
            }

            // We keep this at the order level and it is at the item level. So, if they are all prime or all not prime we set Yes/No. In ohter cases, Unknown
            var isPrime = AmazonIsPrime.Unknown;
            if (salesOrder.RequestedFulfillments.All(f => f.ShippingPreferences?.IsPremiumProgram ?? false))
            {
                isPrime = AmazonIsPrime.Yes;
            }
            else if (salesOrder.RequestedFulfillments.All(f => (!f.ShippingPreferences?.IsPremiumProgram) ?? false))
            {
                isPrime = AmazonIsPrime.No;
            }
            order.IsPrime = (int) isPrime;

            // Purchase order number
            order.PurchaseOrderNumber = salesOrder.Payment?.PurchaseOrderNumber;

            // no customer ID in this Api
            order.OnlineCustomerID = null;

            // requested shipping
            order.RequestedShipping =
                GetRequestedShipping(salesOrder.RequestedFulfillments.FirstOrDefault()?.ShippingPreferences?.ShippingService);

            // Address
            LoadAddresses(order, salesOrder);

            // only load order items on new orders
            if (order.IsNew)
            {
                order.OrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);

                var giftNotes = GetGiftNotes(salesOrder);
                IEnumerable<CouponCode> couponCodes = GetCouponCodes(salesOrder);
                foreach (var fulfillment in salesOrder.RequestedFulfillments)
                {
                    LoadOrderItems(fulfillment, order, giftNotes, couponCodes);
                }

                // Taxes
                if (AmazonStore.AmazonVATS != true)
                {
                    var totalTax = salesOrder.RequestedFulfillments?
                        .SelectMany(f => f.Items)?
                        .SelectMany(i => i.Taxes)?
                        .Sum(t => t.Amount) ?? 0;

                    if (totalTax > 0)
                    {
                        AddToCharge(order, "Tax", "Tax", totalTax);
                    }
                }

                // update the total
                var calculatedTotal = OrderUtility.CalculateTotal(order);

                // get the amount so we can fudge order totals
                order.OrderTotal = salesOrder.Payment.AmountPaid ?? calculatedTotal;
            }

            // save
            var retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "AmazonPlatformDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }


        /// <summary>
        /// GetRequestedShipping (in the format we used to get it from MWS "carrier: details")
        /// </summary
        private string GetRequestedShipping(string shippingService)
        {
            if (string.IsNullOrWhiteSpace(shippingService))
            {
                return string.Empty;
            }

            var firstSpace = shippingService.IndexOf(' ');
            if (firstSpace == -1)
            {
                return shippingService;
            }

            return $"{shippingService.Substring(0, firstSpace)}:{shippingService.Substring(firstSpace)}";
        }

        /// <summary>
        /// Attempts to figure out the Amazon status based on the Platform status
        /// </summary>
        /// <remarks>
        /// Unfortunately, this isn't a one to one to from Platform Status to Amazon Status. This
        /// is the code I used to "unmap" the platform mapping for existing filters:
        /// https://github.com/shipstation/integrations-ecommerce/blob/915ffd7a42f22ae737bf7d277e69409c3cf1b845/modules/amazon-order-source/src/methods/mappers/sales-orders-export-mappers.ts#L150
        /// </remarks>
        private string GetAmazonStatus(OrderSourceSalesOrderStatus platformStatus, string orderId)
        {
            switch (platformStatus)
            {
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                    return "Unshipped";
                case OrderSourceSalesOrderStatus.Cancelled:
                    return "Cancelled";
                case OrderSourceSalesOrderStatus.Completed:
                    return "Shipped";
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                    return "Pending";
                case OrderSourceSalesOrderStatus.OnHold:
                default:
                    log.Warn($"Encountered unmapped status of {platformStatus} for orderId {orderId}.");
                    return "Unknown";
            }
        }
    }
}
