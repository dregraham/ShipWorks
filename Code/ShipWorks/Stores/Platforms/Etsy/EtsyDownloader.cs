using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Etsy.Enums;
using System.Text;
using log4net;
using Newtonsoft.Json.Linq;
using Interapptive.Shared.Net;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Downloader for EtsyStores
    /// </summary>
    public class EtsyDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyDownloader));

        int totalCount = 0;
        const int goBackDaysForUnpaid = 60;
        const int goBackDaysForUnshipped = 60;
        EtsyWebClient webClient;
        EtsyStoreType storeType;

        /// <summary>
        /// Number of orders to download at a time.
        /// </summary>
        const int limit = 100;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyDownloader(EtsyStoreType etsyStoreType, EtsyStoreEntity store)
            : base(store)
        {
            if (etsyStoreType == null)
            {
                throw new ArgumentNullException("etsyStoreType");
            }
            webClient = new EtsyWebClient(etsyStoreType.EtsyStore);
            storeType = etsyStoreType;
        }

        /// <summary>
        /// Download data for the Etsy store
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Checking for shipped orders...";
                UpdateShippedOrders();

                Progress.Detail = "Checking for paid orders...";
                UpdatePaidOrders();

                Progress.Detail = "Checking for new orders...";
                DownloadNewOrders();
            }
            catch (EtsyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets all locally unshipped orders. If Etsy says they are shipped, we update our DB.
        /// </summary>
        private void UpdateShippedOrders()
        {
            //Grab the orders that currently have the orderStatusField as false
            List<EtsyOrderEntity> shipWorksOrders = GetOrdersByEtsyStatus(EtsyOrderFields.WasShipped, false, goBackDaysForUnshipped, EtsyOrderStatus.Open);

            //Query etsy for those orders whose status has changed
            List<EtsyOrderEntity> newlyChangedOrders = EtsyOrderStatusUtility.GetOrdersWithChangedStatus(storeType, shipWorksOrders, "was_shipped", false);

            //Update those statuses locally
            foreach (var changedOrder in newlyChangedOrders)
            {
                //Etsy says they are shipped, so we mark them as shipped too.
                EtsyOrderStatusUtility.UpdateOrderStatus(changedOrder, null, true);
            }
        }

        /// <summary>
        /// Gets all locally unpaid orders. If Etsy says they are paid, we update our DB.
        /// </summary>
        private void UpdatePaidOrders()
        {
            //Grab the orders that currently have the orderStatusField as false
            List<EtsyOrderEntity> shipWorksOrders = GetOrdersByEtsyStatus(EtsyOrderFields.WasPaid, false, goBackDaysForUnpaid, EtsyOrderStatus.Open);

            //Query etsy for those orders whose status has changed
            List<EtsyOrderEntity> newlyChangedOrders = EtsyOrderStatusUtility.GetOrdersWithChangedStatus(storeType, shipWorksOrders, "was_paid", false);
            UpdatePaymentInformation(newlyChangedOrders);
        }

        /// <summary>
        /// Update the payment information in the orders List.
        /// </summary>
        private void UpdatePaymentInformation(List<EtsyOrderEntity> orders)
        {
            //Use tempOrders as a queue of unprocessed orders
            List<EtsyOrderEntity> tempOrders = new List<EtsyOrderEntity>();
            tempOrders.AddRange(orders);

            while (tempOrders.Count() > 0)
            {
                //Get a batch of order numbers
                var pageOfOrderNumbers = (from x in tempOrders.Take(EtsyEndpoints.GetOrderLimit)
                                          select x);

                UpdatePaymentInformationBatch(pageOfOrderNumbers);

                //remove the processed orders
                tempOrders.RemoveRange(0, pageOfOrderNumbers.Count());
            }

            using (EntityCollection<EtsyOrderEntity> collection = new EntityCollection<EtsyOrderEntity>(orders))
            {
                SqlAdapter.Default.SaveEntityCollection(collection);
            }            
        }

        /// <summary>
        /// Update a batch of orders. Currently Etsy can only handle 100 at a time. This doesn't actually save the orders.
        /// </summary>
        private void UpdatePaymentInformationBatch(IEnumerable<EtsyOrderEntity> pageOfOrderNumbers)
        {
            //format the order numbers into a comma seperated list
            string formattedOrderNumbers = string.Join(",", pageOfOrderNumbers.Select(x => x.OrderNumber.ToString()));

            var orderDetails = webClient.GetPaymentInformation(formattedOrderNumbers);

            foreach (JToken orderPaymentStatus in orderDetails)
            {
                EtsyOrderEntity order = pageOfOrderNumbers.Single(o => o.OrderNumber == (long) orderPaymentStatus["receipt_id"]);

                LoadPaymentDetail(order, "Payment Detail", orderPaymentStatus.GetValue("message_from_payment", ""), true);

                order.WasPaid = true;
            }
        }

        /// <summary>
        /// Gets Entity orders based on field value.
        /// </summary>
        private List<EtsyOrderEntity> GetOrdersByEtsyStatus(EntityField2 field, bool value, int daysBack, EtsyOrderStatus onlineStatusCode)
        {
            using (EntityCollection<EtsyOrderEntity> swUnpaidOrders = new EntityCollection<EtsyOrderEntity>())
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    IRelationPredicateBucket filter = new RelationPredicateBucket(((field == value)
                        & (EtsyOrderFields.OnlineStatusCode == (int) onlineStatusCode)
                        & (EtsyOrderFields.StoreID == Store.StoreID)
                        & EtsyOrderFields.OrderDate >= DateTime.UtcNow.AddDays(-daysBack)));

                    adapter.FetchEntityCollection(swUnpaidOrders, filter);
                }
                return swUnpaidOrders.ToList();
            }
        }

        /// <summary>
        /// Handles the downloading of orders.
        /// </summary>
        private void DownloadNewOrders()
        {
            while (true)
            {
                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                if (!DownloadNextOrdersPage())
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Download the next page of orders. Return True if more to process.
        /// </summary>
        private bool DownloadNextOrdersPage()
        {
            DateTime startDate;
            DateTime endDate;
            bool isMoreToProcess = true;

            GetDateRange(out startDate, out endDate);

            int offset = GetOffset(startDate, endDate);

            // 'GetOffset' sets the total count.  Should probably be refactored
            if (totalCount == 0)
            {
                Progress.Detail = "No orders to download.";
                return false;
            }

            List<JToken> orders = webClient.GetOrders(startDate, endDate, limit, offset);

            // If any orders were downloaded we have to import them
            if (orders.Count > 0)
            {
                //Load orders into the database
                LoadOrders(orders);
            }

            if (offset == 0)
            {
                isMoreToProcess = false;
                Progress.Detail = "Done.";
            }

            return isMoreToProcess;
        }

        /// <summary>
        /// Gets the date range for the next batch of orders.
        /// </summary>
        private void GetDateRange(out DateTime startDate, out DateTime endDate)
        {
            // Gets the current time from Etsy and subtracts 5 minutes.
            endDate = webClient.GetEtsyDateTime().AddMinutes(-5);

            //The most recent order date. 
            DateTime? calculatedStartDate = GetOrderDateStartingPoint();

            if (calculatedStartDate.HasValue)
            {
                startDate = calculatedStartDate.Value;
            }
            else
            {
                //They must have chosen all orders. Start at the beginning of time for store.
                startDate = webClient.GetStoreCreationDate();
            }
        }

        /// <summary>
        /// Get the offset to use for an Etsy Query.
        /// (If there are 1000 orders and the limit is 100, 900 will be returned. When offset is used, the 100 oldest orders will download)
        /// </summary>
        private int GetOffset(DateTime startDate, DateTime endDate)
        {
            //Gets number of orders between dates.
            int currentCount = webClient.GetOrderCount(startDate, endDate);

            //If totalCount hasn't been set, this is the first time through. Set it.
            if (totalCount == 0)
            {
                totalCount = currentCount;
            }

            int offset = 0;

            if (currentCount > limit)
            {
                offset = currentCount - limit;
            }

            return offset;
        }

        /// <summary>
        /// Load all the orders contained in the iterator
        /// </summary>
        private void LoadOrders(List<JToken> orders)
        {
            // Go through each order in the XML Document
            foreach (var order in orders)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                LoadOrder(order);

                // Update the status
                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }
        }

        /// <summary>
        /// Extract and save the order from the downloaded order
        /// </summary>
        private void LoadOrder(JToken orderFromEtsy)
        {
            // Now extract the Order#
            long orderNumber = (long) orderFromEtsy["receipt_id"];

            // Get the order instance
            OrderEntity orderEntity = InstantiateOrder(new OrderNumberIdentifier(orderNumber));
            EtsyOrderEntity order = (EtsyOrderEntity)orderEntity;

            // Set the total.  It will be calculated and verified later.
            order.OrderTotal = orderFromEtsy.GetValue("grandtotal", 0m);

            // Dates
            double creationUnixTime = (double) orderFromEtsy["creation_tsz"];
            order.OrderDate = DateTimeUtility.FromUnixTimestamp(creationUnixTime);

            // Customer
            order.OnlineCustomerID = orderFromEtsy.GetValue("buyer_user_id", 0);

            // Requested shipping - this doesn't come through the API.
            order.RequestedShipping = "";

            // Load address info
            LoadOrderAddress(order, orderFromEtsy);

            order.OnlineLastModified = DateTimeUtility.FromUnixTimestamp(orderFromEtsy.GetValue("last_modified_tsz", creationUnixTime));
            order.WasPaid = orderFromEtsy.GetValue("was_paid", false);
            order.WasShipped = orderFromEtsy.GetValue("was_shipped", false);

            // Only update the rest for brand new orders
            if (order.IsNew)
            {
                InstantiateNote(order, WebUtility.HtmlDecode(orderFromEtsy.GetValue("message_from_buyer", "")), order.OrderDate, NoteVisibility.Public);

                // Items
                int loadedItems = LoadItems(order, (JArray) orderFromEtsy["Transactions"]);
                if (loadedItems > 0)
                {
                    // If the order total doesn't match the sum of the transactions, then it means we've probably got more
                    // transactions to load. Etsy doesn't currently give us pagination data for related data and I don't think
                    // it would be a good idea to rely on the default limit to stay 25.
                    decimal totalPrice = orderFromEtsy.GetValue("total_price", 0m);
                    decimal itemPrice = order.OrderItems.Select(x => x.UnitPrice * Convert.ToDecimal(x.Quantity)).Sum();

                    if (totalPrice != itemPrice)
                    {
                        int offset = loadedItems;
                        int totalTransactions = 0;

                        do
                        {
                            // Get another batch of transactions for the current order
                            JToken transactionToken = webClient.GetTransactionsForReceipt(orderNumber, limit, offset);

                            totalTransactions = transactionToken.GetValue("count", 0);
                            loadedItems = LoadItems(order, (JArray)transactionToken["results"]);

                            offset = offset + loadedItems;

                            // We want to loop until our item count matches the total transaction, but
                            // stopping when there were no items loaded should stop an infinite loop if
                            // something goes wrong and etsy returns no transactions before we're done
                        } while (loadedItems > 0 && order.OrderItems.Count < totalTransactions);
                    }
                }

                // Load all the charges
                LoadOrderCharges(order, orderFromEtsy);

                // Load all payment details
                LoadPaymentDetail(order, "Payment Detail", orderFromEtsy.GetValue("message_from_payment", ""));
                LoadPaymentDetail(order, "Payment Type", GetPaymentMethod(orderFromEtsy.GetValue("payment_method", "")));
            }

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "EtsyDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Loads items into the order from a collection of transactions
        /// </summary>
        /// <returns>The number of items loaded</returns>
        private int LoadItems(EtsyOrderEntity order, JArray transactions)
        {
            if (transactions == null || transactions.Count <= 0)
            {
                return 0;
            }

            foreach (JToken transaction in transactions)
            {
                LoadItem(order, transaction);
            }

            return transactions.Count;
        }

        /// <summary>
        /// Load the order address information
        /// </summary>
        private static void LoadOrderAddress(EtsyOrderEntity order, JToken orderFromEtsy)
        {
            PersonName buyer = PersonName.Parse(orderFromEtsy.GetValue("name", ""));
            order.ShipFirstName = buyer.First;
            order.ShipLastName = buyer.LastWithSuffix;
            order.ShipMiddleName = buyer.Middle;
            order.ShipNameParseStatus = (int) buyer.ParseStatus;
            order.ShipUnparsedName = buyer.UnparsedName;

            order.ShipCompany = string.Empty;
            order.ShipStreet1 = orderFromEtsy.GetValue("first_line", "");
            order.ShipStreet2 = orderFromEtsy.GetValue("second_line", "");
            order.ShipCity = orderFromEtsy.GetValue("city", "");
            order.ShipStateProvCode = Geography.GetStateProvCode(orderFromEtsy.GetValue("state", ""));
            order.ShipPostalCode = orderFromEtsy.GetValue("zip", "");
            order.ShipCountryCode = orderFromEtsy["Country"].GetValue("iso_country_code", "US");

            order.ShipPhone = string.Empty;
            order.ShipEmail = orderFromEtsy.GetValue("buyer_email", "");

            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");
            PersonAdapter.Copy(shipAdapter, billAdapter);
        }

        /// <summary>
        /// Given the etsy payment code, return a more user friendly name.
        /// </summary>
        private string GetPaymentMethod(string etsyPaymentMethodCode)
        {
            switch (etsyPaymentMethodCode)
            {
                case "pp":
                    return "PayPal";
                case "cc":
                    return "Credit Card";
                case "ck":
                case "check":
                    return "Check";
                case "mo":
                    return "Money Order";
                case "other":
                    return "Other";
                default:
                    log.WarnFormat("Payment type of {0} not expected in EtsyDownloader.GetPaymentMethod", etsyPaymentMethodCode);
                    return etsyPaymentMethodCode;
            }
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(OrderEntity order, JToken transaction)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.Name = transaction.GetValue("title", "");
            item.Code = transaction.GetValue("transaction_id", "");
            item.SKU = transaction.GetValue("listing_id", "");
            item.Quantity = transaction.GetValue("quantity", 0);
            item.UnitPrice = transaction.GetValue("price", 0m);

            LoadOrderItemAttributes(item, (JArray) transaction["variations"]);

            JToken images = (JToken) transaction["MainImage"];
            if (images != null && images.Type != JTokenType.Null)
            {
                item.Thumbnail = images.GetValue("url_75x75", "");
                item.Image = images.GetValue("url_570xN", "");
            }
        }

        /// <summary>
        /// Loads the order item attributes.
        /// </summary>
        private void LoadOrderItemAttributes(OrderItemEntity item, JArray variations)
        {
            if (variations!=null)
            {
                foreach (JToken variation in variations)
                {
                    item.OrderItemAttributes.Add(new OrderItemAttributeEntity()
                    {
                        Name = variation.GetValue("formatted_name",""),
                        Description = variation.GetValue("formatted_value",""),
                        IsManual = false,
                        UnitPrice = 0
                    });
                }
            }
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, JToken orderFromEtsy)
        {
            // Charge - Discount
            decimal discount = orderFromEtsy.GetValue("discount_amt", 0m);

            if (discount > 0)
            {
                LoadCharge(order, "Discount", "Discount", -discount);
            }

            LoadCharge(order, "Tax", "Tax", orderFromEtsy.GetValue("total_tax_cost", 0m));
            LoadCharge(order, "Shipping", "Shipping", orderFromEtsy.GetValue("total_shipping_cost", 0m));
        }

        /// <summary>
        /// Load an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            if (name.Length == 0)
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Load the given payment detail into the ordr
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value, bool ignoreDuplicates = false)
        {
            value = Regex.Replace(value, "Paypal sent us the following information from the buyer.", "", RegexOptions.IgnoreCase).Trim();

            // If we need to ignore adding any notes that are dupes of ones that exist...
            if (ignoreDuplicates)
            {
                List<OrderPaymentDetailEntity> detailsToCheck = new List<OrderPaymentDetailEntity>();

                // Check any notes already attached to the order, new or not
                detailsToCheck.AddRange(order.OrderPaymentDetails);

                // If the order isn't new, check the ones in the database too
                if (!order.IsNew)
                {
                    detailsToCheck.AddRange(DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderPaymentDetailEntity).Select(pd => (OrderPaymentDetailEntity) pd));
                }

                // Just check the label, since the Value could have been truncated and not exactly match
                if (detailsToCheck.Any(pd => string.Compare(pd.Label, label, true) == 0))
                {
                    return;
                }
            }

            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }
    }
}
