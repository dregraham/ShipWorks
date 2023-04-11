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
        private int fbaOrdersDownloaded;

        /// <summary>
        /// Gets the Amazon store entity
        /// </summary>
        private AmazonStoreEntity AmazonStore => (AmazonStoreEntity) Store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, IPlatformOrderWebClient platformOrderWebClient)
            : base(store, storeTypeManager.GetType(store), storeManager, platformOrderWebClient)
        {
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

        protected override void CollectDownloadTelemetry(TrackedDurationEvent trackedDurationEvent)
        {
            trackedDurationEvent.AddMetric("Amazon.Fba.Order.Count", fbaOrdersDownloaded);
            base.CollectDownloadTelemetry(trackedDurationEvent);
        }

        protected override async Task<OrderEntity> CreateOrder(OrderSourceApiSalesOrder salesOrder)
        {
            if(salesOrder.Status == OrderSourceSalesOrderStatus.AwaitingPayment)
            {
                return null;
            }

            var fulfillmentChannel = GetFulfillmentChannel(salesOrder);
            if (AmazonStore.ExcludeFBA && fulfillmentChannel == AmazonMwsFulfillmentChannel.AFN)
            {
                return null;
            }

            var amazonOrderId = salesOrder.OrderNumber;

            // get the order instance
            var result = await InstantiateOrder(new AmazonOrderIdentifier(amazonOrderId)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", amazonOrderId, result.Message);
                return null;
            }

            var order = (AmazonOrderEntity) result.Value;
            order.AmazonOrderID = amazonOrderId;
            if (order.IsNew)
            {
                order.OrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);
            }

            // Platform may provide this in the future, but this isn't MVP
            order.EarliestExpectedDeliveryDate = null;
            order.LatestExpectedDeliveryDate = salesOrder.RequestedFulfillments?.Max(f => f?.ShippingPreferences?.DeliverByDate)?.DateTime;

            order.FulfillmentChannel = (int) fulfillmentChannel;

            // If the order is new and it is of Amazon fulfillment type, increase the FBA count.
            if (order.IsNew && fulfillmentChannel == AmazonMwsFulfillmentChannel.AFN && salesOrder.Status != OrderSourceSalesOrderStatus.Cancelled)
            {
                fbaOrdersDownloaded++;
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

            return order;
        }

        protected override void AddTaxes(OrderSourceApiSalesOrder salesOrder, OrderEntity order)
        {
            if (AmazonStore.AmazonVATS)
            {
                return;
            }
            base.AddTaxes(salesOrder, order);
        }

        protected override OrderItemEntity LoadOrderItem(OrderSourceSalesOrderItem orderItem, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            var item = (AmazonOrderItemEntity) base.LoadOrderItem(orderItem, order, giftNotes, couponCodes);

            // amazon-specific fields
            item.AmazonOrderItemCode = orderItem.LineItemId;
            item.ASIN = orderItem.Product.Identifiers?.Asin;
            item.ConditionNote = orderItem.Product?.Details?.FirstOrDefault((d) => d.Name == "Condition")?.Value;

            return item;
        }

        /// <summary>
        /// Attempts to figure out the Amazon status based on the Platform status
        /// </summary>
        /// <remarks>
        /// Unfortunately, this isn't a one to one to from Platform Status to Amazon Status. This
        /// is the code I used to "unmap" the platform mapping for existing filters:
        /// https://github.com/shipstation/integrations-ecommerce/blob/915ffd7a42f22ae737bf7d277e69409c3cf1b845/modules/amazon-order-source/src/methods/mappers/sales-orders-export-mappers.ts#L150
        /// </remarks>
        protected override string GetOrderStatusString(OrderSourceApiSalesOrder salesOrder, string orderId)
        {
            switch (salesOrder.Status)
            {
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                    return "Unshipped";
                case OrderSourceSalesOrderStatus.Cancelled:
                    return "Cancelled";
                case OrderSourceSalesOrderStatus.Completed:
                    return "Shipped";
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                    return "Pending";
            }
            log.Warn($"Encountered unmapped status of {salesOrder.Status} for orderId {orderId}.");
            return base.GetOrderStatusString(salesOrder, orderId);
        }

		/// <summary>
		/// GetRequestedShipping (in the format we used to get it from MWS "carrier: details")
		/// </summary
		protected override string GetRequestedShipping(string shippingService)
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
	}
}
