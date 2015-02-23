using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
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
            Progress.Detail = "Downloading New Orders...";

            GrouponWebClient client = new GrouponWebClient((GrouponStoreEntity)Store);
            
            int currentPage = 0;
            int numberOfPages = 0;

            try
            {
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

                        LoadOrder(jsonOrder);
                    }

                } while(currentPage < numberOfPages);

            }
            catch (GrouponException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }

        }

        /// <summary>
        /// LoadORder from JToken
        /// </summary>
        private void LoadOrder(JToken jsonOrder)
        {
            string orderid =  jsonOrder["orderid"].ToString();
            GrouponOrderEntity order = (GrouponOrderEntity)InstantiateOrder(new GrouponOrderIdentifier(orderid));

            // Nothing to do if its not new - they don't change
            if (!order.IsNew)
            {
                return;
            }

            // set the progress detail
            Progress.Detail = String.Format("Processing order {0}...", QuantitySaved + 1);

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
            string itemWeightUnit = jsonOrder["shipping"].Value<string>("product_weight_unit") ?? "";

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
        private void LoadItem(GrouponOrderEntity order, GrouponItem grouponItem, string itemWeightUnit)
        {
            GrouponOrderItemEntity item = (GrouponOrderItemEntity)InstantiateOrderItem(order);

            item.SKU = grouponItem.Sku;
            item.Name = grouponItem.Name;
            item.Weight = GetWeight(grouponItem.Weight, itemWeightUnit);
            item.UnitPrice = grouponItem.UnitPrice;
            item.Quantity = grouponItem.Quantity;

            //Groupon fields
            item.Permalink = grouponItem.Permalink;
            item.ChannelSKUProvided = grouponItem.ChannelSkuProvided;
            item.FulfillmentLineItemID = grouponItem.FulfillmentLineitemId;
            item.BomSKU = grouponItem.BomSku;
            item.GrouponLineItemID = grouponItem.GrouponLineitemId;
        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private static void LoadAddressInfo(GrouponOrderEntity order, GrouponCustomer customer)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            PersonName name = PersonName.Parse(customer.Name);
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            shipAdapter.FirstName = name.First;
            shipAdapter.MiddleName = name.First;
            shipAdapter.LastName = name.Last;
            shipAdapter.Street1 = customer.Address1;
            shipAdapter.Street2 = customer.Address2;
            shipAdapter.City = customer.City;
            //Groupon can send "null" as the state, check for null test and use blank instead 
            shipAdapter.StateProvCode = Geography.GetStateProvCode((customer.State == "null") ? customer.State : "");
            shipAdapter.PostalCode = customer.Zip;
            shipAdapter.CountryCode = Geography.GetCountryCode((customer.Country == "null") ? customer.Country : "");
            shipAdapter.Phone = customer.Phone;

            //Groupon does not provide a bill to address so we copy from shipping to billing
            PersonAdapter.Copy(shipAdapter, billAdapter);
        }

        /// <summary>
        /// convert to lbs from Groupon weight
        /// </summary>
        private static double GetWeight(double weight, string itemWeightUnit)
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

        /// <summary>
        /// Remove UTC from end of Groupon date time string
        /// </summary>
        private static DateTime GetDate(string date)
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
