using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz.Util;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.LemonStand.DTO;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Downloader for LemonStand
    /// </summary>
    public class LemonStandDownloader : StoreDownloader
    {
        private const int itemsPerPage = 50;
        private readonly ILemonStandWebClient client;
        private readonly ISqlAdapterRetry sqlAdapter;

        LemonStandStatusCodeProvider statusProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LemonStandDownloader" /> class.
        /// </summary>
        /// <param name="store">The store entity</param>
        public LemonStandDownloader(StoreEntity store)
            : this(
                store, new LemonStandWebClient((LemonStandStoreEntity) store),
                new SqlAdapterRetry<SqlException>(5, -5, "LemonStandStoreDownloader.LoadOrder"))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LemonStandDownloader" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        /// <param name="storeType">The storetype, used for tests</param>
        public LemonStandDownloader(StoreEntity store, ILemonStandWebClient webClient, ISqlAdapterRetry sqlAdapter, StoreType storeType)
            : base(store, storeType)
        {
            client = webClient;
            this.sqlAdapter = sqlAdapter;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LemonStandDownloader" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public LemonStandDownloader(StoreEntity store, ILemonStandWebClient webClient, ISqlAdapterRetry sqlAdapter)
            : base(store, (LemonStandStoreType) StoreTypeManager.GetType(store))
        {
            client = webClient;
            this.sqlAdapter = sqlAdapter;
        }

        /// <summary>
        ///     Download orders from LemonStand
        /// </summary>
        /// <exception cref="DownloadException">
        /// </exception>
        protected override void Download()
        {
            UpdateOrderStatuses();

            Progress.Detail = "Downloading new orders...";

            try
            {
                List<JToken> jsonOrders = new List<JToken>();

                bool allOrdersRetrieved = false;
                int currentPage = 1;

                DateTime startDateTime = GetDownloadStartingPoint();
                string start = ToLemonStandDate(startDateTime);

                // LemonStand does not return any information about number of pages, so we set the limit to 50 orders per page
                // We get 50 orders at a time and if there are in fact 50 items, then get the next page
                while (!allOrdersRetrieved)
                {
                    // Check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    // Get orders from LemonStand
                    JToken result = client.GetOrders(currentPage, start);

                    // Get JSON result objects into a list
                    IList<JToken> orders = result["data"].Children().ToList();
                    jsonOrders.AddRange(orders);

                    if (orders.Count < itemsPerPage)
                    {
                        allOrdersRetrieved = true;
                    }

                    currentPage++;
                }

                int expectedCount = jsonOrders.Count;

                if (ProcessOrders(jsonOrders, expectedCount))
                {
                    return;
                }

                Progress.Detail = "Done";
                Progress.PercentComplete = 100;
            }
            catch (LemonStandException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        private bool ProcessOrders(List<JToken> jsonOrders, int expectedCount)
        {
            // Load orders
            foreach (JToken jsonOrder in jsonOrders)
            {
                // check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return true;
                }

                // Set the progress detail
                Progress.Detail = "Processing order " + (QuantitySaved + 1) + " of " + expectedCount + "...";
                Progress.PercentComplete = Math.Min(100, 100*QuantitySaved/expectedCount);

                LoadOrder(jsonOrder);
            }
            return false;
        }

        /// <summary>
        ///     Load Order from JToken
        /// </summary>
        public void LoadOrder(JToken jsonOrder)
        {
            LemonStandOrderEntity order = PrepareOrder(jsonOrder);

            sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        ///     Prepares the order for loading.
        /// </summary>
        /// <param name="jsonOrder">The json order.</param>
        /// <returns>Order Entity to be saved to database</returns>
        /// <exception cref="LemonStandException"></exception>
        public LemonStandOrderEntity PrepareOrder(JToken jsonOrder)
        {
            //                              order
            //                          /     |      \
            //                    invoices  customer  items
            //                      /         |          \
            //                shipment  billing_address  product
            //                  /
            //          shipment_address

            try
            {
                MethodConditions.EnsureArgumentIsNotNull(jsonOrder, nameof(jsonOrder));

                //Deserialize Json Order into order DTO
                LemonStandOrder lsOrder = JsonConvert.DeserializeObject<LemonStandOrder>(jsonOrder.ToString());

                int orderID = int.Parse(lsOrder.ID);

                LemonStandOrderEntity order =
                    (LemonStandOrderEntity) InstantiateOrder(new LemonStandOrderIdentifier(orderID.ToString()));
                order.LemonStandOrderID = lsOrder.ID;
                order.OnlineStatus = lsOrder.Status;
                order.OnlineStatusCode = lsOrder.ShopOrderStatusID;

                // Only load new orders
                if (order.IsNew)
                {
                    order.OrderDate = GetDate(lsOrder.CreatedAt);
                    order.OnlineLastModified = GetDate(lsOrder.UpdatedAt);
                    order.OrderNumber = int.Parse(lsOrder.Number);

                    // Need invoice id to get shipment information
                    LemonStandInvoice invoice =
                        JsonConvert.DeserializeObject<LemonStandInvoice>(
                            jsonOrder.SelectToken("invoices.data").Children().First().ToString());

                    LoadOrderCharges(order, lsOrder);

                    // Get shipment information and set requested shipping
                    JToken jsonShipment = client.GetShipment(invoice.ID);
                    LemonStandShipment shipment =
                        JsonConvert.DeserializeObject<LemonStandShipment>(
                            jsonShipment.SelectToken("data.shipments.data").Children().First().ToString());

                    order.RequestedShipping = shipment.ShippingService;

                    // Get shipping address from shipment
                    JToken jsonShippingAddress = client.GetShippingAddress(shipment.ID);
                    LemonStandShippingAddress shippingAddress =
                        JsonConvert.DeserializeObject<LemonStandShippingAddress>(
                            jsonShippingAddress.SelectToken("data.shipping_address.data").ToString());

                    // Get customer information and billing address
                    LemonStandCustomer customer =
                        JsonConvert.DeserializeObject<LemonStandCustomer>(
                            jsonOrder.SelectToken("customer.data").ToString());
                    JToken jsonBillingAddress = client.GetBillingAddress(customer.ID);
                    LemonStandBillingAddress billingAddress =
                        JsonConvert.DeserializeObject<LemonStandBillingAddress>(
                            jsonBillingAddress.SelectToken("data.billing_addresses.data").Children().First().ToString());

                    string email = customer.Email;

                    // Load shipping and billing address
                    LoadAddressInfo(order, shippingAddress, billingAddress, email);

                    // Load order items
                    LoadItems(jsonOrder, order);

                    order.OrderTotal = decimal.Parse(lsOrder.Total);
                }
                return order;
            }
            catch (Exception e)
            {
                throw new LemonStandException(e.Message);
            }
        }

        /// <summary>
        ///     Converts LemonStands date format to UTC
        /// </summary>
        /// <param name="date">The date from LemonStand</param>
        /// <returns>DateTime in UTC</returns>
        public static DateTime GetDate(string date)
        {
            DateTime result = DateTime.MinValue;

            DateTime.TryParse(date, out result);

            if (result == DateTime.MinValue)
            {
                throw new LemonStandException("Error loading the date");
            }

            return result.ToUniversalTime();
        }

        /// <summary>
        ///     Converts database date format to the LemonStand date format.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <returns></returns>
        public static string ToLemonStandDate(DateTime utcDateTime)
        {
            // LemonStand date format - 2014-06-02T12:08:24-0700
            DateTimeOffset time = utcDateTime;
            time = time.ToOffset(TimeSpan.FromHours(-7));

            return time.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        /// <summary>
        ///     Loads an order charge.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="name">The charge name.</param>
        /// <param name="amount">The charge amount.</param>
        private void LoadOrderCharge(OrderEntity order, string name, decimal amount)
        {
            //InstantiateOrderCharge
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = name.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        ///     Loads the order charges.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="lsOrder">The LemonStand order DTO</param>
        private void LoadOrderCharges(OrderEntity order, LemonStandOrder lsOrder)
        {
            if (lsOrder.TotalDiscount.IsNullOrWhiteSpace())
            {
                lsOrder.TotalDiscount = "0";
            }

            if (lsOrder.TotalShippingPaid.IsNullOrWhiteSpace())
            {
                lsOrder.TotalShippingPaid = "0";
            }

            if (lsOrder.TotalSalesTaxPaid.IsNullOrWhiteSpace())
            {
                lsOrder.TotalSalesTaxPaid = "0";
            }

            if (lsOrder.TotalShippingTaxPaid.IsNullOrWhiteSpace())
            {
                lsOrder.TotalShippingTaxPaid = "0";
            }

            // LemonStand gives discount as a positive value, we want to subtract it from the order total
            // in the order charge calculations, so make it negative here.
            LoadOrderCharge(order, "Discount", -decimal.Parse((lsOrder.TotalDiscount)));
            LoadOrderCharge(order, "Shipping", decimal.Parse(lsOrder.TotalShippingPaid));
            LoadOrderCharge(order, "Sales Tax", decimal.Parse(lsOrder.TotalSalesTaxPaid));
            LoadOrderCharge(order, "Shipping Tax", decimal.Parse(lsOrder.TotalShippingTaxPaid));
        }

        /// <summary>
        ///     Gets the download starting point.
        /// </summary>
        /// <returns>A DateTime object.</returns>
        private DateTime GetDownloadStartingPoint()
        {
            // We're going to have our starting point default to either the initial download days setting or 30 days back
            int previousDaysToDownload = Store.InitialDownloadDays ?? 30;
            DateTime startingPoint = DateTime.UtcNow.AddDays(-1*previousDaysToDownload);

            DateTime? lastModifiedDate = GetOnlineLastModifiedStartingPoint();
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
        ///     Loads Shipping and Billing address into the order entity
        /// </summary>
        /// <param name="order">The LemonStand order entity</param>
        /// <param name="shipAddress">The shippping addres DTO</param>
        /// <param name="billAddress">The billing address DTO</param>
        /// <param name="email">The customers email address</param>
        private static void LoadAddressInfo(LemonStandOrderEntity order, LemonStandShippingAddress shipAddress,
            LemonStandBillingAddress billAddress, string email)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            shipAdapter.Email = email;
            shipAdapter.FirstName = shipAddress.FirstName;
            shipAdapter.LastName = shipAddress.LastName;
            shipAdapter.Street1 = shipAddress.StreetAddress;
            shipAdapter.Street2 = shipAddress.StreetAddress2;
            shipAdapter.City = shipAddress.City;
            shipAdapter.StateProvCode = Geography.GetStateProvCode(shipAddress.State);
            shipAdapter.PostalCode = shipAddress.PostalCode;
            shipAdapter.CountryCode = Geography.GetCountryCode(shipAddress.Country);
            shipAdapter.Phone = shipAddress.Phone;

            billAdapter.Email = email;
            billAdapter.FirstName = billAddress.FirstName;
            billAdapter.LastName = billAddress.LastName;
            billAdapter.Street1 = billAddress.StreetAddress;
            billAdapter.Street2 = billAddress.StreetAddress2;
            billAdapter.City = billAddress.City;
            billAdapter.StateProvCode = Geography.GetStateProvCode(billAddress.State);
            billAdapter.PostalCode = billAddress.PostalCode;
            billAdapter.CountryCode = Geography.GetCountryCode(billAddress.Country);
            billAdapter.Phone = billAddress.Phone;
        }

        /// <summary>
        ///     Loads the order items.
        /// </summary>
        /// <param name="jsonOrder">The json order</param>
        /// <param name="order">The LemonStand order entity</param>
        private void LoadItems(JToken jsonOrder, LemonStandOrderEntity order)
        {
            LemonStandStoreEntity lsStore = (LemonStandStoreEntity) Store;
            LruCache<int, LemonStandItem> storeProductCache =
                LemonStandProductCache.GetStoreProductImageCache(lsStore.StoreURL, lsStore.Token);

            //List of order items
            IList<JToken> jsonItems = jsonOrder.SelectToken("items.data").Children().ToList();

            // For each item, use the product ID to see if the item has been saved to the cache
            // If so, load from the cache. If it is not in the cache, make a request to get the item information from LemonStand
            foreach (JToken jsonItem in jsonItems)
            {
                string productID = jsonItem.SelectToken("shop_product_id").ToString();

                // Check to see if the item has been cached
                LemonStandItem product = storeProductCache[int.Parse(productID)];

                if (product == null)
                {
                    // Item is not in the cache, so get the information from LemonStand
                    product = GetProductFromLemonStand(productID);

                    // Since it was not in the cache, let's add it
                    storeProductCache[int.Parse(productID)] = product;
                }

                product.Quantity = jsonItem.SelectToken("quantity").ToString();
                product.BasePrice = jsonItem.SelectToken("price").ToString();
                LoadItem(order, product);
            }
        }

        /// <summary>
        ///     Load an order item
        /// </summary>
        /// <param name="order">The LemonStand order entity</param>
        /// <param name="product">The LemonStand item DTO</param>
        private void LoadItem(LemonStandOrderEntity order, LemonStandItem product)
        {
            LemonStandOrderItemEntity item = (LemonStandOrderItemEntity) InstantiateOrderItem(order);

            item.Description = product.Description;
            item.SKU = product.Sku;
            item.Code = product.ID;
            item.Name = product.Name;
            item.Weight = (string.IsNullOrWhiteSpace(product.Weight)) ? 0 : Convert.ToDouble(product.Weight);
            item.UnitPrice = Convert.ToDecimal(product.BasePrice);
            item.Quantity = int.Parse(product.Quantity);
            item.Thumbnail = product.Thumbnail;

            // LemonStand specific item properties
            item.UrlName = product.UrlName;
            item.UnitCost = (string.IsNullOrWhiteSpace(product.Cost)) ? 0 : Convert.ToDecimal(product.Cost);
            item.ShortDescription = product.ShortDescription;
            item.Category = product.Category;

            // Load item attributes
            foreach (JToken jsonAttribute in product.Attributes)
            {
                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                attribute.Name = jsonAttribute.SelectToken("name").ToString();
                attribute.Description = jsonAttribute.SelectToken("value").ToString();
                attribute.UnitPrice = 0;
            }
        }

        /// <summary>
        ///     Gets the product information from LemonStand.
        /// </summary>
        /// <param name="productID">The product identifier.</param>
        /// <returns>A populated LemonStandItem DTO</returns>
        private LemonStandItem GetProductFromLemonStand(string productID)
        {
            JToken jsonProduct = client.GetProduct(productID);

            //Deserialize into LemonStand item
            LemonStandItem product =
                JsonConvert.DeserializeObject<LemonStandItem>(jsonProduct.SelectToken("data").ToString());

            product.Attributes = jsonProduct.SelectToken("data.attributes.data").Children().ToList();
            product.Category =
                jsonProduct.SelectToken("data.categories.data").Children().First().SelectToken("name").ToString();
            // Get the product images from LemonStand
            List<JToken> productImagesJson = jsonProduct.SelectToken("data.images.data")
                .Children()
                .First()
                .SelectToken("thumbnails")
                .Children()
                .ToList();

            // If we received a productImages response, process it
            if (productImagesJson != null)
            {
                JToken image = productImagesJson.FirstOrDefault();
                if (image != null)
                {
                    product.Thumbnail = "http:" + image.SelectToken("location");
                }
            }

            return product;
        }

        /// <summary>
        /// Update the local order status provider
        /// </summary>
        private void UpdateOrderStatuses()
        {
            Progress.Detail = "Updating status codes...";

            // refresh the status codes from LemonStand
            statusProvider = new LemonStandStatusCodeProvider((LemonStandStoreEntity)Store);
            statusProvider.UpdateFromOnlineStore();
        }
    }
}