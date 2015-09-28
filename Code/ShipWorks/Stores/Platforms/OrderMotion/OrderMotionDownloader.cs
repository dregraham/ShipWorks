using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using Rebex.Net;
using ShipWorks.Data.Connection;
using ShipWorks.Email;
using ShipWorks.ApplicationCore.Logging;
using Rebex.Mail;
using System.Xml;
using Interapptive.Shared.IO.Text.Csv;
using System.IO;
using ShipWorks.Stores.Content;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business.Geography;
using Rebex.Mime.Headers;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Order downloader for OrderMotion stores
    /// </summary>
    public class OrderMotionDownloader : StoreDownloader
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(OrderMotionDownloader));

        // number of email messages to be downloaded
        int messageCount;

        // Cache of OrderInformationReponses from OrderMotion
        Dictionary<long, IXPathNavigable> orderResponseCache = new Dictionary<long, IXPathNavigable>();

        // cached Items
        Dictionary<string, bool> loadedItems = new Dictionary<string, bool>();

        // cached item attribute names
        Dictionary<string, string> itemAttributes = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionDownloader(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Start the download process
        /// </summary>
        protected override void Download()
        {
            try
            {
                EmailAccountEntity emailAccount = EmailAccountManager.GetAccount(((OrderMotionStoreEntity)Store).OrderMotionEmailAccountID);
                if (emailAccount == null)
                {
                    throw new DownloadException("The email account configured for downloading has been deleted.");
                }

                Progress.Detail = "Logging in to email account...";

                using (Pop3 popClient = EmailUtility.LogonToPop3(emailAccount))
                {
                    messageCount = popClient.GetMessageCount();

                    if (messageCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    // get all the messages we know about
                    for (int i = 1; i <= messageCount; i++)
                    {
                        // check for cancel
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        DownloadMailMessage(popClient, i);
                    }

                    // Must be called to finalize delete
                    popClient.Disconnect();
                }
            }
            catch (OrderMotionException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (EmailLogonException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (Pop3Exception ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (TlsException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Downloads an email message from the pop server and processes it for orders.
        /// </summary>
        private void DownloadMailMessage(Pop3 popClient, int sequenceNumber)
        {
            Progress.Detail = string.Format("Processing message {0} of {1}...", sequenceNumber, messageCount);

            MailMessage message = popClient.GetMailMessage(sequenceNumber);

            // Log the message
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.OrderMotion, "Message");
            logEntry.LogRequest(message);

            // OrderMotion order emails come with CSV attached
            if (message.Attachments.Count == 0)
            {
                log.WarnFormat("Message found that does not appear to contain OrderMotion order xml. Subject '{0}'", message.Subject);
            }
            else
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    // don't even try to process anything with a mime type of application/*  like application/pdf.
                    if (attachment.MediaType != null && attachment.MediaType.IndexOf("application", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        log.InfoFormat("Skipping the processing of OrderMotion attachment named {0} because it has a MIME type of {1}.", attachment.FileName, attachment.MediaType);
                        continue;
                    }

                    try
                    {
                        LoadOrders(attachment);
                    }
                    catch (MalformedCsvException ex)
                    {
                        // log
                        throw new OrderMotionException("The OrderMotion email contained corrupt or invalid CSV data.  Please check your email account and delete the oldest message.", ex);
                    }
                }
            }

            // Remove the message so we don't download again next time
            if (OrderMotionUtility.DeleteMessagesAfterDownload)
            {
                popClient.Delete(sequenceNumber);
            }

            // Update progress
            Progress.PercentComplete = 100 * sequenceNumber / messageCount;
        }

        /// <summary>
        /// Processes an email attachment for OrderMotion orders
        /// </summary>
        private void LoadOrders(Attachment attachment)
        {
            string attachmentBody = attachment.ContentString;
            if (attachmentBody == null)
            {
                // Excel spreadsheets and the like don't get put into ContentSting
                using (StreamReader reader = new StreamReader(attachment.GetContentStream()))
                {
                    attachmentBody = reader.ReadToEnd();
                }

                if (String.IsNullOrEmpty(attachmentBody))
                {
                    log.Warn("Attachment is not appear to have OrderMotion content.");
                    return;
                }
            }

            // malformedcsvexception
            using (StringReader stringReader = new StringReader(attachmentBody))
            {
                using (CsvReader reader = new CsvReader(stringReader, true))
                {
                    // OrderMotion's CSVs are poorly formatted
                    reader.MissingFieldAction = MissingFieldAction.ReplaceByEmpty;
                    reader.DuplicateFieldAction = DuplicateFieldAction.Ignore;

                    // cycle through the records
                    while (reader.ReadNextRecord())
                    {
                        // load the order information ffrom the current record
                        LoadOrder(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Load the order from the current CSV record
        /// </summary>
        private void LoadOrder(CsvReader reader)
        {
            try
            {
                string invoiceId = reader["INVOICE_NO"];
                int slash = invoiceId.IndexOf("-");

                // get the parts out we need. "ordernumber-shipmentid"
                int orderNumber = Convert.ToInt32(invoiceId.Substring(0, slash));
                int shipmentId = Convert.ToInt32(invoiceId.Substring(slash + 1));

                OrderMotionOrderIdentifier identifier = new OrderMotionOrderIdentifier(orderNumber, shipmentId);
                OrderMotionOrderEntity order = (OrderMotionOrderEntity)InstantiateOrder(identifier);

                // set the order postfix
                if (shipmentId > 1)
                {
                    order.ApplyOrderNumberPostfix(String.Format("-{0}", shipmentId));
                }

                XPathNavigator xpath = GetOrderMotionOrder(order.OrderNumber);

                DateTime orderDate = GetOrderDate(reader, xpath);

                order.OrderDate = orderDate;

                // requested shipping formatting
                LoadShippingMethod(order, xpath);

                // promotion code
                order.OrderMotionPromotion = XPathUtility.Evaluate(xpath, "/OrderInformationResponse/OrderHeader/Promotion", "");

                // The order motion invoice ID
                order.OrderMotionInvoiceNumber = invoiceId;

                // no customer ID
                int customerID = XPathUtility.Evaluate(xpath, "/OrderInformationResponse/Customer/@number", 0);
                if (customerID > 0)
                {
                    order.OnlineCustomerID = customerID;
                }
                else
                {
                    order.OnlineCustomerID = null;
                }

                // address
                LoadAddressInfo(order, xpath);

                if (order.IsNew)
                {
                    // notes
                    LoadNotes(order, xpath);

                    // order items
                    LoadItems(order, xpath);
                   
                    // order charges
                    LoadCharges(order, xpath);

                    // payment details
                    LoadPaymentDetails(order, xpath);

                    // calculate the total
                    order.OrderTotal = OrderUtility.CalculateTotal(order);
                }

                // save the order
                SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "OrderMotion.LoadOrder");
                retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
            }
            catch (ArgumentException ex)
            {
                // Field wasn't found, skip it
                log.Warn(ex);
            }
        }

        /// <summary>
        /// Loads payment information from the order
        /// </summary>
        private void LoadPaymentDetails(OrderMotionOrderEntity order, XPathNavigator xpath)
        {
            XPathNodeIterator paymentIterator = xpath.Select("//Payment");
            if (paymentIterator.MoveNext())
            {
                XPathNavigator paymentXPath = paymentIterator.Current.Clone();

                // get the payment type
                LoadPaymentDetail(order, "Payment Type", XPathUtility.Evaluate(paymentXPath, "@description", ""));

                // see if there's credit card information
                int cardType = XPathUtility.Evaluate(paymentXPath, "CreditCardType", 0);
                if (cardType > 0)
                {
                    LoadPaymentDetail(order, "Card Owner", XPathUtility.Evaluate(paymentXPath, "CreditCardName", ""));
                    LoadPaymentDetail(order, "Card Expires", XPathUtility.Evaluate(paymentXPath, "CreditCardExpDate", ""));
                    LoadPaymentDetail(order, "Card Number", XPathUtility.Evaluate(paymentXPath, "CreditCardNumber", ""));
                    LoadPaymentDetail(order, "CCV", XPathUtility.Evaluate(paymentXPath, "CreditCardCCV", ""));
                }
            }
        }

        /// <summary>
        /// Load an individual PaymentDetail record
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string name, string data)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;

            PaymentDetailSecurity.Protect(detail);
        }


        /// <summary>
        /// Loads order charges from the order
        /// </summary>
        private void LoadCharges(OrderMotionOrderEntity order, XPathNavigator xpath)
        {
            // only put OrderCharges on the orders for shipmentid 1
            if (order.OrderMotionShipmentID == 1)
            {
                // Tax
                OrderChargeEntity charge = InstantiateOrderCharge(order);
                charge.Type = "TAX";
                charge.Description = "Tax";
                charge.Amount = XPathUtility.Evaluate(xpath, @"sum(//LineItem/Tax)", 0.0M);

                // Shipping 
                charge = InstantiateOrderCharge(order);
                charge.Type = "SHIPPING";
                charge.Description = "Shipping";
                charge.Amount = XPathUtility.Evaluate(xpath, @"//ShippingInformation/ShippingAmount", 0.0M);

                decimal handlingAmount = XPathUtility.Evaluate(xpath, @"//ShippingInformation/HandlingAmount", 0.0M);
                if (handlingAmount > 0.0M)
                {
                    charge = InstantiateOrderCharge(order);
                    charge.Type = "HANDLING";
                    charge.Description = "Handling";
                    charge.Amount = handlingAmount;
                }
            }
        }

        /// <summary>
        /// Load order items from the order
        /// </summary>
        private void LoadItems(OrderMotionOrderEntity order, XPathNavigator xpath)
        {
            XPathNodeIterator itemIterator = xpath.Select("//LineItem");
            while (itemIterator.MoveNext())
            {
                XPathNavigator itemNavigator = itemIterator.Current.Clone();

                // Create the order item
                OrderItemEntity item = InstantiateOrderItem(order);

                item.Code = XPathUtility.Evaluate(itemNavigator, "ItemCode", "");
                item.SKU = item.Code;
                item.Name = XPathUtility.Evaluate(itemNavigator, "ProductName", "");
                item.Quantity = XPathUtility.Evaluate(itemNavigator, "Quantity", 0);
                item.UnitPrice = XPathUtility.Evaluate(itemNavigator, "Price", 0.0M);
                item.LocalStatus = GetItemStatus(XPathUtility.Evaluate(itemNavigator, "LineStatus", ""));

                // weight is a total weight.  need to divide by quanitty
                double weight = XPathUtility.Evaluate(itemNavigator, "DimensionData/Weight", 0.0);
                if (item.Quantity > 0)
                {
                    item.Weight = weight / item.Quantity;
                }
                else
                {
                    item.Weight = weight;
                }

                LoadOptions(item, itemNavigator);
            }
        }

        /// <summary>
        /// Loads options/attributes for the specified item
        /// </summary>
        private void LoadOptions(OrderItemEntity item, XPathNavigator xpath)
        {
            XPathNodeIterator attributeIterator = xpath.Select("ItemAttributes/AttributeValue");
            while (attributeIterator.MoveNext())
            {
                XPathNavigator attribXPath = attributeIterator.Current.Clone();
                if (attribXPath.Value != null)
                {
                    // there's a value here, determine which attribute it is
                    int attributeID = XPathUtility.Evaluate(attribXPath, "@attributeID", -1);
                    if (attributeID == -1)
                    {
                        continue;
                    }

                    string attributeName = GetItemAttributeName(item.Code, attributeID);
                    if (attributeName != null && attributeName.Length > 0)
                    {
                        // create an option
                        OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

                        option.Name = attributeName;
                        option.Description = attribXPath.Value;
                        option.UnitPrice = 0M;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a ShipWorks status string from OrderMotion Status Code
        /// </summary>
        private string GetItemStatus(string omStatusCode)
        {
            int intCode = -1;
            string status = "";
            try
            {
                intCode = int.Parse(omStatusCode);
            }
            catch (FormatException)
            {
                // ignore parsing problems, OM sent invalid data
            }

            // Note: these status codes were provided by "Freddy" at OrderMotion.
            // See FB Case 41811
            switch (intCode)
            {
                case 0:
                    status = "Pending";
                    break;
                case 2:
                    status = "Clearing";
                    break;
                case 3:
                case 20:
                    status = "Backordered";
                    break;
                case 4:
                    status = "Review";
                    break;
                case 5:
                    status = "On Hold";
                    break;
                case 6:
                    status = "In Progress";
                    break;
                case 7:
                    status = "Waiting";
                    break;
                case 10:
                    status = "Released";
                    break;
                case 30:
                    status = "Warehouse";
                    break;
                case 40:
                    status = "Shipped";
                    break;
                case 50:
                    status = "Returned";
                    break;
                case 52:
                    status = "Pending Returned";
                    break;
                case 60:
                    status = "Refunded";
                    break;
                case 90:
                    status = "Cancelled";
                    break;
                case 95:
                    status = "Auto-Cancelled";
                    break;
                default:
                    status = omStatusCode;
                    break;
            }

            return status;
        }

        /// <summary>
        /// Loads Shipping and Billing address into the order entity
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, XPathNavigator xpath)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // if the shipping address says to not use the shipping address, must use the billing address
            int useShipTo = XPathUtility.Evaluate(xpath, @"/OrderInformationResponse/ShippingInformation/Address[@type='ShipTo']/UseShipTo", 1);
            string shipAddressXPath = @"/OrderInformationResponse/ShippingInformation/Address[@type='ShipTo']";
            if (useShipTo == 0)
            {
                shipAddressXPath = @"/OrderInformationResponse/Customer/Address[@type='BillTo']";
            }

            XPathNodeIterator shipAddressIterator = xpath.Select(shipAddressXPath);
            if (shipAddressIterator.MoveNext())
            {
                LoadAddressInfo(shipAdapter, shipAddressIterator.Current.Clone());
            }

            // Billing addrss
            XPathNodeIterator billingIterator = xpath.Select(@"/OrderInformationResponse/Customer/Address[@type='BillTo']");
            if (billingIterator.MoveNext())
            {
                LoadAddressInfo(billAdapter, billingIterator.Current.Clone());
            }
        }

        /// <summary>
        /// Copies address information from the provided xpathnavigator to the personadapter
        /// </summary>
        private void LoadAddressInfo(PersonAdapter adapter, XPathNavigator xpath)
        {
            adapter.NameParseStatus = PersonNameParseStatus.Simple;
            adapter.FirstName = XPathUtility.Evaluate(xpath, "Firstname", "");
            adapter.LastName = XPathUtility.Evaluate(xpath, "Lastname", "");
            adapter.Company = XPathUtility.Evaluate(xpath, "Company", "");
            adapter.Street1 = XPathUtility.Evaluate(xpath, "Address1", "");
            adapter.Street2 = XPathUtility.Evaluate(xpath, "Address2", "");
            adapter.City = XPathUtility.Evaluate(xpath, "City", "");
            adapter.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "State", ""));
            adapter.PostalCode = XPathUtility.Evaluate(xpath, "ZIP", "");
            adapter.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, "Country", ""));

            if (adapter.CountryCode == "USA")
            {
                adapter.CountryCode = "US";
            }

            adapter.Email = XPathUtility.Evaluate(xpath, "Email", "");
            adapter.Phone = CleanPhone(XPathUtility.Evaluate(xpath, "FullPhone", ""));

            // Depending on if its pulling address information from the customer or shipping section, phone and country may take more work.
            if (string.IsNullOrWhiteSpace(adapter.Phone))
            {
                adapter.Phone = CleanPhone(XPathUtility.Evaluate(xpath, "PhoneNumber", ""));
            }

            if (string.IsNullOrWhiteSpace(adapter.CountryCode))
            {
                string tld = XPathUtility.Evaluate(xpath, "TLD", "");

                // If we can get a country name from the tld, we know its valid
                if (Geography.GetCountryName(tld) != tld)
                {
                    adapter.CountryCode = tld;
                }
            }

            if (string.IsNullOrWhiteSpace(adapter.CountryCode))
            {
                string fullAddress = XPathUtility.Evaluate(xpath, "FullAddress", "");
                string[] addressParts = fullAddress.Trim().Split(' ', '\r', '\n');

                if (addressParts.Length > 0)
                {
                    string countryPart = addressParts[addressParts.Length - 1];

                    if (countryPart == "USA")
                    {
                        countryPart = "US";
                    }

                    adapter.CountryCode = Geography.GetCountryCode(countryPart);
                }
            }
        }

        /// <summary>
        /// Cleans the OrderMotion phone number for use in ShipWorks (Fedex was having issues using the value coming from OrderMotion)
        /// </summary>
        static string CleanPhone(string phone)
        {
            return Regex.Replace(phone, @"[^\d\-]", "");
        }


        /// <summary>
        /// Returns the order date from the Api response.
        /// </summary>
        private static DateTime GetOrderDate(CsvReader reader, XPathNavigator xpath)
        {
            // order date, try defaulting to the value in the csv file
            string defaultDate;
            try
            {
                defaultDate = reader["Date_of_Sale"];
            }
            catch (ArgumentException)
            {
                defaultDate = DateTime.UtcNow.ToString();
            }
            catch (FormatException)
            {
                defaultDate = DateTime.UtcNow.ToString();
            }

            return DateTime.Parse(XPathUtility.Evaluate(xpath, "/OrderInformationResponse/OrderHeader/OrderDate", defaultDate));
        }

        private static void LoadShippingMethod(OrderMotionOrderEntity order, XPathNavigator xpath)
        {
            string shipping = XPathUtility.Evaluate(xpath, "/OrderInformationResponse/ShippingInformation/Method", "");
            string carrierName = XPathUtility.Evaluate(xpath, "/OrderInformationResponse/ShippingInformation/Method/@carrierName", "");
            if (carrierName.Length > 0)
            {
                shipping = String.Format("{0} {1}", carrierName, shipping);
            }
            order.RequestedShipping = shipping;
        }

        private void LoadNotes(OrderMotionOrderEntity order, XPathNavigator xpath)
        {
            // order notes
            string notes = XPathUtility.Evaluate(xpath, "/OrderInformationResponse/ShippingInformation/SpecialInstructions", "");
            if (notes.Length > 0)
            {
                InstantiateNote(order, notes, order.OrderDate, NoteVisibility.Public);
            }
        }

        /// <summary>
        /// Retrieves and temporarily caches the OrderMotion responsed ata for the order nubmer provided.
        /// </summary>
        private XPathNavigator GetOrderMotionOrder(long orderNumber)
        {
            // see if we have it in the cache first
            if (!orderResponseCache.ContainsKey(orderNumber))
            {
                // make a web service call to get the order information from OrderMotion
                OrderMotionWebClient client = new OrderMotionWebClient((OrderMotionStoreEntity)Store);

                // get the order xml and cache it
                orderResponseCache[orderNumber] = client.GetOrder(orderNumber);
            }

            // return the cached document
            return orderResponseCache[orderNumber].CreateNavigator();
        }

        #region OrderMotion ItemAttribute processing and caching
        /// <summary>
        /// Looks up the item attribute name based on Id for a given itemcode. Caches the result.
        /// </summary>
        private string GetItemAttributeName(string itemCode, int attributeID)
        {
            string attributeHashKey = GetAttributeHashKey(itemCode, attributeID);
            string attributeName = null;

            // first see if the desired itemCode has been loaded
            if (!loadedItems.ContainsKey(itemCode))
            {
                // load it
                CacheItemAttributes(itemCode);
            }

            // now look for the attributeID
            if (itemAttributes.ContainsKey(attributeHashKey))
            {
                attributeName = (string)itemAttributes[attributeHashKey];
            }

            return attributeName;
        }

        /// <summary>
        /// Caches the attributes for a specified itemCode
        /// </summary>
        private void CacheItemAttributes(string itemCode)
        {
            try
            {
                OrderMotionWebClient client = new OrderMotionWebClient((OrderMotionStoreEntity)Store);

                // get the detail information for this itemcode
                XPathNavigator xpath = client.GetItemInformation(itemCode).CreateNavigator();

                // cache that we've loaded this item - in the future we may store actual item data here
                loadedItems[itemCode] = true;

                // continue to load its attributres
                XPathNodeIterator attributeIterator = xpath.Select(@"//CustomItemAttribute/Attribute");
                while (attributeIterator.MoveNext())
                {
                    XPathNavigator attribXPath = attributeIterator.Current.Clone();

                    string name = attributeIterator.Current.Value;
                    int id = XPathUtility.Evaluate(attribXPath, "@attributeID", -1);

                    if (id != -1 && name != null && name.Length > 0)
                    {
                        string key = GetAttributeHashKey(itemCode, id);
                        itemAttributes[key] = name;
                    }
                }
            }
            catch (OrderMotionException ex)
            {
                log.ErrorFormat("Unable to cache item {0}: {1}", itemCode, ex.Message);
            }
        }

        /// <summary>
        /// Formats the itemAttributes hash table keys
        /// </summary>
        private string GetAttributeHashKey(string itemCode, int attributeID)
        {
            return string.Format("{0}_{1}", itemCode, attributeID);
        }

        #endregion
    }
}
