﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon.DTO;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Groupon)]
    public class GrouponDownloader : StoreDownloader
    {
        private readonly ILog log;
        private readonly ICombineOrder orderCombiner;
        private readonly IDataProvider dataProvider;
        private readonly IGrouponWebClient webClient;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GrouponDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IGrouponWebClient webClient,
            ICombineOrder orderCombiner,
            IDataProvider dataProvider,
            IDateTimeProvider dateTimeProvider,
            Func<Type, ILog> createLogger,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager.GetType(store))
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.webClient = webClient;
            this.dataProvider = dataProvider;
            this.orderCombiner = orderCombiner;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Downloading New Orders...";

            DateTime start = dateTimeProvider.UtcNow.AddDays(-7);

            try
            {
                do
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    int currentPage = 1;
                    int numberOfPages = 1;

                    do
                    {
                        //Grab orders
                        JToken result = await webClient.GetOrders((GrouponStoreEntity) Store, start, currentPage).ConfigureAwait(false);

                        //Update numberOfPages
                        numberOfPages = (int) result["meta"]["no_of_pages"];

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

                            await LoadOrder(jsonOrder).ConfigureAwait(false);
                        }

                        currentPage++;

                    } while (currentPage <= numberOfPages);

                    start = start.AddHours(23);

                } while (start <= dateTimeProvider.UtcNow);

                Progress.Detail = "Done";
                Progress.PercentComplete = 100;
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
        private async Task LoadOrder(JToken jsonOrder)
        {
            string orderid = jsonOrder["orderid"].ToString();

            GenericResult<OrderEntity> result = await InstantiateOrder(new GrouponOrderIdentifier(orderid)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderid, result.Message);
                return;
            }

            GrouponOrderEntity order = (GrouponOrderEntity) result.Value;

            //Order Item Status
            string status = jsonOrder["line_items"].Children().First()["status"].ToString() ?? "";

            if (order.IsNew && status != "open")
            {
                return;
            }

            // Order already exists or is new and of status open
            order.OnlineStatus = GetOrderStatusName(status);
            order.OnlineStatusCode = GetOrderStatusName(status);

            // set the progress detail
            Progress.Detail = String.Format("Processing order {0}...", QuantitySaved + 1);

            //Order Date
            DateTime orderDate = GetDate(jsonOrder["date"].ToString());
            order.OrderDate = orderDate;
            order.OnlineLastModified = orderDate;

            order.ParentOrderID = jsonOrder["parent_orderid"]?.ToString();

            //Order Address
            GrouponCustomer customer = JsonConvert.DeserializeObject<GrouponCustomer>(jsonOrder["customer"].ToString());
            LoadAddressInfo(order, customer);

            //Order requested shipping
            order.RequestedShipping = jsonOrder["shipping"]["method"].ToString();

            //Order is new and its status is open
            if (order.IsNew)
            {
                await LoadOrderDetailsWhenNew(jsonOrder, order).ConfigureAwait(false);
            }

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GrouponStoreDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            await CombineWithParent(order.OrderID).ConfigureAwait(false);
        }

        /// <summary>
        /// Combine the child order with a parent order if one exists.
        /// </summary>
        private async Task<GenericResult<long>> CombineWithParent(long childOrderID)
        {
            GrouponOrderEntity childOrder = dataProvider.GetEntity(childOrderID) as GrouponOrderEntity;
            if (childOrder.ParentOrderID.IsNullOrWhiteSpace())
            {
                return GenericResult.FromError<long>("No parent order found.");
            }

            GrouponOrderEntity parentOrder;
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                // Look for orders that have the same ParentOrderID as the child order
                EntityQuery<GrouponOrderEntity> parentOrderQuery = new QueryFactory().GrouponOrder
                    .Where(GrouponOrderFields.ParentOrderID == childOrder.ParentOrderID & GrouponOrderFields.OrderID != childOrderID);

                parentOrder = await adapter.FetchFirstAsync(parentOrderQuery).ConfigureAwait(false);
            }

            if (parentOrder == null)
            {
                return GenericResult.FromError<long>("No parent order found.");
            }

            return await orderCombiner.Combine(parentOrder.OrderID, new[] { childOrder, parentOrder }, parentOrder.OrderNumberComplete).ConfigureAwait(false);
        }

        /// <summary>
        /// Load order details when the order is new
        /// </summary>
        private async Task LoadOrderDetailsWhenNew(JToken jsonOrder, GrouponOrderEntity order)
        {
            // The order number format seemed to change on 2015-06-03 so that it no longer is guaranteed to have any numeric components
            order.OrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);

            //Unit of measurement used for weight
            string itemWeightUnit = jsonOrder["shipping"].Value<string>("product_weight_unit") ?? "";

            //List of order items
            IList<JToken> jsonItems = jsonOrder["line_items"].Children().ToList();
            foreach (JToken jsonItem in jsonItems)
            {
                // Deserialized into grouponitem
                GrouponItem item = JsonConvert.DeserializeObject<GrouponItem>(jsonItem.ToString());
                LoadItem(order, item, itemWeightUnit);
            }

            //OrderTotal
            order.OrderTotal = (decimal) jsonOrder["amount"]["total"];
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadItem(GrouponOrderEntity order, GrouponItem grouponItem, string itemWeightUnit)
        {
            GrouponOrderItemEntity item = (GrouponOrderItemEntity) InstantiateOrderItem(order);

            double itemWeight = (String.IsNullOrWhiteSpace(grouponItem.Weight)) ? 0 : Convert.ToDouble(grouponItem.Weight);

            item.SKU = grouponItem.Sku;
            item.Code = grouponItem.FulfillmentLineitemId;
            item.Name = grouponItem.Name;
            item.Weight = GetWeight(itemWeight, itemWeightUnit);
            item.UnitPrice = Convert.ToDecimal(grouponItem.UnitPrice);
            item.Quantity = Convert.ToInt16(grouponItem.Quantity);

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
            shipAdapter.MiddleName = name.Middle;
            shipAdapter.LastName = name.Last;
            shipAdapter.Street1 = customer.Address1;
            shipAdapter.Street2 = customer.Address2;
            shipAdapter.City = customer.City;
            //Groupon can send "null" as the state, check for null test and use blank instead
            shipAdapter.StateProvCode = Geography.GetStateProvCode(customer.State);
            shipAdapter.PostalCode = customer.Zip;
            shipAdapter.CountryCode = Geography.GetCountryCode(customer.Country);
            shipAdapter.Phone = customer.Phone;

            //Groupon does not provide a bill to address so we copy from shipping to billing
            PersonAdapter.Copy(shipAdapter, billAdapter);
        }

        /// <summary>
        /// convert to lbs from Groupon weight
        /// </summary>
        private static double GetWeight(double weight, string itemWeightUnit)
        {
            // Groupon sometimes sends weight in ounces, convert from ounces to lbs in that case
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
            int TimeZonePos = date.IndexOf("UTC", StringComparison.Ordinal);

            return TimeZonePos > 0 ?
                DateTime.Parse(date.Substring(0, TimeZonePos)) :
                DateTime.Parse(date);
        }

        /// <summary>
        /// Get the display name for the given order status code
        /// </summary>
        public static string GetOrderStatusName(string orderStatus)
        {
            switch (orderStatus)
            {
                case "ship": return "Shipped";
                case "open": return "Open";
            }

            return orderStatus;
        }
    }
}
