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
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify
{
    // TODO: Update registration to use a Keyed Component to replace the MWS downloader
    /// <summary>
    /// Order downloader for Shopify stores via Platform
    /// </summary>
    [Component(RegistrationType.Self)]

    public class ShopifyPlatformDownloader : PlatformDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, IPlatformOrderWebClient platformOrderWebClient)
            : base(store, storeTypeManager.GetType(store), storeManager, platformOrderWebClient)
        {
        }


        protected override async Task<OrderEntity> CreateOrder(OrderSourceApiSalesOrder salesOrder)
        {
            long shopifyOrderId = long.Parse(salesOrder.OrderId);

            GenericResult<OrderEntity> result = await InstantiateOrder(
                    new ShopifyOrderIdentifier(shopifyOrderId),
                    new[] { EntityType.OrderPaymentDetailEntity })
                .ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", shopifyOrderId, result.Message);
                return null;
            }

            var order = (ShopifyOrderEntity) result.Value;
            //unshipped
            //partial
            //fulfilled
            //restocked
            //unknown
            order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Unknown;
            
            //authorized
            //pending
            //paid
            //abandoned
            //refunded
            //voided
            //partially_refunded
            //partially_paid
            //unknown
            order.PaymentStatusCode = (int) ShopifyPaymentStatus.Unknown;

            switch (salesOrder.Status)
            {
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Pending; break;
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                    order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Unshipped; break;
                case OrderSourceSalesOrderStatus.Cancelled:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Abandoned; break;
                case OrderSourceSalesOrderStatus.Completed:
                    order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled; break;
                    //statuses not mapped in platform:
                    //case OrderSourceSalesOrderStatus.OnHold:
                    //    order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled; break;
                    //case OrderSourceSalesOrderStatus.PendingFulfillment:
                    //    order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Partial; break;
            }

            switch (salesOrder.Payment.PaymentStatus)
            {
                case OrderSourcePaymentStatus.AwaitingPayment:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Pending; break;
                case OrderSourcePaymentStatus.PaymentCancelled:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Abandoned; break;
                case OrderSourcePaymentStatus.PaymentFailed:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Pending; break;
                case OrderSourcePaymentStatus.PaymentInProcess:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Authorized; break;
                case OrderSourcePaymentStatus.Paid:
                    order.PaymentStatusCode = (int) ShopifyPaymentStatus.Paid; break;
            }
            return order;
        }

        /// <summary>
        /// Attempts to figure out the Shopify status based on the Platform status
        /// </summary>
        /// <remarks>
        /// Unfortunately, this isn't a one to one to from Platform Status to Shopify Status. This
        /// is the code I used to "unmap" the platform mapping for existing filters:
        /// https://github.com/shipstation/integrations-ecommerce/blob/master/modules/shopify/src/methods/sales-orders-export.ts
        /// </remarks>
        protected override string GetOrderStatusString(OrderSourceApiSalesOrder salesOrder, string orderId)
        {
            switch (salesOrder.Status)
            {
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                case OrderSourceSalesOrderStatus.PendingFulfillment:
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                case OrderSourceSalesOrderStatus.OnHold:
                    return EnumHelper.GetDescription(ShopifyStatus.Open);
                case OrderSourceSalesOrderStatus.Cancelled:
                    return EnumHelper.GetDescription(ShopifyStatus.Canceled);
                case OrderSourceSalesOrderStatus.Completed:
                    return EnumHelper.GetDescription(ShopifyStatus.Closed);
                    //not mapped:
                    //return EnumHelper.GetDescription(ShopifyStatus.Shipped);
            }
            log.Warn($"Encountered unmapped status of {salesOrder.Status} for orderId {orderId}.");
            return base.GetOrderStatusString(salesOrder, orderId);
        }

        protected override OrderItemEntity LoadOrderItem(OrderSourceSalesOrderItem orderItem, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            var item = (ShopifyOrderItemEntity) base.LoadOrderItem(orderItem, order, giftNotes, couponCodes);
            //TODO:
            //item.InventoryItemID =
            //item.ShopifyProductID;
            //item.ShopifyOrderItemID=

            //item.TransactionID = orderItem.LineItemId;

            //var productListing = orderItem.Product?.ProductId;//"productId": "3114960238:653614320"
            //int length;
            //if (productListing != null && ((length = productListing.IndexOf(":")) > 0))
            //{
            //    item.ListingID = productListing.Substring(length+1);
            //}
            return item;
        }
    }
}
