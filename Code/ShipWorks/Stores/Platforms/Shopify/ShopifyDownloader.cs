using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Downloader for ShopifyStores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Shopify)]
    public class ShopifyDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyDownloader));
        private readonly ShopifyRequestedShippingField requestedShippingField = ShopifyRequestedShippingField.Code;
        private readonly IShopifyWebClient webClient = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyDownloader(StoreEntity store,
            Func<ShopifyStoreEntity, IProgressReporter, IShopifyWebClient> webClientFactory)
            : base(store)
        {
            requestedShippingField = (ShopifyRequestedShippingField) ((ShopifyStoreEntity) store).ShopifyRequestedShippingOption;
            OrdersPerPage = ShopifyConstants.ShopifyOrdersPerPage;
            webClient = webClientFactory(store as ShopifyStoreEntity, Progress);
        }


        public int OrdersPerPage { get; set; }

        /// <summary>
        /// Download data for the Shopify store
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Downloading orders...";

            using (new LoggedStopwatch(log, "ShopifyDownloader.Download()"))
            {
                try
                {
                    await PerformDownload().ConfigureAwait(false);
                }
                catch (ShopifyWebClientThrottleWaitCancelException)
                {
                    // Just a cancel - nothing to do
                }
                catch (ShopifyException ex)
                {
                    log.Error(ex);
                    throw new DownloadException(ex.Message, ex);
                }
                catch (SqlForeignKeyException ex)
                {
                    log.Error(ex);
                    throw new DownloadException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Perform the actual download
        /// </summary>
        private async Task PerformDownload()
        {
            Range<DateTime> dateRange = await GetNextDownloadDateRange().ConfigureAwait(false);

            // Get count of orders
            int totalCount = webClient.GetOrderCount(dateRange.Start, dateRange.End);
            if (totalCount == 0)
            {
                Progress.Detail = "No orders to download.";
                Progress.PercentComplete = 100;
                return;
            }

            Tuple<DateTime, int> previousDownloadInfo = new Tuple<DateTime, int>(dateRange.Start, totalCount);

            // Keep going until there are no more to download
            while (true)
            {
                bool shouldContinue = await DownloadOrderRange(dateRange, totalCount).ConfigureAwait(false);

                if (!shouldContinue || Progress.IsCancelRequested)
                {
                    return;
                }

                // Update our date range to see if we can grab any orders that have come in while we've been downloading
                dateRange = await GetNextDownloadDateRange().ConfigureAwait(false);

                // Get how many have come in since we started
                int nextCount = webClient.GetOrderCount(dateRange.Start, dateRange.End);

                // Detect if we are in an infinite loop where we keep asking for the same last modified time
                // and keep getting back the same order count.  Return if this happens.
                if (previousDownloadInfo.Item1 == dateRange.Start && previousDownloadInfo.Item2 == nextCount)
                {
                    return;
                }

                // Update our previous download date and order count.
                previousDownloadInfo = new Tuple<DateTime, int>(dateRange.Start, nextCount);

                if (nextCount > 0)
                {
                    // If there are any, add them to our total and keep going
                    totalCount += nextCount;
                }
                else
                {
                    Progress.Detail = "Done.";
                    return;
                }
            }
        }

        /// <summary>
        /// Get the next download starting and ending date and time
        /// </summary>
        private async Task<Range<DateTime>> GetNextDownloadDateRange()
        {
            // Getting last online modified starting point
            DateTime? startDate = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);

            if (!startDate.HasValue)
            {
                startDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(180));
            }

            // Use the web servers ending time, backed off by a little so we don't have to worry about race conditions
            DateTime endDate = webClient.GetServerCurrentDateTime().Subtract(TimeSpan.FromMinutes(2));

            return startDate.Value.To(endDate);
        }

        /// <summary>
        /// Make the call to Shopify to get a list of orders matching criteria
        /// </summary>
        /// <param name="dateRange">Filter by shopify order modified date between these dates</param>
        /// <param name="totalCount">Total count of orders</param>
        /// <returns>Number of orders matching criteria</returns>
        public async Task<bool> DownloadOrderRange(Range<DateTime> dateRange, int totalCount)
        {
            // Check for cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Update progress reporter that we are now downloading orders
            Progress.Detail = "Downloading orders...";

            try
            {
                // Create the initial date range requested
                ShopifyGetOrdersDateRange orderRange = new ShopifyGetOrdersDateRange(dateRange);

                // Iterate through each date range
                foreach (ShopifyGetOrdersDateRange subRange in orderRange.GenerateOrderRanges(webClient, OrdersPerPage))
                {
                    // Check for cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }

                    List<JToken> orders = new List<JToken>();

                    // Go through each page determined by the date range to be necessary to get all of the orders
                    for (int page = 1; page <= subRange.PageCount; page++)
                    {
                        List<JToken> pageOfOrders = webClient.GetOrders(subRange.StartDate, subRange.EndDate, page);
                        orders.AddRange(pageOfOrders);
                    }

                    // We have to import them in ascending order in case the user cancels, so our LastModified dates stay in order
                    orders = orders.OrderBy(o => o.GetValue<DateTime>("updated_at")).ToList();

                    // Load the orders
                    bool wasSuccessful = await LoadOrders(orders, totalCount).ConfigureAwait(false);
                    if (!wasSuccessful)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (JsonException ex)
            {
                log.Error("An error occurred in GetOrders.", ex);
                throw new ShopifyException("Shopify returned an invalid response while downloading orders.", ex);
            }
        }

        /// <summary>
        /// Load all the orders contained in the iterator
        /// </summary>
        private async Task<bool> LoadOrders(List<JToken> jsonOrders, int totalCount)
        {
            //Iterate through each jsonOrder
            foreach (JToken jsonOrder in jsonOrders)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                // Call LoadOrder to process the jsonOrder
                await LoadOrder(jsonOrder).ConfigureAwait(false);

                // Update the PercentComplete
                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }

            return true;
        }

        /// <summary>
        /// Extract and save the order from the XML
        /// </summary>
        private async Task LoadOrder(JToken jsonOrder)
        {
            MethodConditions.EnsureArgumentIsNotNull(jsonOrder, nameof(jsonOrder));

            try
            {
                //Parse id
                long shopifyOrderId = jsonOrder.GetValue<long>("id");

                //Get the order instance.
                GenericResult<OrderEntity> result = await InstantiateOrder(new ShopifyOrderIdentifier(shopifyOrderId)).ConfigureAwait(false);
                if (result.Failure)
                {
                    log.InfoFormat("Skipping order '{0}': {1}.", shopifyOrderId, result.Message);
                    return;
                }

                ShopifyOrderEntity order = (ShopifyOrderEntity) result.Value;

                //Set the total.  It will be calculated and verified later.
                order.OrderTotal = jsonOrder.GetValue("total_price", 0.0m);

                // Get order date
                order.OrderDate = jsonOrder.GetValue("created_at", DateTime.MinValue).ToUniversalTime();

                //Get the customer
                long onlineCustomerID = jsonOrder.GetValue<long>("customer.id", -1);
                order.OnlineCustomerID = (onlineCustomerID == -1) ? (long?) null : onlineCustomerID;

                //Requested shipping
                if (requestedShippingField == ShopifyRequestedShippingField.Code)
                {
                    order.RequestedShipping = jsonOrder.GetValue("shipping_lines[0].code", string.Empty);
                }
                else
                {
                    order.RequestedShipping = jsonOrder.GetValue("shipping_lines[0].title", string.Empty);
                }

                //Set OnlineLastModified to their last modified date
                order.OnlineLastModified = jsonOrder.GetValue("updated_at", order.OnlineLastModified).ToUniversalTime();

                //Load Online Status
                LoadStatus(order, jsonOrder);

                //Load address info
                LoadAddressInfo(order, jsonOrder);

                // Load any order notes
                await LoadOrderNote(order, jsonOrder).ConfigureAwait(false);

                // Only update the rest for brand new orders
                if (order.IsNew)
                {
                    LoadOrderDetailsWhenNew(jsonOrder, order);
                }

                // Save the downloaded order
                SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ShopifyDownloader.LoadOrder");
                await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
            }
            catch (JsonException jsonEx)
            {
                log.Error(jsonEx);
                throw new DownloadException("Shopify returned an invalid response while downloading orders.", jsonEx);
            }
        }

        /// <summary>
        /// Load order details when the order is new
        /// </summary>
        private void LoadOrderDetailsWhenNew(JToken jsonOrder, ShopifyOrderEntity order)
        {
            LoadOrderNumber(order, jsonOrder);

            // Iterate through each jsonOrder to get line items
            foreach (JToken lineItem in jsonOrder.SelectToken("line_items"))
            {
                LoadItem(order, lineItem);
            }

            // Load all the charges
            LoadOrderCharges(order, jsonOrder);

            // Load all payment details
            LoadPaymentDetails(order, jsonOrder);
        }

        /// <summary>
        /// Load the order number for the Shopify order, taking into consideration the prefix\postfix Shopify allows
        /// </summary>
        private void LoadOrderNumber(ShopifyOrderEntity order, JToken jsonOrder)
        {
            order.OrderNumber = jsonOrder.GetValue<int>("order_number");

            // Shopify allows adding prefix\suffix to the order number.  The "full" order number is stored in the 'name' node.  We'll use that to determine what
            // the prefix\suffix is
            string fullOrderNumber = jsonOrder.GetValue<string>("name");

            int numericIndex = fullOrderNumber.IndexOf(order.OrderNumber.ToString(), StringComparison.Ordinal);

            // Shouldn't happen, but bail if it does
            if (numericIndex < 0)
            {
                return;
            }

            // Extract the prefix
            string prefix = fullOrderNumber.Substring(0, numericIndex);

            // Extract the postfix
            string postfix = fullOrderNumber.Substring(numericIndex + order.OrderNumber.ToString().Length);

            // Shopify defaults to # as a prefix... let's not consider that
            if (prefix == "#")
            {
                prefix = "";
            }

            order.ApplyOrderNumberPrefix(prefix);
            order.ApplyOrderNumberPostfix(postfix);
        }

        /// <summary>
        /// Extract and set the status, financial, and fulfillment statuses from the jsonOrder, and convert to ShipWorks versions.
        /// </summary>
        private void LoadStatus(ShopifyOrderEntity order, JToken jsonOrder)
        {
            //Get financial_status
            string jsonOnlineFinancialStatus = jsonOrder.GetValue("financial_status", string.Empty).ToLowerInvariant();

            //Get fulfillment_status
            string jsonOnlineFulfillmentStatus = jsonOrder.GetValue("fulfillment_status", string.Empty).ToLowerInvariant();

            // Get the financial status to display
            //Add Financial status
            ShopifyPaymentStatus onlineFinancialStatus = ShopifyPaymentStatus.Unknown;
            try
            {
                onlineFinancialStatus = EnumHelper.GetEnumByApiValue<ShopifyPaymentStatus>(jsonOnlineFinancialStatus);
            }
            catch (InvalidOperationException ex)
            {
                // Sometimes Shopify adds new statuses which keeps users from downloading.  So, we'll leave assigned to unknown and carry on.
                // Log the msg so we know what status was missing, if the customer contacts us.
                log.Debug(ex);
            }

            order.PaymentStatusCode = (int) onlineFinancialStatus;

            // If the order hasn't been fulfilled, no need to display that info
            //Add Fulfillment status
            if (string.IsNullOrWhiteSpace(jsonOnlineFulfillmentStatus))
            {
                order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Unshipped;
            }
            else
            {
                try
                {
                    order.FulfillmentStatusCode = (int) EnumHelper.GetEnumByApiValue<ShopifyFulfillmentStatus>(jsonOnlineFulfillmentStatus);
                }
                catch (InvalidOperationException ex)
                {
                    order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Unknown;
                    // Sometimes Shopify adds new statuses which keeps users from downloading.  So, we'll leave assigned to unknown and carry on.
                    // Log the msg so we know what status was missing, if the customer contacts us.
                    log.Debug(ex);
                }
            }

            // These aren't always returned in the json...so if they fail, just continue
            //Check for cancelled_at and closed_at
            DateTime canceledAt = jsonOrder.GetValue<DateTime>("cancelled_at", DateTime.MinValue);
            DateTime closedAt = jsonOrder.GetValue<DateTime>("closed_at", DateTime.MinValue);

            if (canceledAt != DateTime.MinValue)
            {
                string jsonOnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Canceled);

                order.OnlineStatus = jsonOnlineStatus;
            }
            else if (closedAt != DateTime.MinValue)
            {
                string jsonOnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Closed);

                order.OnlineStatus = jsonOnlineStatus;
            }
            else
            {
                // Check to see if the order is 'completed' online, and mark it shipped locally if so.
                if (order.FulfillmentStatusCode == (int) ShopifyFulfillmentStatus.Fulfilled &&
                    onlineFinancialStatus == ShopifyPaymentStatus.Paid)
                {
                    order.OnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Shipped);
                }
                else
                {
                    order.OnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Open);
                }
            }
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(ShopifyOrderEntity order, JToken lineItem)
        {
            ShopifyOrderItemEntity item = (ShopifyOrderItemEntity) InstantiateOrderItem(order);

            // Set item properties
            item.Name = lineItem.GetValue<string>("title", string.Empty);
            item.Code = lineItem.GetValue<string>("sku", string.Empty);
            item.SKU = item.Code;
            item.Quantity = lineItem.GetValue<int>("quantity", 0);
            item.UnitPrice = lineItem.GetValue<decimal>("price", 0.0m);
            item.Weight = GetLineItemWeight(lineItem);

            decimal lineDiscount = lineItem.GetValue("total_discount", 0.0m);
            if (lineDiscount != 0)
            {
                LoadCharge(order, "Discount", $"SKU: {item.Code}", -lineDiscount);
            }

            // Load the item option - which to Shopify is a "variant"
            LoadOption(item, lineItem);

            // Try to get the shopify product via api so we can get image urls
            LoadProducts(item, lineItem);
        }

        /// <summary>
        /// Load the option of the given item
        /// </summary>
        private void LoadOption(OrderItemEntity item, JToken lineItem)
        {
            string name = lineItem.GetValue<string>("variant_title", string.Empty);
            if (string.IsNullOrWhiteSpace(name) && (lineItem.SelectToken("properties") == null || !lineItem.SelectToken("properties").Any()))
            {
                return;
            }

            //Instantiate the order item attribute
            OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

            //Set the option properties
            option.Name = "Variant";
            option.Description = name;

            // Shopify only sends the total line price
            option.UnitPrice = 0;

            // Add any properties for this option
            LoadOptionProperties(item, lineItem);
        }

        /// <summary>
        /// Add any Shopify option properties to the order item attribute
        /// </summary>
        private void LoadOptionProperties(OrderItemEntity item, JToken lineItem)
        {
            JToken properties = lineItem.SelectToken("properties");
            if (properties == null)
            {
                return;
            }

            foreach (JToken property in properties.Select(prop => prop))
            {
                string propertyName = property.GetValue<string>("name", string.Empty);
                string propertyValue = property.GetValue<string>("value", string.Empty);

                //Instantiate the order item attribute
                OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);
                option.Name = string.Format("   {0}", propertyName);
                option.Description = propertyValue;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Load the option of the given item
        /// </summary>
        private void LoadProducts(ShopifyOrderItemEntity item, JToken lineItem)
        {
            long productId = lineItem.GetValue<long>("product_id");
            item.ShopifyProductID = productId;

            JToken shopifyProduct = webClient.GetProduct(productId);

            // Product may not exist
            if (shopifyProduct == null)
            {
                return;
            }

            GetImage(item, shopifyProduct);
            GetBarcode(item, lineItem, shopifyProduct);
        }

        /// <summary>
        /// Gets the image from the shopifyProduct and copies them into the ShopifyOrderItemEntity
        /// </summary>
        private static void GetImage(ShopifyOrderItemEntity item, JToken shopifyProduct)
        {
            JToken images = shopifyProduct.SelectToken("product.images");

            if (images != null)
            {
                string imageUrl = string.Empty;
                foreach (JToken prodImg in shopifyProduct.SelectToken("product.images").Select(img => img))
                {
                    imageUrl = prodImg.GetValue("src", string.Empty);
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        break;
                    }
                }

                item.Image = imageUrl;
                item.Thumbnail = imageUrl;
            }
        }

        /// <summary>
        /// Retrieves the corresponding variant barcode from the Shopify product and stores it as the UPC of the ShopifyOrderItemEntity
        /// </summary>
        private void GetBarcode(ShopifyOrderItemEntity item, JToken lineItem, JToken shopifyProduct)
        {
            // The barcode is stored as part of the product variant, so we need the variantID.
            long variantID = lineItem.GetValue<long>("variant_id");

            // Grab all the variants of the product
            JToken variants = shopifyProduct.SelectToken("product.variants");

            // Get the variant with a corresponding variantID
            JToken variant = variants.FirstOrDefault(v => v.GetValue<long>("id") == variantID);
            if (variant != null)
            {
                item.UPC = variant.GetValue<string>("barcode");
            }
        }

        /// <summary>
        /// Extract the shopify line item weight in grams and return the value converted to pounds
        /// </summary>
        private static double GetLineItemWeight(JToken jsonLineItem)
        {
            double unitWeight = 0d;
            double gramsInAPound = 453.59237;

            //Get the unit number of grams
            unitWeight = jsonLineItem.GetValue<double>("grams", 0.0d);

            //Convert to pounds
            unitWeight = unitWeight / gramsInAPound;

            return unitWeight;
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, JToken jsonOrder)
        {
            // Charge - Discount
            //Iterate through each discount_codes
            foreach (JToken discount in jsonOrder.SelectToken("discount_codes").Select(d => d))
            {
                //Load charge for each discount code
                LoadCharge(order, "Discount", discount.GetValue("code", "Discount"), -discount.GetValue<decimal>("amount", 0m));
            }

            //Iterate through each tax_lines
            foreach (JToken tax in jsonOrder.SelectToken("tax_lines").Select(t => t))
            {
                // Get the tax name
                string name = tax.GetValue("title", "Tax");

                // LoadCharge for tax line
                LoadCharge(order, "Tax", name, tax.GetValue<decimal>("price", 0m));
            }

            //Find the first shippling_lines
            JToken shipping = jsonOrder.SelectToken("shipping_lines").Select(s => s).FirstOrDefault();
            if (shipping != null)
            {
                //LoadCharge for shipping line
                LoadCharge(order, "Shipping", shipping.GetValue("title", "Shipping"), shipping.GetValue<decimal>("price", 0m));
            }
        }

        /// <summary>
        /// Load an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            if (string.IsNullOrWhiteSpace(name))
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Load the payment details for the order
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, JToken jsonOrder)
        {
            // Load each payment detail
            JToken paymentDetails = jsonOrder.SelectToken("payment_details");
            if (paymentDetails != null)
            {
                foreach (var item in paymentDetails)
                {
                    string uglyLabel = ((JProperty) item).Name;
                    string value = paymentDetails.GetValue<string>(uglyLabel, "");

                    string cleansedLabel = AddressCasing.Apply(uglyLabel.Replace("_", " "));

                    LoadPaymentDetail(order, cleansedLabel, value);
                }
            }

            // Extract the name of the gateway
            string gateway = jsonOrder.GetValue<string>("gateway");
            if (!string.IsNullOrEmpty(gateway))
            {
                LoadPaymentDetail(order, "Payment Type", AddressCasing.Apply(gateway));
            }
        }

        /// <summary>
        /// Load the given payment detail into the order
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            if (string.IsNullOrEmpty(label) || string.IsNullOrEmpty(value))
            {
                return;
            }

            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);
            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// If a note is on the order, save it
        /// </summary>
        private async Task LoadOrderNote(OrderEntity order, JToken jsonOrder)
        {
            string orderNote = jsonOrder.GetValue<string>("note", string.Empty);

            if (!string.IsNullOrWhiteSpace(orderNote))
            {
                await InstantiateNote(order, orderNote, order.OrderDate, NoteVisibility.Public, true).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, JToken jsonOrder)
        {
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));
            MethodConditions.EnsureArgumentIsNotNull(jsonOrder, nameof(jsonOrder));

            // Load shipping address info
            LoadAddressInfo(order, jsonOrder, "shipping", "Ship");

            // Load billing address info
            LoadAddressInfo(order, jsonOrder, "billing", "Bill");

            // Shopify doesn't provide a separate email for bill\ship, so set them both here to what we have
            order.BillEmail = jsonOrder.GetValue<string>("email", "");
            order.ShipEmail = order.BillEmail;
        }

        /// <summary>
        /// Load the address info for the given address type prefix
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, JToken jsonOrder, string addressType, string dbPrefix)
        {
            string addressKey = string.Format("{0}_address", addressType);

            //See if the NameParts entries exist
            string first = jsonOrder.GetValue<string>(string.Format("{0}.first_name", addressKey), "");
            string middle = string.Empty;
            string last = jsonOrder.GetValue<string>(string.Format("{0}.last_name", addressKey), "");

            //Parse person's name
            PersonName fullName = PersonName.Parse(string.Format("{0} {1}", first, last));
            PersonAdapter personAdapter = new PersonAdapter(order, dbPrefix);

            personAdapter.UnparsedName = fullName.FullName;
            personAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            personAdapter.FirstName = first;
            personAdapter.MiddleName = middle;
            personAdapter.LastName = last;

            personAdapter.Company = jsonOrder.GetValue<string>(string.Format("{0}.company", addressKey), "");
            personAdapter.Phone = jsonOrder.GetValue<string>(string.Format("{0}.phone", addressKey), "");

            personAdapter.Street1 = jsonOrder.GetValue<string>(string.Format("{0}.address1", addressKey), "");
            personAdapter.Street2 = jsonOrder.GetValue<string>(string.Format("{0}.address2", addressKey), "");

            personAdapter.City = jsonOrder.GetValue<string>(string.Format("{0}.city", addressKey), "");
            personAdapter.StateProvCode = Geography.GetStateProvCode(jsonOrder.GetValue<string>(string.Format("{0}.province_code", addressKey), ""));
            personAdapter.PostalCode = jsonOrder.GetValue<string>(string.Format("{0}.zip", addressKey), "");
            personAdapter.CountryCode = Geography.GetCountryCode(jsonOrder.GetValue<string>(string.Format("{0}.country_code", addressKey), ""));

            personAdapter.Phone = jsonOrder.GetValue<string>(string.Format("{0}.phone", addressKey), "");
        }
    }
}
