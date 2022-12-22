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
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace ShipWorks.Stores.Platforms.Etsy
{
    // TODO: Update registration to use a Keyed Component to replace the MWS downloader
    /// <summary>
    /// Order downloader for Etsy stores via Platform
    /// </summary>
    [Component(RegistrationType.Self)]

    public class EtsyPlatformDownloader : PlatformDownloader
    {
        /// <summary>
        /// Gets the Etsy store entity
        /// </summary>
        private EtsyStoreEntity EtsyStore => (EtsyStoreEntity) Store;

        /// <summary>
        /// Object factory for the platform web client
        /// </summary>
        private readonly Func<EtsyStoreEntity, IPlatformOrderWebClient> createWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, Func<EtsyStoreEntity, IPlatformOrderWebClient> createWebClient)
            : base(store, storeTypeManager.GetType(store), storeManager)
        {
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Start the download from Platform for the Etsy store
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Connecting to Platform...";

                var client = createWebClient(EtsyStore);

                client.Progress = Progress;

                Progress.Detail = "Checking for new orders ";

                var result =
                    await client.GetOrders(Store.OrderSourceID, Store.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

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
                    EtsyStore.ContinuationToken = result.Orders.ContinuationToken;
                    await storeManager.SaveStoreAsync(EtsyStore).ConfigureAwait(false);

                    result = await client.GetOrders(Store.OrderSourceID, EtsyStore.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

                }

                Progress.PercentComplete = 100;
                Progress.Detail = "Done.";

                // There's an error within the refresh
                if (result.Error)
                {
                    // We only throw at the end to give the import a chance to process any orders that were provided.
                    throw new Exception(
                        "Connection to Etsy failed. Please try again. If it continues to fail, update your credentials in store settings or contact ShipWorks support.");
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
	            await LoadOrder(salesOrder);
            }
        }

        private async Task LoadOrder(OrderSourceApiSalesOrder salesOrder)
        {
            var etsyOrderId = salesOrder.OrderNumber;

            // get the order instance
            var result = await InstantiateOrder(long.Parse(etsyOrderId)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", etsyOrderId, result.Message);
                return;
            }

            var order = (EtsyOrderEntity) result.Value;
            order.ChannelOrderID = salesOrder.SalesOrderGuid;
            order.WasPaid = salesOrder.Payment.PaymentStatus == OrderSourcePaymentStatus.Paid;
            order.WasShipped = salesOrder.Status == OrderSourceSalesOrderStatus.Completed;

            if (salesOrder.Status == OrderSourceSalesOrderStatus.Cancelled && order.IsNew)
            {
                log.InfoFormat("Skipping order '{0}' due to canceled and not yet seen by ShipWorks.", etsyOrderId);
                return;
            }

            var orderDate = salesOrder.CreatedDateTime?.DateTime ?? DateTime.UtcNow;
            var modifiedDate = salesOrder.ModifiedDateTime?.DateTime ?? DateTime.UtcNow;

            //Basic properties
            order.OrderDate = orderDate;
            order.OnlineLastModified = modifiedDate >= orderDate ? modifiedDate : orderDate;
            
            // set the status
            var orderStatus = GetOrderStatusString(salesOrder, order.OrderNumberComplete);
            order.OnlineStatus = orderStatus;
            order.OnlineStatusCode = orderStatus;
            
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
                //if (EtsyStore.AmazonVATS != true)
                //{
                    var totalTax = salesOrder.RequestedFulfillments?
                        .SelectMany(f => f.Items)?
                        .SelectMany(i => i.Taxes)?
                        .Sum(t => t.Amount) ?? 0;

                    if (totalTax > 0)
                    {
                        AddToCharge(order, "Tax", "Tax", totalTax);
                    }
                //}

                // update the total
                var calculatedTotal = OrderUtility.CalculateTotal(order);

                // get the amount so we can fudge order totals
                order.OrderTotal = salesOrder.Payment.AmountPaid ?? calculatedTotal;
            }

            // save
            var retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "EtsyPlatformDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Attempts to figure out the Etsy status based on the Platform status
        /// </summary>
        /// <remarks>
        /// Unfortunately, this isn't a one to one to from Platform Status to Etsy Status. This
        /// is the code I used to "unmap" the platform mapping for existing filters:
        /// https://github.com/shipstation/integrations-vice/blob/master/ecommerce/modules/etsy/src/services/mappers/salesOrderExport.Mapper.js#L133
        /// </remarks>
        protected override string GetOrderStatusString(OrderSourceApiSalesOrder salesOrder, string orderId)
        {
            switch (salesOrder.Payment.PaymentStatus)
            {
                case OrderSourcePaymentStatus.PaymentInProcess:
                    return "Payment Processing";
            }
            switch (salesOrder.Status)
            {
                case OrderSourceSalesOrderStatus.PendingFulfillment:
                    return "Open";
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                    return "Paid";
                case OrderSourceSalesOrderStatus.Cancelled:
                    return "Cancelled";
                case OrderSourceSalesOrderStatus.Completed:
                    //return "Shipped"; - ambigous: in platform, both  Completed and Shipped are mapped to Completed
                    return "Completed";
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                    return "Unpaid";
            }
            log.Warn($"Encountered unmapped status of {salesOrder.Status} for orderId {orderId}.");
            return base.GetOrderStatusString(salesOrder, orderId);
        }
    }
}
