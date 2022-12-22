using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

#nullable enable
namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public abstract class PlatformDownloader : StoreDownloader
    {
        protected readonly ILog log;

        /// <summary>
        /// Object factory for the platform web client
        /// </summary>
        private readonly Func<StoreEntity, IPlatformOrderWebClient> createWebClient;
        /// <summary>
        /// Store manager used to save the continuation token to the platform store
        /// </summary>
        protected readonly IStoreManager storeManager;


        protected PlatformDownloader(StoreEntity store, StoreType storeType, IStoreManager storeManager, Func<StoreEntity, IPlatformOrderWebClient> createWebClient) : base(store, storeType)
        {
            log = LogManager.GetLogger(this.GetType());
            this.storeManager = storeManager;
            this.createWebClient = createWebClient;
        }
        protected List<GiftNote> GetGiftNotes(OrderSourceApiSalesOrder salesOrder)
        {
            var itemNotes = new List<GiftNote>();
            if (salesOrder.Notes != null)
            {
                foreach (var note in salesOrder.Notes)
                {
                    if (note.Type == OrderSourceNoteType.GiftMessage)
                    {
                        itemNotes.Add(GiftNote.FromOrderSourceNote(note));
                    }
                }
            }

            return itemNotes;
        }

        protected static IEnumerable<CouponCode> GetCouponCodes(OrderSourceApiSalesOrder salesOrder)
        {
            return salesOrder.Payment.CouponCodes.Select((c) => JsonConvert.DeserializeObject<CouponCode>(c));
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        protected void LoadOrderItem(OrderSourceSalesOrderItem orderItem, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            var item = (AmazonOrderItemEntity) InstantiateOrderItem(order);

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

            // amazon-specific fields
            item.AmazonOrderItemCode = orderItem.LineItemId;
            item.ASIN = orderItem.Product.Identifiers?.Asin;

            //Load the gift messages
            foreach(var giftNote in giftNotes)
            {
                if(giftNote.Message.HasValue() || giftNote.Fee > 0)
                {
                    OrderItemAttributeEntity giftAttribute = InstantiateOrderItemAttribute(item);
                    giftAttribute.Name = "Gift Message";
                    giftAttribute.Description = giftNote.Message;
                    giftAttribute.UnitPrice = giftNote.Fee;
                    item.OrderItemAttributes.Add(giftAttribute);
                }

                if (giftNote.GiftWrapLevel.HasValue())
                {
                    OrderItemAttributeEntity levelAttribute = InstantiateOrderItemAttribute(item);
                    levelAttribute.Name = "Gift Wrap Level";
                    levelAttribute.Description = giftNote.GiftWrapLevel;
                    levelAttribute.UnitPrice = 0;
                    item.OrderItemAttributes.Add(levelAttribute);
                }
            }

            //Load any coupon codes
            if (couponCodes != null)
            {
                foreach (var couponSet in couponCodes)
                {
                    foreach (var code in couponSet.Codes)
                    {
                        OrderItemAttributeEntity couponAttribute = InstantiateOrderItemAttribute(item);
                        couponAttribute.Name = "Promotion ID";
                        couponAttribute.Description = code;
                        couponAttribute.UnitPrice = 0;
                        item.OrderItemAttributes.Add(couponAttribute);
                    }
                }
            }

            item.ConditionNote = orderItem.Product?.Details?.FirstOrDefault((d) => d.Name == "Condition")?.Value;

            AddOrderItemCharges(orderItem, order);
        }

        /// <summary>
        /// Populate image urls
        /// </summary>
        private static void PopulateUrls(OrderSourceSalesOrderItem orderItem, AmazonOrderItemEntity item)
        {
            var urls = orderItem.Product?.Urls;

            item.Thumbnail = urls?.ThumbnailUrl ?? string.Empty;
            item.Image = urls?.ImageUrl ?? string.Empty;
        }

        /// <summary>
        /// Add item charges to the order
        /// </summary>
        private void AddOrderItemCharges(OrderSourceSalesOrderItem orderItem, OrderEntity order)
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
        protected void AddToCharge(OrderEntity order, string chargeType, string name, decimal amount)
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

        protected static void LoadAddresses(OrderEntity order, OrderSourceApiSalesOrder salesOrder)
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

        /// <summary>
        /// Loads the order items of an amazon order
        /// </summary>
        protected void LoadOrderItems(OrderSourceRequestedFulfillment fulfillment, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            foreach (var item in fulfillment.Items)
            {
                var filteredNotes = giftNotes.Where(i => i.OrderItemId == item.LineItemId);
                var filteredCouponCodes = couponCodes.Where((c) => c.OrderItemId == item.LineItemId);
                LoadOrderItem(item, order, filteredNotes, filteredCouponCodes);
            }
        }

        protected virtual string GetOrderStatusString(OrderSourceApiSalesOrder orderSourceApiSalesOrder, string orderId)
        {
            switch (orderSourceApiSalesOrder.Status)
            {
                case OrderSourceSalesOrderStatus.AwaitingPayment:
                    return "Awaiting Payment";
                case OrderSourceSalesOrderStatus.AwaitingShipment:
                    return "Awaiting Shipment";
                case OrderSourceSalesOrderStatus.PendingFulfillment:
                    return "Pending Fulfillment";
                case OrderSourceSalesOrderStatus.OnHold:
                    return "On Hold";
                case OrderSourceSalesOrderStatus.Cancelled:
                case OrderSourceSalesOrderStatus.Completed:
                default:
                    return orderSourceApiSalesOrder.Status.ToString();
            }
        }

        /// <summary>
        /// GetRequestedShipping (in the format we used to get it from MWS "carrier: details")
        /// </summary
        protected string GetRequestedShipping(string? shippingService)
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
        /// Start the download from Platform for the Etsy store
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Connecting to Platform...";

                var client = createWebClient(Store);

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

                    foreach (var salesOrder in result.Orders.Data.Where(x => x.Status != OrderSourceSalesOrderStatus.AwaitingPayment))
                    {
                        await LoadOrder(salesOrder).ConfigureAwait(false); 
                    }

                    // Save the continuation token to the store
                    Store.ContinuationToken = result.Orders.ContinuationToken;
                    await storeManager.SaveStoreAsync(Store).ConfigureAwait(false);

                    result = await client.GetOrders(Store.OrderSourceID, Store.ContinuationToken, Progress.CancellationToken).ConfigureAwait(false);

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
        /// Create the order instance
        /// </summary>
        protected abstract Task<OrderEntity?> CreateOrder(OrderSourceApiSalesOrder salesOrder);
        
        /// <summary>
        /// Store order in database
        /// </summary>
        private async Task LoadOrder(OrderSourceApiSalesOrder salesOrder)
        {
            var order = await CreateOrder(salesOrder);
            if (order == null)
            {
                return;
            }

            if (salesOrder.Status == OrderSourceSalesOrderStatus.Cancelled && order.IsNew)
            {
                log.InfoFormat("Skipping order '{0}' due to canceled and not yet seen by ShipWorks.", salesOrder.OrderNumber);
                return;
            }

            order.ChannelOrderID = salesOrder.SalesOrderGuid;

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

                AddTaxes(salesOrder, order);

                // update the total
                var calculatedTotal = OrderUtility.CalculateTotal(order);

                // get the amount so we can fudge order totals
                order.OrderTotal = salesOrder.Payment.AmountPaid ?? calculatedTotal;
            }

            // save
            var retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "PlatformDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        protected virtual void AddTaxes(OrderSourceApiSalesOrder salesOrder, OrderEntity order)
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
    }
}
