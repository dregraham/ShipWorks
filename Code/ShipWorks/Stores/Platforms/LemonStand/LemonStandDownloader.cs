using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.LemonStand.DTO;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Downloader for LemonStand
    /// </summary>
    public class LemonStandDownloader : StoreDownloader
    {
        private readonly ILemonStandWebClient client;
        private readonly ISqlAdapterRetry sqlAdapter;
        private const int itemsPerPage = 250;

        /// <summary>
        /// Initializes a new instance of the <see cref="LemonStandDownloader"/> class.
        /// </summary>
        /// <param name="store"></param>
        public LemonStandDownloader(StoreEntity store)
            : this(
                store, new LemonStandWebClient((LemonStandStoreEntity) store),
                new SqlAdapterRetry<SqlException>(5, -5, "LemonStandStoreDownloader.LoadOrder"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LemonStandDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public LemonStandDownloader(StoreEntity store, ILemonStandWebClient webClient, ISqlAdapterRetry sqlAdapter)
            : base(store)
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
            Progress.Detail = "Downloading new orders...";

            try
            {
                List<JToken> jsonOrders = new List<JToken>();

                bool allOrdersRetrieved = false;
                int currentPage = 1;

                // LemonStand does not return any information about number of pages, but by default returns 250 items per page
                // So we get first 250 and if there are in fact 250 items, then get the next page
                while (!allOrdersRetrieved)
                {
                    // Check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    // Get orders from LemonStand 
                    JToken result = client.GetOrders(currentPage);

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

                // Load orders 
                foreach (JToken jsonOrder in jsonOrders)
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    // Set the progress detail
                    Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved+1, expectedCount);
                    Progress.PercentComplete = Math.Min(100, 100*QuantitySaved/expectedCount);

                    LoadOrder(jsonOrder);
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

        /// <summary>
        ///     Load Order from JToken
        /// </summary>
        public void LoadOrder(JToken jsonOrder)
        {
            LemonStandOrderEntity order = PrepareOrder(jsonOrder);

            sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Prepares the order for loading.
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
                //Deserialize Json Order into order DTO
                LemonStandOrder lsOrder = JsonConvert.DeserializeObject<LemonStandOrder>(jsonOrder.ToString());

                
                int orderID = int.Parse(lsOrder.ID);

                LemonStandOrderEntity order =
                    (LemonStandOrderEntity)InstantiateOrder(new LemonStandOrderIdentifier(orderID.ToString()));

                order.OnlineStatus = lsOrder.Status;

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
                        JsonConvert.DeserializeObject<LemonStandCustomer>(jsonOrder.SelectToken("customer.data").ToString());
                    JToken jsonBillingAddress = client.GetBillingAddress(customer.ID);
                    LemonStandBillingAddress billingAddress =
                        JsonConvert.DeserializeObject<LemonStandBillingAddress>(
                            jsonBillingAddress.SelectToken("data.billing_addresses.data").Children().First().ToString());

                    string email = customer.Email;

                    // Load shipping and billing address
                    LoadAddressInfo(order, shippingAddress, billingAddress, email);

                    // Load order items
                    LoadItems(jsonOrder, order);

                    order.OrderTotal = decimal.Parse(lsOrder.SubtotalPaid);
                    
                }
                return order;
            }
            catch (Exception e)
            {
                throw new LemonStandException(e.Message);
            }
        }

        /// <summary>
        ///     Converts LemonStand date to UTC
        /// </summary>
        /// <param name="date">The date from LemonStand</param>
        /// <returns>DateTime in UTC</returns>
        public DateTime GetDate(string date)
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
        ///     Loads Shipping and Billing address into the order entity
        /// </summary>
        private static void LoadAddressInfo(LemonStandOrderEntity order, LemonStandShippingAddress shipAddress,
            LemonStandBillingAddress billAddress, string email)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            shipAdapter.Email = email;
            shipAdapter.FirstName = shipAddress.FirstName;
            shipAdapter.LastName = shipAddress.LastName;
            shipAdapter.Street1 = shipAddress.StreetAddress;
            shipAdapter.City = shipAddress.City;
            shipAdapter.StateProvCode = Geography.GetStateProvCode(shipAddress.State);
            shipAdapter.PostalCode = shipAddress.PostalCode;
            shipAdapter.CountryCode = Geography.GetCountryCode(shipAddress.Country);
            shipAdapter.Phone = shipAddress.Phone;

            billAdapter.Email = email;
            billAdapter.FirstName = billAddress.FirstName;
            billAdapter.LastName = billAddress.LastName;
            billAdapter.Street1 = billAddress.StreetAddress;
            billAdapter.City = billAddress.City;
            billAdapter.StateProvCode = Geography.GetStateProvCode(billAddress.State);
            billAdapter.PostalCode = billAddress.PostalCode;
            billAdapter.CountryCode = Geography.GetCountryCode(billAddress.Country);
            billAdapter.Phone = billAddress.Phone;
        }

        /// <summary>
        ///     Loads the order items.
        /// </summary>
        /// <param name="jsonOrder">The json order.</param>
        /// <param name="order">The order.</param>
        private void LoadItems(JToken jsonOrder, LemonStandOrderEntity order)
        {
            //List of order items
            IList<JToken> jsonItems = jsonOrder.SelectToken("items.data").Children().ToList();
            foreach (JToken jsonItem in jsonItems)
            {
                string productID = jsonItem.SelectToken("shop_product_id").ToString();

                JToken jsonProduct = client.GetProduct(productID);

                //Deserialize into LemonStand item
                LemonStandItem product =
                    JsonConvert.DeserializeObject<LemonStandItem>(jsonProduct.SelectToken("data").ToString());
                product.Quantity = jsonItem.SelectToken("quantity").ToString();
                LoadItem(order, product);
            }
        }

        /// <summary>
        ///     Load an order item
        /// </summary>
        private void LoadItem(LemonStandOrderEntity order, LemonStandItem product)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            //"data": [{
            //    "id": 1,
            //    "shop_order_id": 1,
            //    "shop_product_id": 1,
            //    "shop_tax_class_id": 1,
            //    "name": "Baseball cap",
            //    "description": null,
            //    "quantity": 1,
            //    "original_price": 39.99,
            //    "price": 39.99,
            //    "base_price": 39.99,
            //    "cost": null,
            //    "discount": 0,
            //    "total": 39.99,
            //    "options": null,
            //    "extras": null
            //}]

            item.SKU = product.Sku;
            item.Code = product.ID;
            item.Name = product.Name;
            item.Weight = (string.IsNullOrWhiteSpace(product.Weight)) ? 0 : Convert.ToDouble(product.Weight);
            item.UnitPrice = Convert.ToDecimal(product.BasePrice);
            item.Quantity = int.Parse(product.Quantity);
        }
    }
}