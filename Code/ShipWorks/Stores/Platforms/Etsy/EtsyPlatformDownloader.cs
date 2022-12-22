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

#nullable enable
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
        /// Constructor
        /// </summary>
        public EtsyPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, IPlatformOrderWebClient platformOrderWebClient)
            : base(store, storeTypeManager.GetType(store), storeManager, platformOrderWebClient)
        {
        }

        protected override async Task<OrderEntity?> CreateOrder(OrderSourceApiSalesOrder salesOrder)
        {
            var etsyOrderId = salesOrder.OrderNumber;

            var result = await InstantiateOrder(long.Parse(etsyOrderId)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", etsyOrderId, result.Message);
                return null;
            }

            var order = (EtsyOrderEntity) result.Value;
            order.WasPaid = salesOrder.Payment.PaymentStatus == OrderSourcePaymentStatus.Paid;
            order.WasShipped = salesOrder.Status == OrderSourceSalesOrderStatus.Completed;
            return order;
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

        protected override OrderItemEntity LoadOrderItem(OrderSourceSalesOrderItem orderItem, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            var item = (EtsyOrderItemEntity) base.LoadOrderItem(orderItem, order, giftNotes, couponCodes);
            item.TransactionID = orderItem.LineItemId;
            
            var productListing = orderItem.Product?.ProductId;//"productId": "3114960238:653614320"
            int length;
            if (productListing != null && ((length = productListing.IndexOf(":")) > 0))
            {
                item.ListingID = productListing.Substring(length+1);
            }
            return item;
        }
    }
}
