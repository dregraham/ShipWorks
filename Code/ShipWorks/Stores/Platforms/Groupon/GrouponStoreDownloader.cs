using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using System.Text.RegularExpressions;
using log4net;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponStoreDownloader : StoreDownloader
    {

        public GrouponStoreDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Downloading New Orders...";

            GrouponStoreWebClient client = new GrouponStoreWebClient((GrouponStoreEntity)Store);
            
            int currentPage = 1;
            int numberOfPages = 1;

            do 
            {
                //Grab orders 
                JObject result = client.GetOrders(currentPage);

                // get JSON result objects into a list
                IList<JToken> jsonOrders = result["data"].Children().ToList();

                //Load orders 
                LoadOrders(jsonOrders);

                //Update currentPage
                currentPage = Convert.ToInt16(result["meta"]["current_page"].ToString());
                numberOfPages = Convert.ToInt16(result["meta"]["no_of_pages"].ToString());

            } while(currentPage < numberOfPages);
        }

        private void LoadOrders(IList<JToken> jsonOrders)
        {
            foreach (JToken jsonOrder in jsonOrders)
            {
                LoadOrder(jsonOrder);


            }
        }

        private void LoadOrder(JToken jsonOrder)
        {
            string orderid =  jsonOrder["orderid"].ToString();
            GrouponOrderEntity order = (GrouponOrderEntity)InstantiateOrder(new GrouponStoreOrderIdentifier(orderid));

            // Nothing to do if its not new - they don't change
            if (!order.IsNew)
            {
                return;
            }

            //OrderNumber
            order.OrderNumber = GetNextOrderNumber();

            //Order Date
            DateTime orderDate = GetDate(jsonOrder["date"].ToString());
            order.OrderDate = orderDate;
            order.OnlineLastModified = orderDate;

            //Order Address
            JToken jsonAddress = jsonOrder["customer"];
            LoadAddressInfo(order, jsonAddress);

            //Order requestedshipping
            order.RequestedShipping = jsonOrder["shipping"]["method"].ToString();

            //Order Items
            IList<JToken> jsonItems = jsonOrder["line_items"].Children().ToList();
            foreach (JToken jsonItem in jsonItems)
            {
                LoadItem(order, jsonItem);
            }

            //OrderTotal
            order.OrderTotal = Convert.ToDecimal(jsonOrder["amount"]["total"].ToString());

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GrouponStoreDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadItem(GrouponOrderEntity order, JToken jsonItem)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.SKU = jsonItem["sku"].ToString();
            item.Name = jsonItem["name"].ToString();
            item.Weight = Convert.ToDouble(jsonItem["weight"].ToString());
            item.Code = jsonItem["ci_lineitemid"].ToString();
            item.UnitPrice = Convert.ToDecimal(jsonItem["unit_price"].ToString());
            item.Quantity = Convert.ToInt16(jsonItem["quantity"].ToString());

        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private void LoadAddressInfo(GrouponOrderEntity order, JToken address)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // Fill in the address info
            LoadAddressInfo(shipAdapter ,address ,"Ship");

            //Groupon does not provide a bill to address so we copy from shipping to billing
            PersonAdapter.Copy(shipAdapter, billAdapter);
        }

        /// <summary>
        /// Loads address information from xml to the person adapter
        /// </summary>
        private void LoadAddressInfo(PersonAdapter adapter, JToken address, string prefix)
        {
            PersonName name = PersonName.Parse(address["name"].ToString());
            adapter.NameParseStatus = PersonNameParseStatus.Simple; 
            adapter.FirstName = name.First;
            adapter.MiddleName = name.First;
            adapter.LastName = name.Last;
            adapter.Street1 = address["address1"].ToString();
            adapter.Street2 = address["address2"].ToString();
            adapter.City = address["city"].ToString();
            adapter.StateProvCode = Geography.GetStateProvCode(address["state"].ToString());
            adapter.PostalCode = address["zip"].ToString();
            adapter.CountryCode = Geography.GetCountryCode(address["country"].ToString());
            adapter.Phone = address["phone"].ToString();
        }

        private DateTime GetDate(string date)
        {

            int TimeZonePos = date.IndexOf("UTC");

            if (TimeZonePos > 0)
            {
                return DateTime.Parse(date.Substring(0, TimeZonePos));
            }
            else
            {
                return DateTime.Parse(date);
            }
            
        }
        

    }
}
