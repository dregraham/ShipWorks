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
    /// Downloader for LemonStand
    /// </summary>
    class LemonStandDownloader : StoreDownloader
    {
        public LemonStandDownloader(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Download orders from LemonStand
        /// </summary>
        /// <exception cref="DownloadException">
        /// </exception>
        protected override void Download()
        {
            Progress.Detail = "Downloading New Orders...";

            LemonStandWebClient client = new LemonStandWebClient((LemonStandStoreEntity)Store);
           
            try
            {

                // check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                //get orders from LemonStand 
                JToken result = client.GetOrders();

                // get JSON result objects into a list
                IList<JToken> jsonOrders = result["data"].Children().ToList();
                int expectedCount = jsonOrders.Count;

                //Load orders 
                foreach (JToken jsonOrder in jsonOrders)
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    LoadOrder(jsonOrder, client);

                    // set the progress detail
                    Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved, expectedCount);
                    Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);                    
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
        /// Load Order from JToken
        /// </summary>
        private void LoadOrder(JToken jsonOrder, LemonStandWebClient client)
        {
            //                              order
            //                          /     |      \
            //                    invoices  customer  items 
            //                      /         |          \
            //                shipment  billing_address  product 
            //                  /
            //          shipment_address

            //Deserialize Json Order into order DTO
            LemonStandOrder lsOrder = JsonConvert.DeserializeObject<LemonStandOrder>(jsonOrder.ToString());
            int orderID = int.Parse(lsOrder.ID);
            LemonStandOrderEntity order = (LemonStandOrderEntity)InstantiateOrder(new LemonStandOrderIdentifier(orderID.ToString()));            
            
            order.OnlineStatus = lsOrder.Status;            

            // Only load new orders
            if (order.IsNew) 
            {                
                order.OrderDate = getDate(lsOrder.CreatedAt);
                order.OnlineLastModified = getDate(lsOrder.UpdatedAt);
                order.OrderNumber = int.Parse(lsOrder.Number);
                
                // Need invoice id to get shipment information
                LemonStandInvoice invoice = JsonConvert.DeserializeObject<LemonStandInvoice>(jsonOrder.SelectToken("invoices.data").Children().First().ToString());
                
                // Get shipment information and set requested shipping
                JToken jsonShipment = client.GetShipment(invoice.ID);
                LemonStandShipment shipment = JsonConvert.DeserializeObject<LemonStandShipment>(jsonShipment.SelectToken("data.shipments.data").Children().First().ToString());

                order.RequestedShipping = shipment.ShippingService.ToString();

                // Get shipping address from shipment
                JToken jsonShippingAddress = client.GetShippingAddress(shipment.ID);
                LemonStandShippingAddress shippingAddress = JsonConvert.DeserializeObject<LemonStandShippingAddress>(jsonShippingAddress.SelectToken("data.shipping_address.data").ToString());
            
                // Get customer information and billing address
                LemonStandCustomer customer = JsonConvert.DeserializeObject<LemonStandCustomer>(jsonOrder.SelectToken("customer.data").ToString());
                JToken jsonBillingAddress = client.GetBillingAddress(customer.ID);
                LemonStandBillingAddress billingAddress = JsonConvert.DeserializeObject<LemonStandBillingAddress>(jsonBillingAddress.SelectToken("data.billing_addresses.data").Children().First().ToString());

                string email = customer.Email;

                // Load shipping and billing address
                LoadAddressInfo(order, shippingAddress, billingAddress, email);

                // Load order items
                LoadItems(jsonOrder, client, order);

                order.OrderTotal = decimal.Parse(lsOrder.SubtotalPaid);
            }
            
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "LemonStandStoreDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Converts LemonStand date to UTC
        /// </summary>
        /// <param name="date">The date from LemonStand</param>
        /// <returns>DateTime in UTC</returns>
        private DateTime getDate(string date) 
        {
            DateTime result = DateTime.UtcNow;
            
            DateTime.TryParse(date, out result);

            return result.ToUniversalTime();
        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private static void LoadAddressInfo(LemonStandOrderEntity order, LemonStandShippingAddress shipAddress, LemonStandBillingAddress billAddress, string email)
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
        /// Loads the order items.
        /// </summary>
        /// <param name="jsonOrder">The json order.</param>
        /// <param name="client">The client.</param>
        /// <param name="order">The order.</param>
        private void LoadItems(JToken jsonOrder, LemonStandWebClient client, LemonStandOrderEntity order)
        {
            //List of order items
            IList<JToken> jsonItems = jsonOrder.SelectToken("items.data").Children().ToList();
            foreach (JToken jsonItem in jsonItems)
            {
                string productID = jsonItem.SelectToken("shop_product_id").ToString();

                JToken jsonProduct = client.GetProduct(productID);

                //Deserialize into LemonStand item
                LemonStandItem product = JsonConvert.DeserializeObject<LemonStandItem>(jsonProduct.SelectToken("data").ToString());
                product.Quantity = jsonItem.SelectToken("quantity").ToString();
                LoadItem(order, product);
            }
        }

        /// <summary>
        /// Load an order item
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
            item.Weight = (String.IsNullOrWhiteSpace(product.Weight)) ? 0 : Convert.ToDouble(product.Weight);
            item.UnitPrice = Convert.ToDecimal(product.BasePrice);
            item.Quantity = Convert.ToInt16(product.Quantity);
        }        
    }
}
