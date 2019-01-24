﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Order downloader for Volusion stores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Volusion)]
    public class VolusionDownloader : StoreDownloader
    {
        private readonly IVolusionWebClient webClient;
        private readonly ILog log;

        // total number of orders to be imported
        // shipping method map
        private VolusionShippingMethods shippingMethods;

        // payment method map
        private VolusionPaymentMethods paymentMethods;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionDownloader(StoreEntity store, IVolusionWebClient webClient, IStoreTypeManager storeTypeManager, Func<Type, ILog> createLogger)
            : base(store, storeTypeManager.GetType(store))
        {
            this.webClient = webClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Convenience property for quick access to the specific entity
        /// </summary>
        protected VolusionStoreEntity VolusionStoreEntity => (VolusionStoreEntity) Store;

        /// <summary>
        /// Download orders from the store
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        [NDependIgnoreLongMethod]
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                // get the collection of currently chosen codes to be downloaded
                List<string> selectedStatuses = VolusionStoreEntity.DownloadOrderStatuses.Split(',').ToList();

                // Volusion requires an explicit value when querying for orders, cannot pass a range or list of statuses
                // download for each status
                foreach (string status in selectedStatuses)
                {
                    int quantitySaved = 0;
                    Progress.Detail = "Preparing to download orders...";
                    shippingMethods = new VolusionShippingMethods((VolusionStoreEntity) Store);
                    paymentMethods = new VolusionPaymentMethods((VolusionStoreEntity) Store);

                    Progress.Detail = string.Format("Downloading {0} Orders...", status);

                    // get all the orders - volusion just gives them all. At once.
                    IXPathNavigable ordersResponse = webClient.GetOrders(Store as IVolusionStoreEntity, status);

                    // check for cancel
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    // volusion returns a completely blank response sometimes.  This is
                    // apparently completely fine and A.OK.
                    if (ordersResponse == null)
                    {
                        Progress.PercentComplete = 100;
                        Progress.Detail = "Done.";
                        continue;
                    }

                    XPathNavigator xpath = ordersResponse.CreateNavigator();

                    int totalCount = GetOrderCount(xpath);
                    if (totalCount == 0)
                    {
                        Progress.PercentComplete = 100;
                        Progress.Detail = "Done.";
                        continue;
                    }

                    XPathNodeIterator orders = xpath.Select("//Orders");
                    while (orders.MoveNext())
                    {
                        Progress.Detail = String.Format("Processing order {0} of {1}...", quantitySaved, totalCount);

                        // load each order
                        await LoadOrder(orders.Current.Clone()).ConfigureAwait(false);

                        Progress.PercentComplete = Math.Min(100, 100 * (quantitySaved) / totalCount);

                        quantitySaved++;

                        // check for cancel
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }
                    }
                }
            }
            catch (VolusionException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Processes a single order
        /// </summary>
        private async Task LoadOrder(XPathNavigator xpath)
        {
            long orderNumber = XPathUtility.Evaluate(xpath, "OrderID", 0);

            // find an existing order in ShipWorks or create a new one
            GenericResult<OrderEntity> result = await InstantiateOrder(orderNumber).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumber, result.Message);
                return;
            }

            OrderEntity order = result.Value;

            order.OrderDate = GetDate(xpath, "OrderDate");

            // customer id
            long customerId = XPathUtility.Evaluate(xpath, "CustomerID", -1);
            if (customerId == -1)
            {
                order.OnlineCustomerID = null;
            }
            else
            {
                order.OnlineCustomerID = customerId;
            }

            order.OnlineStatus = XPathUtility.Evaluate(xpath, "OrderStatus", "");

            order.RequestedShipping = shippingMethods.GetShippingMethods(XPathUtility.Evaluate(xpath, "ShippingMethodID", -1));

            // shipping/billing address
            LoadAddressInfo(order, xpath);

            // do the remaining only on new orders
            if (order.IsNew)
            {
                await LoadNotes(order, xpath).ConfigureAwait(false);

                // Items
                XPathNodeIterator itemNodes = xpath.Select("OrderDetails");
                while (itemNodes.MoveNext())
                {
                    LoadItem(order, itemNodes.Current);
                }

                // Load all of the charges
                LoadOrderCharges(order, xpath);

                // load the payment details
                LoadPaymentDetails(order, xpath);

                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // save it
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "VolusionDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads Payment detail
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, XPathNavigator xpath)
        {
            int paymentMethodId = XPathUtility.Evaluate(xpath, "PaymentMethodID", 0);

            VolusionPaymentMethod paymentMethod = paymentMethods.GetPaymentMethod(paymentMethodId);
            if (paymentMethod == null)
            {
                LoadPaymentDetail(order, "Payment Type", "Unknown");
            }
            else
            {
                if (String.Compare(paymentMethod.PaymentType, "Credit Card", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    LoadPaymentDetail(order, "Payment Type", paymentMethod.PaymentType);

                    // add details for the credit card
                    LoadPaymentDetail(order, "Card Owner", XPathUtility.Evaluate(xpath, "CardHoldersName", ""));
                    LoadPaymentDetail(order, "Card Expires", XPathUtility.Evaluate(xpath, "CreditCardExpDate", ""));
                    LoadPaymentDetail(order, "Card Type", paymentMethod.Name);
                }
                else
                {
                    LoadPaymentDetail(order, "Payment Type", paymentMethod.PaymentType);
                }
            }

            // add check number if it's there
            string checkNumber = XPathUtility.Evaluate(xpath, "CheckNumber", "");
            if (checkNumber.Length > 0)
            {
                LoadPaymentDetail(order, "Check Number", checkNumber);
            }
        }

        /// <summary>
        /// Creates a Payment Detail record for the order
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string detailValue)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);
            detail.Label = label;
            detail.Value = detailValue;
        }

        /// <summary>
        /// Load order charges
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, XPathNavigator xpath)
        {
            // Tax1, Tax2, Tax3
            for (int x = 1; x <= 3; x++)
            {
                // get the amount
                decimal taxTotal = XPathUtility.Evaluate(xpath, "SalesTax" + x.ToString(), 0M);
                string taxTitle = XPathUtility.Evaluate(xpath, String.Format("Tax{0}_Title", x), "");

                if (taxTotal > 0M)
                {
                    // create an order charge for it
                    OrderChargeEntity charge = InstantiateOrderCharge(order);

                    charge.Type = taxTitle.ToUpper(CultureInfo.InvariantCulture);
                    charge.Description = taxTitle;
                    charge.Amount = taxTotal;
                }
            }

            // shipping
            decimal shippingTotal = XPathUtility.Evaluate(xpath, "TotalShippingCost", 0M);
            if (shippingTotal > 0M)
            {
                OrderChargeEntity charge = InstantiateOrderCharge(order);
                charge.Type = "SHIPPING";
                charge.Description = "Shipping";
                charge.Amount = shippingTotal;
            }
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        private void LoadItem(OrderEntity order, XPathNavigator xpath)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.Code = XPathUtility.Evaluate(xpath, "ProductCode", "");
            item.SKU = item.Code;
            item.Name = XPathUtility.Evaluate(xpath, "ProductName", "");
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.Weight = XPathUtility.Evaluate(xpath, "ProductWeight", (double) 0.0);

            // make sure we're getting a quantity back before dividing for unit price
            item.UnitPrice = XPathUtility.Evaluate(xpath, "ProductPrice", 0.0M);

            // parse out the options
            string optionsString = XPathUtility.Evaluate(xpath, "Options", "");
            LoadOptions(item, optionsString);

            OrderItemAttributeEntity option = null;

            // handle order item gift wrapping
            bool isGift = XPathUtility.Evaluate(xpath, "GiftWrap", "N") == "Y";
            if (isGift)
            {
                decimal giftWrapCost = XPathUtility.Evaluate(xpath, "GiftWrapCost", 0.0M);

                option = InstantiateOrderItemAttribute(item);
                option.Name = "Gift Wrap";
                option.Description = "Yes";
                option.UnitPrice = giftWrapCost;
            }

            // need to add order item attributes describing the gift wrapping
            string giftWrapNote = XPathUtility.Evaluate(xpath, "GiftWrapNote", "");
            if (giftWrapNote.Length > 0)
            {
                option = InstantiateOrderItemAttribute(item);
                option.Name = "Gift Wrap Note";
                option.Description = giftWrapNote;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Order Item Options
        /// </summary>
        private void LoadOptions(OrderItemEntity item, string optionsString)
        {
            Regex optionRegex = new Regex(@"\[+(?<name>[^\]]*)\:(?<value>[^\]]*)");
            MatchCollection matches = optionRegex.Matches(optionsString);
            foreach (Match match in matches)
            {
                string optionName = match.Groups["name"].Value;
                string optionValue = match.Groups["value"].Value;

                OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

                option.Name = optionName;
                option.Description = optionValue;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Loads notes on the order
        /// </summary>
        private async Task LoadNotes(OrderEntity order, XPathNavigator xpath)
        {
            bool isGift = XPathUtility.Evaluate(xpath, "IsAGift", "N") == "Y";
            if (isGift)
            {
                await InstantiateNote(order, "Gift: Yes", order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            // order comments
            string orderComments = XPathUtility.Evaluate(xpath, "Order_Comments", "");
            if (orderComments.Length > 0)
            {
                await InstantiateNote(order, orderComments, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            // order notes (private)
            string orderNotes = XPathUtility.Evaluate(xpath, "OrderNotes", "");
            if (orderNotes.Length > 0)
            {
                await InstantiateNote(order, orderNotes, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Looks up a customer's email address
        /// </summary>
        private string GetCustomerEmail(object customerId)
        {
            if (customerId == null)
            {
                return "";
            }

            try
            {
                IXPathNavigable response = webClient.GetCustomer(Store as IVolusionStoreEntity, (long) customerId);

                return XPathUtility.Evaluate(response.CreateNavigator(), "//EmailAddress", "");
            }
            catch (VolusionException)
            {
                return "";
            }
        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, XPathNavigator xpath)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // Fill in the address info
            LoadAddressInfo(shipAdapter, xpath, "Ship");
            LoadAddressInfo(billAdapter, xpath, "Billing");

            billAdapter.Email = GetCustomerEmail(order.OnlineCustomerID);

            // fix bad/missing shipping information, take from the customer record
            if (shipAdapter.FirstName.Length == 0 && shipAdapter.LastName.Length == 0 && shipAdapter.City.Length == 0)
            {
                PersonAdapter.Copy(billAdapter, shipAdapter);
            }
            else
            {
                // if the basic person details look the same, copy the billing email address to shipping
                if (shipAdapter.FirstName == billAdapter.FirstName &&
                    shipAdapter.LastName == billAdapter.LastName &&
                    shipAdapter.Street1 == billAdapter.Street1 &&
                    shipAdapter.PostalCode == billAdapter.PostalCode)
                {
                    shipAdapter.Email = billAdapter.Email;
                }
            }
        }

        /// <summary>
        /// Loads address information from xml to the person adapter
        /// </summary>
        private void LoadAddressInfo(PersonAdapter adapter, XPathNavigator xpath, string prefix)
        {
            adapter.NameParseStatus = PersonNameParseStatus.Simple;
            adapter.Company = XPathUtility.Evaluate(xpath, prefix + "CompanyName", "");
            adapter.FirstName = XPathUtility.Evaluate(xpath, prefix + "FirstName", "");
            adapter.LastName = XPathUtility.Evaluate(xpath, prefix + "LastName", "");
            adapter.Street1 = XPathUtility.Evaluate(xpath, prefix + "Address1", "");
            adapter.Street2 = XPathUtility.Evaluate(xpath, prefix + "Address2", "");
            adapter.City = XPathUtility.Evaluate(xpath, prefix + "City", "");
            adapter.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, prefix + "State", ""));
            adapter.PostalCode = XPathUtility.Evaluate(xpath, prefix + "PostalCode", "");
            adapter.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, prefix + "Country", ""));
            adapter.Phone = XPathUtility.Evaluate(xpath, prefix + "PhoneNumber", "");
            adapter.Fax = XPathUtility.Evaluate(xpath, prefix + "FaxNumber", "");
        }

        /// <summary>
        /// Converts a date from server local time to UTC
        /// </summary>
        private DateTime GetDate(XPathNavigator xpath, string node)
        {
            VolusionStoreEntity volusionStore = (VolusionStoreEntity) Store;

            string date = XPathUtility.Evaluate(xpath, node, "");

            try
            {
                DateTime serverTime = DateTime.Parse(date);

                try
                {
                    TimeZoneInfo serverTz = TimeZoneInfo.FindSystemTimeZoneById(volusionStore.ServerTimeZone);
                    return TimeZoneInfo.ConvertTimeToUtc(serverTime, serverTz);
                }
                catch (Exception ex) when (ex is InvalidTimeZoneException || ex is TimeZoneNotFoundException || ex is ArgumentException)
                {
                    // just convert directly to UTC
                    return serverTime.ToUniversalTime();
                }
            }
            catch (FormatException)
            {
                throw new VolusionException($"Failed to convert Order Date '{date}' to DateTime.");
            }
        }

        /// <summary>
        /// Returns the number of orders that were downloaded in xml
        /// </summary>
        private int GetOrderCount(XPathNavigator xpath)
        {
            return XPathUtility.Evaluate(xpath, "count(//Orders)", 0);
        }
    }
}