using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using log4net;
using Rebex.Mail;
using Rebex.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Downloader for Yahoo! stores
    /// </summary>
    public class YahooEmailDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooEmailDownloader));

        // Regex for extracting the image url
        static Regex thumbRegex = new Regex(
            "src[ ]*=[ ]*('|\")?(?<url>[^ >]*)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        int messageCount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailDownloader(YahooStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Initiate the download
        /// </summary>
        protected override void Download()
        {
            try
            {
                EmailAccountEntity emailAccount = EmailAccountManager.GetAccount(((YahooStoreEntity)Store).YahooEmailAccountID);
                if (emailAccount == null)
                {
                    throw new DownloadException("The email account configured for downloading has been deleted.");
                }

                Progress.Detail = "Logging in to email account...";

                // Logon to the email account
                using (Pop3 popClient = EmailUtility.LogonToPop3(emailAccount))
                {
                    messageCount = popClient.GetMessageCount();

                    if (messageCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    // Get all the messages we know about
                    for (int i = 1; i <= messageCount; i++)
                    {
                        // Check if it has been cancelled
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
            catch (WebException ex)
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
        /// Download and process the mail message given by the specified sequence number
        /// </summary>
        private void DownloadMailMessage(Pop3 popClient, int sequenceNumber)
        {
            Progress.Detail = string.Format("Processing message {0} of {1}...", sequenceNumber, messageCount);

            MailMessage message = popClient.GetMailMessage(sequenceNumber);

            // Log the message
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Yahoo, "Message");
            logEntry.LogRequest(message);

            string xmlContent = GetMessageXmlContent(message);

            if (xmlContent != null)
            {
                xmlContent = XmlUtility.StripInvalidXmlCharacters(xmlContent);

                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlContent);

                    LoadOrders(xmlDocument);
                }
                catch (XmlException ex)
                {
                    log.WarnFormat("Could not load XML content from email message. Subject '{0}', Error: {1}", message.Subject, ex.Message);
                }
            }
            else
            {
                log.WarnFormat("Message found that does not appear to contain Yahoo! order xml. Subject '{0}'", message.Subject);
            }

            // Remove the message so we don't download again next time
            if (YahooEmailUtility.DeleteMessagesAfterDownload)
            {
                popClient.Delete(sequenceNumber);
            }

            // Update progress
            Progress.PercentComplete = 100 * sequenceNumber / messageCount;
        }

        /// <summary>
        /// Extract the xml message content string from the message
        /// </summary>
        private string GetMessageXmlContent(MailMessage message)
        {
            // Find the first text/xml attachment.  There should only be one anyway.
            Attachment xmlAttachment = message.Attachments.FirstOrDefault(a => a.MediaType == "text/xml" || a.MediaType == "application/xml");

            // If we found that, use it.
            if (xmlAttachment != null)
            {
                return xmlAttachment.ContentString;
            }

            // Next check alternate views
            AlternateView xmlView = message.AlternateViews.FirstOrDefault(v => v.MediaType == "text/xml" || v.MediaType == "application/xml");

            // If we found it, use it.
            if (xmlView != null)
            {
                return xmlView.ContentString;
            }
            
            // Didn't find it - non ShipWorks message must be assumed
            return null;
        }

        /// <summary>
        /// Load orders from the given Yahoo! xml content
        /// </summary>
        private void LoadOrders(XmlDocument xmlDocument)
        {
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // Go through each order in the XML Document
            foreach (XPathNavigator order in xpath.Select("//Order"))
            {
                LoadOrder(order);
            }
        }

        /// <summary>
        /// Extract the order from the XML
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadOrder(XPathNavigator xpath)
        {
            // Get the OrderID
            string yahooOrderID = xpath.GetAttribute("id", "");

            // Now extract the Order#
            int lastSlash = yahooOrderID.LastIndexOf("-");
            int orderNumber = Convert.ToInt32(yahooOrderID.Substring(lastSlash + 1));

            // Create a new order for it
            YahooOrderEntity order = (YahooOrderEntity) InstantiateOrder(new YahooOrderIdentifier(yahooOrderID));

            long numericTime = XPathUtility.Evaluate(xpath, "NumericTime", 0L);

            // Setup the basic proprites
            order.OrderNumber = orderNumber;
            order.OrderDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(numericTime);
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "Shipping", "");

            // Fill in the address info
            LoadAddressInfo(new PersonAdapter(order, "Ship"), xpath.SelectSingleNode("AddressInfo[@type='ship']"));
            LoadAddressInfo(new PersonAdapter(order, "Bill"), xpath.SelectSingleNode("AddressInfo[@type='bill']"));

            // Yahoo! doesnt automatically fill in the ShipTo email even if ShipTo\BillTo are the same
            UpdateShipToEmail(order);

            if (order.IsNew)
            {
                // Add the customer comments to the notes field
                string comments = XPathUtility.Evaluate(xpath, "Comments", "");
                if (comments.Length > 0)
                {
                    InstantiateNote(order, comments, order.OrderDate, NoteVisibility.Public);
                }

                // Items
                foreach (XPathNavigator itemNode in xpath.Select("Item"))
                {
                    LoadItem(order, itemNode);
                }

                // Manually create the GiftWrap item
                bool giftWrap = xpath.Select("GiftWrap").Count != 0;
                if (giftWrap || XPathUtility.Evaluate(xpath, "GiftWrapMessage", "") != "")
                {
                    // Create the gift wrap line item.  The amount of the gift wrap
                    // will be set later when we look through the charges.
                    CreateGiftWrapItem(order, XPathUtility.Evaluate(xpath, "GiftWrapMessage", ""));
                }

                // Totals (charges)
                foreach (XPathNavigator totalsNode in xpath.Select("Total/Line"))
                {
                    LoadCharge(order, totalsNode);
                }

                // Payment details
                XPathNodeIterator cardAuthNodes = xpath.Select("CardEvents/CardAuth");
                if (cardAuthNodes.MoveNext())
                {
                    XPathNavigator authNode = cardAuthNodes.Current.Clone();

                    bool attributes = authNode.MoveToFirstAttribute();

                    while (attributes)
                    {
                        LoadPaymentDetail(order, authNode.LocalName, authNode.Value);

                        attributes = authNode.MoveToNextAttribute();
                    }
                }

                // Payment details (continued)
                bool creditCard = xpath.Select("CreditCard").Count != 0;
                if (creditCard)
                {
                    LoadPaymentDetail(order, "Card Expiration", XPathUtility.Evaluate(xpath, "CreditCard/@expiration", ""));
                    LoadPaymentDetail(order, "Card Type", XPathUtility.Evaluate(xpath, "CreditCard/@type", ""));
                }
            }

            // Save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "YahooEmailDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load the payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(YahooOrderEntity order, string name, string data)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;
        }

        /// <summary>
        /// Load the charge for the order
        /// </summary>
        private void LoadCharge(YahooOrderEntity order, XPathNavigator xpath)
        {
            string type = xpath.GetAttribute("type", "");
            string name = xpath.GetAttribute("name", "");
            decimal amount = Convert.ToDecimal(xpath.Value);

            if (type == "Total")
            {
                order.OrderTotal = amount;
                return;
            }

            if (type == "Subtotal")
            {
                return;
            }

            if (type == "GiftWrap")
            {
                if (SetGiftWrapAmount(order, amount))
                {
                    return;
                }
            }

            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type.ToUpper();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Set the amount of the gift wrap line item
        /// </summary>
        private bool SetGiftWrapAmount(YahooOrderEntity order, decimal amount)
        {
            OrderItemEntity giftWrapItem = order.OrderItems.FirstOrDefault(i => i.Code == "GIFTWRAP");
            if (giftWrapItem != null)
            {
                giftWrapItem.UnitPrice = amount;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create a gift wrap line item with the specified message
        /// </summary>
        private void CreateGiftWrapItem(YahooOrderEntity order, string message)
        {
            YahooOrderItemEntity item = (YahooOrderItemEntity) InstantiateOrderItem(order);

            item.YahooProductID = "giftwrap";
            item.Code = "GIFTWRAP";
            item.Name = "Gift Wrap";
            item.Quantity = 1;
            item.UnitPrice = 0;

            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);

            attribute.Name = "Message";
            attribute.Description = message;
            attribute.UnitPrice = 0;
        }

        /// <summary>
        /// Changes html encoded quotes to regular quotes.
        /// </summary>
        private string QuoteClean(string storeText)
        {
            return storeText.Replace("&quot;", "\"");
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(YahooOrderEntity order, XPathNavigator xpath)
        {
            YahooOrderItemEntity item = (YahooOrderItemEntity) InstantiateOrderItem(order);

            item.YahooProductID = XPathUtility.Evaluate(xpath, "Id", "");
            item.Code = QuoteClean(XPathUtility.Evaluate(xpath, "Code", ""));
            item.Name = QuoteClean(XPathUtility.Evaluate(xpath, "Description", ""));
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "Unit-Price", (decimal) 0.0);
            item.Weight = YahooEmailProductManager.GetItemWeight((YahooStoreEntity) Store, item.YahooProductID);

            // Extract the image, if available
            string thumb = XPathUtility.Evaluate(xpath, "Thumb", "");
            if (thumb != "")
            {
                Match match = thumbRegex.Match(thumb);
                item.Thumbnail = match.Groups["url"].Value;
                item.Image = match.Groups["url"].Value;
            }

            // Now load all the item options
            foreach (XPathNavigator attributeNode in xpath.Select("Option"))
            {
                LoadOption(item, attributeNode);
            }
        }

        /// <summary>
        /// Changes html encoded quotes to regular quotes.
        /// </summary>
        private string QuoteReplace(string text)
        {
            return text.Replace("&quot;", "\"");
        }

        /// <summary>
        /// Load the option of the given item
        /// </summary>
        private void LoadOption(YahooOrderItemEntity item, XPathNavigator xpath)
        {
            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);

            attribute.Name = xpath.GetAttribute("name", "");
            attribute.Description = xpath.Value;
            attribute.UnitPrice = 0;
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private void LoadAddressInfo(PersonAdapter person, XPathNavigator xpath)
        {
            person.NameParseStatus = PersonNameParseStatus.Simple;
            person.FirstName = XPathUtility.Evaluate(xpath, "Name/First", "");
            person.LastName = XPathUtility.Evaluate(xpath, "Name/Last", "");
            person.Company = XPathUtility.Evaluate(xpath, "Company", "");
            person.Street1 = XPathUtility.Evaluate(xpath, "Address1", "");
            person.Street2 = XPathUtility.Evaluate(xpath, "Address2", "");
            person.City = XPathUtility.Evaluate(xpath, "City", "");
            person.StateProvCode = XPathUtility.Evaluate(xpath, "State", "");
            person.PostalCode = XPathUtility.Evaluate(xpath, "Zip", "");
            person.CountryCode = XPathUtility.Evaluate(xpath, "Country", "");
            person.Phone = XPathUtility.Evaluate(xpath, "Phone", "");
            person.Email = XPathUtility.Evaluate(xpath, "Email", "");

            // Sometimes this looks like "US United States"
            if (person.CountryCode.Length > 2)
            {
                person.CountryCode = person.CountryCode.Substring(0, 2);
            }
        }

        /// <summary>
        /// Since Yahoo! does not automatically fill in the ShipTo email when the ShipTo\BillTo
        /// are the same, we manually do it here.
        /// </summary>
        private void UpdateShipToEmail(YahooOrderEntity order)
        {
            // If its the same ShipTo\BillTo, fill in the ShipTo email address
            if (order.ShipFirstName == order.BillFirstName &&
                order.ShipLastName == order.BillLastName &&
                order.ShipStreet1 == order.BillStreet1 &&
                order.ShipPostalCode == order.BillPostalCode)
            {
                order.ShipEmail = order.BillEmail;
            }
        }
    }
}
