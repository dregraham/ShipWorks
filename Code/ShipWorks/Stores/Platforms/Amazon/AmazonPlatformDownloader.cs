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
using Interapptive.Shared.Metrics;
using log4net;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.ShipEngine;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.Amazon
{
    // TODO: Update registration to use a Keyed Component to replace the MWS downloader
    /// <summary>
    /// Order downloader for Amazon stores via Platform
    /// </summary>
    [Component(RegistrationType.Self)]
    public class AmazonPlatformDownloader : StoreDownloader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmazonPlatformDownloader));

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
        /// Store manager used to save the continuation token to the amazon store
        /// </summary>
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager, 
            IStoreManager storeManager, Func<AmazonStoreEntity, IPlatformOrderWebClient> createWebClient)
            : base(store, storeTypeManager.GetType(store))
        {
            this.storeManager = storeManager;
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
                    await client.GetOrders(Store.OrderSourceID, AmazonStore.ContinuationToken).ConfigureAwait(false);

                if (result.Errors.Count > 0)
                {
                    foreach (var platformError in result.Errors)
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
                await LoadOrders(result.Data).ConfigureAwait(false);

                // Save the continuation token to the store
                AmazonStore.ContinuationToken = result.ContinuationToken;
                await storeManager.SaveStoreAsync(AmazonStore);

                trackedDurationEvent.AddMetric("Amazon.Fba.Order.Count", FbaOrdersDownloaded);

                Progress.PercentComplete = 100;
                Progress.Detail = "Done.";
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
            foreach (var salesOrder in orders)
            {
                await LoadOrder(salesOrder);
            }
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
            order.OrderNumberComplete = amazonOrderId;
            order.OrderNumber = long.MinValue;
            order.ChannelOrderID = salesOrder.OrderId;

            var orderStatus = salesOrder.Status.ToString();

            if (string.Compare(orderStatus, "Canceled", StringComparison.OrdinalIgnoreCase) == 0 && order.IsNew)
            {
                log.InfoFormat("Skipping order '{0}' due to canceled and not yet seen by ShipWorks.", amazonOrderId);
                return;
            }

            // basic properties
            order.OrderDate = salesOrder.PaidDate?.UtcDateTime ?? DateTime.UtcNow;
            order.OnlineLastModified = salesOrder.ModifiedDateTime?.UtcDateTime ?? DateTime.UtcNow;

            // TODO: Don't appear to be provided
            order.EarliestExpectedDeliveryDate = null;
            order.LatestExpectedDeliveryDate = null;

            // set the status
            order.OnlineStatus = orderStatus;
            order.OnlineStatusCode = orderStatus;

            // Fulfilled by
            order.FulfillmentChannel = (int) AmazonMwsFulfillmentChannel.Unknown;

            // If the order is new and it is of Amazon fulfillment type, increase the FBA count.
            if (order.IsNew && order.FulfillmentChannel == (int) AmazonMwsFulfillmentChannel.AFN)
            {
                FbaOrdersDownloaded++;
            }

            // IsPrime
            if (salesOrder.OrderSourcePolicies.Count > 0)
            {
                order.IsPrime = (int) (salesOrder.OrderSourcePolicies.Any(x => x.IsPremiumProgram)
                    ? AmazonIsPrime.Yes
                    : AmazonIsPrime.No);
            }
            else
            {
                order.IsPrime = (int) AmazonIsPrime.Unknown;
            }
            
            // Purchase order number
            order.PurchaseOrderNumber = WebUtility.HtmlDecode(salesOrder.OriginalOrderSource.OrderId ?? string.Empty);

            // no customer ID in this Api
            order.OnlineCustomerID = null;

            // requested shipping
            order.RequestedShipping =
                salesOrder.RequestedFulfillments.FirstOrDefault()?.ShippingPreferences.ShippingService ?? string.Empty;

            // Address
            LoadAddresses(order, salesOrder);

            // only load order items on new orders
            if (order.IsNew)
            {
                order.OrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);
                
                foreach (var fulfillment in salesOrder.RequestedFulfillments)
                {
                    LoadOrderItems(fulfillment, order);
                }

                // update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);

                // get the amount so we can fudge order totals
                var orderAmount = salesOrder.Payment.AmountPaid;

                if (order.OrderTotal != orderAmount)
                {
                    var warning = string.Format("Order '{0} total should have been {1}, but was calculated as {2}", order.AmazonOrderID, orderAmount, order.OrderTotal);
                    log.WarnFormat(warning);

                    Debug.Fail(warning);
                }
            }

            // save
            var retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "AmazonPlatformDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        private static void LoadAddresses(AmazonOrderEntity order, OrderSourceApiSalesOrder salesOrder)
        {
            var shipTo = salesOrder.RequestedFulfillments.FirstOrDefault(x => x?.ShipTo != null)?.ShipTo;
            if (shipTo == null || !order.IsNew)
            {
                return;
            }

            var shipFullName = PersonName.Parse(shipTo.Name ?? string.Empty);
            order.ShipFirstName = shipFullName.First;
            order.ShipMiddleName = shipFullName.Middle;
            order.ShipLastName = shipFullName.LastWithSuffix;
            order.ShipNameParseStatus = (int) shipFullName.ParseStatus;
            order.ShipUnparsedName = shipFullName.UnparsedName;
            order.ShipCompany = shipTo.Company;
            order.ShipPhone = shipTo.Phone ?? string.Empty;

            order.ShipStreet1 = shipTo.AddressLine1 ?? string.Empty;
            order.ShipStreet2 = shipTo.AddressLine2 ?? string.Empty;
            order.ShipStreet3 = shipTo.AddressLine3 ?? string.Empty;

            order.ShipCity = shipTo.City ?? string.Empty;
            order.ShipPostalCode = shipTo.PostalCode ?? string.Empty;
            order.ShipCountryCode = Geography.GetCountryCode(shipTo.CountryCode ?? string.Empty);
            order.ShipStateProvCode = Geography.GetStateProvCode(shipTo.StateProvince ?? string.Empty, order.ShipCountryCode);
            
            order.ShipEmail = order.BillEmail ?? string.Empty;

            // Bill To
            var billToFullName = PersonName.Parse(salesOrder.BillTo.Name ?? string.Empty);
            order.BillFirstName = billToFullName.First;
            order.BillMiddleName = billToFullName.Middle;
            order.BillLastName = billToFullName.LastWithSuffix;
            order.BillNameParseStatus = (int) billToFullName.ParseStatus;
            order.BillUnparsedName = billToFullName.UnparsedName;
            order.BillCompany = salesOrder.BillTo.Company;
            order.BillPhone = salesOrder.BillTo.Phone ?? string.Empty;

            order.BillStreet1 = salesOrder.BillTo.AddressLine1 ?? string.Empty;
            order.BillStreet2 = salesOrder.BillTo.AddressLine2 ?? string.Empty;
            order.BillStreet3 = salesOrder.BillTo.AddressLine3 ?? string.Empty;

            order.BillCity = salesOrder.BillTo.City ?? string.Empty;
            order.BillPostalCode = salesOrder.BillTo.PostalCode ?? string.Empty;
            order.BillCountryCode = Geography.GetCountryCode(salesOrder.BillTo.CountryCode ?? string.Empty);
            order.BillStateProvCode = Geography.GetStateProvCode(salesOrder.BillTo.StateProvince ?? string.Empty, order.ShipCountryCode);

            order.BillEmail = salesOrder.Buyer.Email ?? string.Empty;
        }

        /// <summary>
        /// Set the buyer name while downloading an order
        /// </summary>
        private static void SetBuyerName(AmazonOrderEntity order, string buyerFullName)
        {
            // parse the name
            var buyerName = PersonName.Parse(buyerFullName);
            order.BillFirstName = buyerName.First;
            order.BillMiddleName = buyerName.Middle;
            order.BillLastName = buyerName.LastWithSuffix;
            order.BillNameParseStatus = (int) buyerName.ParseStatus;
            order.BillUnparsedName = buyerName.UnparsedName;

            // If first and last name on the buyer are the same as the shipping name, copy the rest of the address too
            if ((string.Equals(order.BillFirstName, order.ShipFirstName, StringComparison.OrdinalIgnoreCase)) &&
                (string.Equals(order.BillLastName, order.ShipLastName, StringComparison.OrdinalIgnoreCase)))
            {
                // until Amazon provides some billing information, copy everything to billing from shipping
                PersonAdapter.Copy(new PersonAdapter(order, "Ship"), new PersonAdapter(order, "Bill"));
            }
        }

        /// <summary>
        /// Loads the order items of an amazon order
        /// </summary>
        private void LoadOrderItems(RequestedFulfillment fulfillment, AmazonOrderEntity order)
        {
            foreach (var item in fulfillment.Items)
            {
                LoadOrderItem(item, order);
            }
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadOrderItem(SalesOrderItem orderItem, AmazonOrderEntity order)
        {
            var item = (AmazonOrderItemEntity) InstantiateOrderItem(order);
            
            // populate the basics
            item.Name = orderItem.Product.Name;
            item.Quantity = orderItem.Quantity;
            item.UnitPrice = orderItem.UnitPrice;
            item.SKU = orderItem.Product.Identifiers.Sku;
            item.Code = item.SKU;

            item.Weight = (double)orderItem.Product.Weight.Value;

            item.Thumbnail = orderItem.Product.Urls?.ThumbnailUrl;
            item.Image = orderItem.Product.Urls?.ImageUrl;

            item.Length = orderItem.Product.Dimensions?.Length ?? 0;
            item.Width = orderItem.Product.Dimensions?.Width ?? 0;
            item.Height = orderItem.Product.Dimensions?.Height ?? 0;

            // amazon-specific fields
            item.AmazonOrderItemCode = orderItem.LineItemId;
            //TODO amz:ConditionNote
                        item.ASIN = orderItem.Product.Identifiers?.Asin;

            // see if we need to add any attributes
            SetOrderItemGiftDetails(orderItem, item);

            // add an attribute for each promotion
            // TODO: amz:PromotionIds/amz:PromotionId

            AddOrderItemCharges(orderItem, order);
        }

        /// <summary>
        /// Add item charges to the order
        /// </summary>
        private void AddOrderItemCharges(SalesOrderItem orderItem, AmazonOrderEntity order)
        {
            // Charges
            if (AmazonStore.AmazonVATS != true)
            {
                foreach (var orderItemTax in orderItem.Taxes)
                {
                    AddToCharge(order, "Tax", orderItemTax.Description, orderItemTax.Amount);
                }
            }

            foreach (var orderItemAdjustment in orderItem.Adjustments)
            {
                AddToCharge(order, "Discount", orderItemAdjustment.Description, -orderItemAdjustment.Amount);
            }

            foreach (var orderItemShippingCharge in orderItem.ShippingCharges)
            {
                AddToCharge(order, "Shipping", orderItemShippingCharge.Description, orderItemShippingCharge.Amount);
            }
        }

        /// <summary>
        /// Locates an order's charge (or creates it) and adds the value
        /// </summary>
        private void AddToCharge(OrderEntity order, string chargeType, string name, decimal amount)
        {
            // Don't need to create 0-value charges
            if (amount == 0)
            {
                return;
            }

            var charge = order.OrderCharges.FirstOrDefault(c => String.Compare(c.Type, chargeType.ToUpper(), StringComparison.OrdinalIgnoreCase) == 0);
            if (charge == null)
            {
                // first one, create it
                charge = InstantiateOrderCharge(order);
                charge.Type = chargeType.ToUpper();
                charge.Description = name;
                charge.Amount = 0;
            }

            charge.Amount += amount;
        }

        /// <summary>
        /// Set gift details on an order item
        /// </summary>
        private void SetOrderItemGiftDetails(SalesOrderItem orderItem, AmazonOrderItemEntity item)
        {
            // TODO
        }
    }
}
