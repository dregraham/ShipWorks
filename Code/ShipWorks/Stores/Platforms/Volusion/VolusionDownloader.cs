using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using System.Text.RegularExpressions;
using log4net;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Order downloader for Volusion stores
    /// </summary>
    public class VolusionDownloader : StoreDownloader
    {
        // total number of orders to be imported 
        // shipping method map
        VolusionShippingMethods shippingMethods;

        // payment method map
        VolusionPaymentMethods paymentMethods;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Convenience property for quick access to the specific entity
        /// </summary>
        protected VolusionStoreEntity VolusionStoreEntity
        {
            get
            {
                return (VolusionStoreEntity)Store;
            }
        }

        /// <summary>
        /// Download orders from the store
        /// </summary>
        protected override void Download()
        {
            try
            { 
                // get the collection of currently chosen codes to be downloaded
                List<string> selectedStatuses = VolusionStoreEntity.DownloadOrderStatuses.Split(',').ToList();

                // Volusion requires an explicit value when querying for orders, cannot pass a range or list of statuses
                // download for each status
                foreach(string status in selectedStatuses)
                {
                    int quantitySaved = 0;
                    Progress.Detail = "Preparing to download orders...";
                    shippingMethods = new VolusionShippingMethods((VolusionStoreEntity)Store);
                    paymentMethods = new VolusionPaymentMethods((VolusionStoreEntity)Store);

                    Progress.Detail = string.Format("Downloading {0} Orders...", status);

                    VolusionWebClient client = new VolusionWebClient((VolusionStoreEntity)Store);

                    // get all the orders - volusion just gives them all. At once.
                    IXPathNavigable ordersResponse = client.GetOrders(status);

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
                        return;
                    }

                    XPathNavigator xpath = ordersResponse.CreateNavigator();

                    int totalCount = GetOrderCount(xpath);
                    if (totalCount == 0)
                    {
                        Progress.PercentComplete = 100;
                        Progress.Detail = "Done.";
                        return;
                    }

                    XPathNodeIterator orders = xpath.Select("//Orders");
                    while (orders.MoveNext())
                    {
                        Progress.Detail = String.Format("Processing order {0} of {1}...", quantitySaved + 1, totalCount);

                        // load each order
                        LoadOrder(client, orders.Current.Clone());

                        Progress.PercentComplete = Math.Min(100, 100 * (quantitySaved) / totalCount);

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
        private void LoadOrder(VolusionWebClient client, XPathNavigator xpath)
        {
            long orderNumber = XPathUtility.Evaluate(xpath, "OrderID", 0);

            // find an existing order in ShipWorks or create a new one
            OrderNumberIdentifier orderIdentifier = new OrderNumberIdentifier(orderNumber);
            OrderEntity order = InstantiateOrder(orderIdentifier);

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
            LoadAddressInfo(order, client, xpath);

            // do the remaining only on new orders
            if (order.IsNew)
            {
                LoadNotes(order, xpath);

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
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
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
            item.Name = XPathUtility.Evaluate(xpath, "ProductName", "");
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.Weight = XPathUtility.Evaluate(xpath, "ProductWeight", (double)0.0);

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

                OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item) ;

                option.Name = optionName;
                option.Description = optionValue;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Loads notes on the order
        /// </summary>
        private void LoadNotes(OrderEntity order, XPathNavigator xpath)
        {
            bool isGift = XPathUtility.Evaluate(xpath, "IsAGift", "N") == "Y";
            if (isGift)
            {
                InstantiateNote(order, "Gift: Yes", order.OrderDate, NoteVisibility.Public);
            }

            // order comments
            string orderComments = XPathUtility.Evaluate(xpath, "Order_Comments", "");
            if (orderComments.Length > 0)
            {
                InstantiateNote(order, orderComments, order.OrderDate, NoteVisibility.Public);
            }

            // order notes (private)
            string orderNotes = XPathUtility.Evaluate(xpath, "OrderNotes", "");
            if (orderNotes.Length > 0)
            {
                InstantiateNote(order, orderNotes, order.OrderDate, NoteVisibility.Internal);
            }
        }

        /// <summary>
        /// Looks up a customer's email address
        /// </summary>
        private string GetCustomerEmail(VolusionWebClient client, object customerId)
        {
            if (customerId == null)
            {
                return "";
            }

            try
            {
                IXPathNavigable response = client.GetCustomer((long)customerId);

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
        private void LoadAddressInfo(OrderEntity order, VolusionWebClient client, XPathNavigator xpath)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // Fill in the address info
            LoadAddressInfo(shipAdapter, xpath, "Ship");
            LoadAddressInfo(billAdapter, xpath, "Billing");

            billAdapter.Email = GetCustomerEmail(client, order.OnlineCustomerID);

            // fix bad/missing shipping information, take from teh customer record
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
            VolusionStoreEntity volusionStore = (VolusionStoreEntity)Store;

            string date = XPathUtility.Evaluate(xpath, node, "");

            DateTime serverTime = DateTime.Parse(date);
            //serverTime = DateTime.SpecifyKind(serverTime, DateTimeKind.un);

            try
            {
                TimeZoneInfo serverTz = TimeZoneInfo.FindSystemTimeZoneById(volusionStore.ServerTimeZone);
                return TimeZoneInfo.ConvertTimeToUtc(serverTime, serverTz);
            }
            catch (InvalidTimeZoneException)
            {
                // just convert directly to utc
                return serverTime.ToUniversalTime();
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
