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
using Interapptive.Shared.Enums;
using ShipWorks.Stores.Platforms.Groupon.DTO;

namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponDownloader : StoreDownloader
    {

        public GrouponDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Downloading Orders...";

            GrouponWebClient client = new GrouponWebClient((GrouponStoreEntity)Store);
            
            int currentPage = 0;
            int numberOfPages = 0;
            int numberOfOrders = 0;

            do 
            {
                // check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                currentPage++;

                //Grab orders 
                JToken result = client.GetOrders(currentPage);

                //Number of Orders
                numberOfOrders = (int)result["meta"]["no_of_items"];

                //Update numberOfPages
                numberOfPages = (int)result["meta"]["no_of_pages"];

                // get JSON result objects into a list
                IList<JToken> jsonOrders = result["data"].Children().ToList();

                //Load orders 
                foreach (JToken jsonOrder in jsonOrders)
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    // set the progress detail
                    Progress.Detail = String.Format("Processing order {0}...", QuantitySaved + 1);

                    LoadOrder(jsonOrder);

                    // move the progress bar along
                    Progress.PercentComplete = Math.Min(100 * QuantitySaved / numberOfOrders, 100);

                }

            } while(currentPage < numberOfPages);
        }

        private void LoadOrder(JToken jsonOrder)
        {
            string orderid =  jsonOrder["orderid"].ToString();
            GrouponOrderEntity order = (GrouponOrderEntity)InstantiateOrder(new GrouponOrderIdentifier(orderid));

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
            GrouponCustomer customer = JsonConvert.DeserializeObject<GrouponCustomer>(jsonOrder["customer"].ToString());
            LoadAddressInfo(order, customer);

            //Order requestedshipping
            order.RequestedShipping = jsonOrder["shipping"]["method"].ToString();

            //Unit of measurement used for weight  
            string itemWeightUnit = jsonOrder["shipping"]["product_weight_unit"].ToString();

            //List of order items
            IList<JToken> jsonItems = jsonOrder["line_items"].Children().ToList();
            foreach (JToken jsonItem in jsonItems)
            {
                //Deserialized into grouponitem
                GrouponItem item = JsonConvert.DeserializeObject<GrouponItem>(jsonItem.ToString());
                LoadItem(order, item, itemWeightUnit);
            }

            //OrderTotal
            order.OrderTotal = (decimal)jsonOrder["amount"]["total"];

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GrouponStoreDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadItem(GrouponOrderEntity order, GrouponItem gItem, string itemWeightUnit)
        {
            GrouponOrderItemEntity item = (GrouponOrderItemEntity)InstantiateOrderItem(order);

            item.SKU = gItem.sku;
            item.Name = gItem.name;
            item.Weight = GetWeight(gItem.weight,itemWeightUnit);
            item.UnitPrice = gItem.unit_price;
            item.Quantity = gItem.quantity;

            //Groupon fields
            item.Permalink = gItem.permalink;
            item.ChannelSKUProvided = gItem.channel_sku_provided;
            item.FulfillmentLineitemID = gItem.fulfillment_lineitem_id;
            item.BomSKU = gItem.bom_sku;
            item.CILineItemID = gItem.ci_lineitemid;
        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private void LoadAddressInfo(GrouponOrderEntity order, GrouponCustomer customer)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            PersonName name = PersonName.Parse(customer.name);
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            shipAdapter.FirstName = name.First;
            shipAdapter.MiddleName = name.First;
            shipAdapter.LastName = name.Last;
            shipAdapter.Street1 = customer.address1;
            shipAdapter.Street2 = customer.address2;
            shipAdapter.City = customer.city;
            shipAdapter.StateProvCode = Geography.GetStateProvCode(customer.state);
            shipAdapter.PostalCode = customer.zip;
            shipAdapter.CountryCode = Geography.GetCountryCode(customer.country);
            shipAdapter.Phone = customer.phone;

            //Groupon does not provide a bill to address so we copy from shipping to billing
            PersonAdapter.Copy(shipAdapter, billAdapter);
        }


        private double GetWeight(double weight, string itemWeightUnit)
        {
            //Groupon sometimes sends weight in ounches, convert from ounches to lbs in that case
            switch (itemWeightUnit)
            {
                case "ounces":
                    return WeightUtility.Convert(WeightUnitOfMeasure.Ounces, WeightUnitOfMeasure.Pounds, weight);
                default:
                    return weight;
            }
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
