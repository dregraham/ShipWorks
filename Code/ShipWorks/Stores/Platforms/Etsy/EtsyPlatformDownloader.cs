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
    //[KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Etsy)]

    public class EtsyPlatformDownloader : StoreDownloader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EtsyPlatformDownloader));

        /// <summary>
        /// Count of FBA orders in a Download call.
        /// </summary>
        public int FbaOrdersDownloaded { get; private set; }

        /// <summary>
        /// Gets the Etsy store entity
        /// </summary>
        private EtsyStoreEntity EtsyStore => (EtsyStoreEntity) Store;

        /// <summary>
        /// Object factory for the platform web client
        /// </summary>
        private readonly Func<EtsyStoreEntity, IPlatformOrderWebClient> createWebClient;

        /// <summary>
        /// Store manager used to save the continuation token to the amazon store
        /// </summary>
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyPlatformDownloader(StoreEntity store, IStoreTypeManager storeTypeManager,
            IStoreManager storeManager, Func<EtsyStoreEntity, IPlatformOrderWebClient> createWebClient)
            : base(store, storeTypeManager.GetType(store))
        {
            this.storeManager = storeManager;
            this.createWebClient = createWebClient;
        }

        /// <summary>
        /// Start the download from Platform for the Etsy store
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
				//FbaOrdersDownloaded - To be removed or replaced !!
				FbaOrdersDownloaded = 0;

                Progress.Detail = "Connecting to Platform...";

                var client = createWebClient(EtsyStore);

                client.Progress = Progress;

                Progress.Detail = "Checking for new orders ";

                var result =
                    await client.GetOrders(Store.OrderSourceID, EtsyStore.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

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

                trackedDurationEvent.AddMetric("Etsy.Fba.Order.Count", FbaOrdersDownloaded);

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
            order.WasPaid = false;//TODO
            order.WasShipped= false;//TODO

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
            var orderStatus = GetOrderStatus(salesOrder.Status, order.OrderNumberComplete);
            order.OnlineStatus = orderStatus;
            order.OnlineStatusCode = orderStatus;
            
            // If the order is new and it is of Etsy fulfillment type, increase the FBA count.
            if (order.IsNew)
            {
                FbaOrdersDownloaded++;
            }

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

                foreach (var fulfillment in salesOrder.RequestedFulfillments)
                {
                    LoadOrderItems(fulfillment, order);
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
        /// Attempts to figure out the Etsy status based on the Platform status
        /// </summary>
        /// <remarks>
        /// Unfortunately, this isn't a one to one to from Platform Status to Etsy Status. This
        /// is the code I used to "unmap" the platform mapping for existing filters:
        /// https://github.com/shipstation/integrations-ecommerce/blob/915ffd7a42f22ae737bf7d277e69409c3cf1b845/modules/amazon-order-source/src/methods/mappers/sales-orders-export-mappers.ts#L150
        /// </remarks>
        private static string GetOrderStatus(OrderSourceSalesOrderStatus platformStatus, string orderId)
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

        /// <summary>
        /// Sets the XXXStreet1 - XXXStreet3 address lines
        /// </summary>
        private static void SetStreetAddress(PersonAdapter address, List<string> addressLines)
        {
            // first get rid of blanks
            addressLines.RemoveAll(s => s.Length == 0);

            var targetLine = 0;
            foreach (var addressLine in addressLines)
            {
                targetLine++;

                switch (targetLine)
                {
                    case 1:
                        address.Street1 = addressLine;
                        break;
                    case 2:
                        address.Street2 = addressLine;
                        break;
                    case 3:
                        address.Street3 = addressLine;
                        break;
                }
            }
        }

        private static void LoadAddresses(EtsyOrderEntity order, OrderSourceApiSalesOrder salesOrder)
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

            var shipAddressLines = new List<string>
            {
                shipTo.AddressLine1 ?? string.Empty,
                shipTo.AddressLine2 ?? string.Empty,
                shipTo.AddressLine3 ?? string.Empty
            };
            SetStreetAddress(new PersonAdapter(order, "Ship"), shipAddressLines);

            order.ShipCity = shipTo.City ?? string.Empty;
            order.ShipPostalCode = shipTo.PostalCode ?? string.Empty;
            order.ShipCountryCode = Geography.GetCountryCode(shipTo.CountryCode ?? string.Empty);
            order.ShipStateProvCode = Geography.GetStateProvCode(shipTo.StateProvince ?? string.Empty, order.ShipCountryCode);

            // Platform only provides one email
            order.ShipEmail = salesOrder.Buyer.Email ?? string.Empty;
            order.BillEmail = order.ShipEmail;

            // Bill To
            var billToFullName = PersonName.Parse(salesOrder.BillTo.Name ?? salesOrder.Buyer.Name ?? string.Empty);
            order.BillFirstName = billToFullName.First;
            order.BillMiddleName = billToFullName.Middle;
            order.BillLastName = billToFullName.LastWithSuffix;
            order.BillNameParseStatus = (int) billToFullName.ParseStatus;
            order.BillUnparsedName = billToFullName.UnparsedName;
            order.BillCompany = salesOrder.BillTo.Company;
            order.BillPhone = salesOrder.BillTo.Phone ?? salesOrder.Buyer.Phone ?? string.Empty;

            var billAddressLines = new List<string>
            {
                salesOrder.BillTo.AddressLine1 ?? string.Empty,
                salesOrder.BillTo.AddressLine2 ?? string.Empty,
                salesOrder.BillTo.AddressLine3 ?? string.Empty
            };
            SetStreetAddress(new PersonAdapter(order, "Bill"), billAddressLines);

            order.BillCity = salesOrder.BillTo.City ?? string.Empty;
            order.BillPostalCode = salesOrder.BillTo.PostalCode ?? string.Empty;
            order.BillCountryCode = Geography.GetCountryCode(salesOrder.BillTo.CountryCode ?? string.Empty);
            order.BillStateProvCode = Geography.GetStateProvCode(salesOrder.BillTo.StateProvince ?? string.Empty, order.BillCountryCode);
        }

        /// <summary>
        /// Loads the order items of an amazon order
        /// </summary>
        private void LoadOrderItems(OrderSourceRequestedFulfillment fulfillment, EtsyOrderEntity order)
        {
            foreach (var item in fulfillment.Items)
            {
	            LoadOrderItem(item, order);
            }
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadOrderItem(OrderSourceSalesOrderItem orderItem, EtsyOrderEntity order)
        {
            var item = (EtsyOrderItemEntity) InstantiateOrderItem(order);

            // populate the basics
            item.Name = orderItem.Product.Name;
            item.Quantity = orderItem.Quantity;
            item.UnitPrice = orderItem.UnitPrice;
            item.SKU = orderItem.Product.Identifiers.Sku;
            item.Code = item.SKU;

            var fromWeightUnit = PlatformUnitConverter.FromPlatformWeight(orderItem.Product.Weight.Unit);
            var weight = WeightUtility.Convert(fromWeightUnit, WeightUnitOfMeasure.Pounds, (double) orderItem.Product.Weight.Value);
            item.Weight = weight;

            PopulateUrls(orderItem, item);

            if (orderItem.Product?.Dimensions != null)
            {
                var dims = orderItem.Product.Dimensions;
                var fromDimUnit = dims.Unit;

                item.Length = (decimal) PlatformUnitConverter.ConvertDimension(dims.Length, fromDimUnit);
                item.Width = (decimal) PlatformUnitConverter.ConvertDimension(dims.Width, fromDimUnit);
                item.Height = (decimal) PlatformUnitConverter.ConvertDimension(dims.Height, fromDimUnit);
            }

            //amazon-specific fields
            //item.AmazonOrderItemCode = orderItem.LineItemId;
            //item.ASIN = orderItem.Product.Identifiers?.Asin;

            //item.ConditionNote = orderItem.Product?.Details?.FirstOrDefault((d) => d.Name == "Condition")?.Value;

            AddOrderItemCharges(orderItem, order);
        }

        /// <summary>
        /// Populate image urls
        /// </summary>
        private static void PopulateUrls(OrderSourceSalesOrderItem orderItem, EtsyOrderItemEntity item)
        {
            var urls = orderItem.Product?.Urls;

            item.Thumbnail = urls?.ThumbnailUrl ?? string.Empty;
            item.Image = urls?.ImageUrl ?? string.Empty;
        }

        /// <summary>
        /// Add item charges to the order
        /// </summary>
        private void AddOrderItemCharges(OrderSourceSalesOrderItem orderItem, EtsyOrderEntity order)
        {
            foreach (var orderItemAdjustment in orderItem.Adjustments)
            {
                AddToCharge(order, orderItemAdjustment.Description, orderItemAdjustment.Description, orderItemAdjustment.Amount);
            }

            foreach (var orderItemShippingCharge in orderItem.ShippingCharges)
            {
                AddToCharge(order, "SHIPPING", orderItemShippingCharge.Description.Replace(" price", string.Empty), orderItemShippingCharge.Amount);
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

            var charge = order.OrderCharges.FirstOrDefault(c => string.Compare(c.Type, chargeType.ToUpper(), StringComparison.OrdinalIgnoreCase) == 0);
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
    }
}
