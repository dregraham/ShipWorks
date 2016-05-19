using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using ShipWorks.ApplicationCore;
using System.Diagnostics;
using Interapptive.Shared.Security;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Web client for connecting to legacy MarketplaceAdvisor stores
    /// </summary>
    public class MarketplaceAdvisorLegacyClient : MarshalByRefObject
    {
        private readonly int accountType;
        private readonly string username;
        private readonly string password;

        private MarketplaceAdvisorLog log;

        // Cache of inventory items we have downloaded
        private static readonly Dictionary<long, MarketplaceAdvisorInventoryItem> inventoryItemMap = new Dictionary<long, MarketplaceAdvisorInventoryItem>();

        const int downloadPageSize = 200;

        /// <summary>
        /// Constructor
        /// </summary>
        private MarketplaceAdvisorLegacyClient(int accountType, string username, string password, MarketplaceAdvisorLog log)
        {
            this.accountType = accountType;
            this.username = username;
            this.password = password;
            this.log = log;
        }

        /// <summary>
        /// Create an instance of the MarketplaceAdvisorLegacyClient 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static MarketplaceAdvisorLegacyClient Create(MarketplaceAdvisorStoreEntity store)
        {
            return TlsAppDomain.Create<MarketplaceAdvisorLegacyClient>(store.AccountType, store.Username, store.Password,
                new MarketplaceAdvisorLog(typeof(MarketplaceAdvisorLegacyClient)));
        }

        /// <summary>
        /// Issue the GetUser command to get information about a user's MarketplaceAdvisor account
        /// </summary>
        public string GetUser()
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                // User/pass
                WriteOpenRequest(xmlWriter);

                // Command
                xmlWriter.WriteElementString("Command", "GetUser");

                // Close all the elements
                WriteCloseRequest(xmlWriter);

                // Process the request
                return ProcessRequest(stringWriter.ToString()).ToString();
            }
        }

        /// <summary>
        /// Get the orders for the given store given the current page
        /// </summary>
        public string GetOrders(int currentPage)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                // User/pass
                WriteOpenRequest(xmlWriter);

                // Command
                xmlWriter.WriteElementString("Command", "GetOrders");

                DateTime startDate = DateTime.UtcNow.AddDays(-30);
                if (InterapptiveOnly.MagicKeysDown)
                {
                    startDate = DateTime.UtcNow.AddYears(-8);
                }

                // Setup the request
                xmlWriter.WriteElementString("Sequence", "0");
                xmlWriter.WriteElementString("StartDate", startDate.ToString("MM-dd-yyyy HH:mm:ss"));
                xmlWriter.WriteElementString("PerPage", downloadPageSize.ToString());
                xmlWriter.WriteElementString("PageNumber", currentPage.ToString());

                if (!InterapptiveOnly.MagicKeysDown)
                {
                    xmlWriter.WriteElementString("ReadyToShip", "1"); // True
                    xmlWriter.WriteElementString("Shipped", "2"); // False
                    xmlWriter.WriteElementString("Pending", "1");
                    xmlWriter.WriteElementString("Archived", "2");
                    xmlWriter.WriteElementString("Cancelled", "2");
                }

                // Close all the elements
                WriteCloseRequest(xmlWriter);

                // Process the request
                return ProcessRequest(stringWriter.ToString());
            }
        }

        /// <summary>
        /// Mark the given orders numbers as processed within MarketplaceAdvisor 
        /// </summary>
        public void MarkOrdersProcessed(List<long> orderList)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                // User/pass
                WriteOpenRequest(xmlWriter);

                // Command
                xmlWriter.WriteElementString("Command", "UpdateOrderStatus");

                xmlWriter.WriteStartElement("OrderStatus");

                foreach (int orderNumber in orderList)
                {
                    xmlWriter.WriteStartElement("Order");
                    xmlWriter.WriteElementString("Number", orderNumber.ToString());
                    xmlWriter.WriteElementString("Processed", "1");
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

                // Close all the elements
                WriteCloseRequest(xmlWriter);

                // Process the request
                XElement response = XElement.Parse(ProcessRequest(stringWriter.ToString()));
                XPathNavigator xpath = response.CreateNavigator();

                XPathNodeIterator statusNodes = xpath.Select("//Status");
                if (statusNodes.Count != orderList.Count)
                {
                    log.WarnFormat("MarkOrdersProcessed: Updated count ({0}) differs from attempted count ({1}).", statusNodes.Count, orderList.Count);
                }
            }
        }

        /// <summary>
        /// Retrieve MW inventory information for the given item
        /// </summary>
        public MarketplaceAdvisorInventoryItem GetInventoryItem(long itemNumber)
        {
            MarketplaceAdvisorInventoryItem inventoryItem;

            if (!inventoryItemMap.TryGetValue(itemNumber, out inventoryItem))
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                    // User/pass
                    WriteOpenRequest(xmlWriter);

                    xmlWriter.WriteElementString("Command", "GetItem");
                    xmlWriter.WriteElementString("ItemID", itemNumber.ToString());

                    // Close all the elements
                    WriteCloseRequest(xmlWriter);

                    XElement response = XElement.Parse(ProcessRequest(stringWriter.ToString()));
                    XPathNavigator xpath = response.CreateNavigator();

                    XPathNavigator xpathItem = xpath.SelectSingleNode("//Item");
                    if (xpathItem == null)
                    {
                        inventoryItem = new MarketplaceAdvisorInventoryItem();
                    }
                    else
                    {
                        inventoryItem = new MarketplaceAdvisorInventoryItem(xpathItem);
                    }

                    inventoryItemMap[itemNumber] = inventoryItem;
                }
            }

            return inventoryItem;
        }

        /// <summary>
        /// Update the online shipment status of the given order.
        /// </summary>
        public void UpdateShipmentStatus(MarketplaceAdvisorOrderDto order, MarketplaceAdvisorShipmentDto shipment)
        {
            // This stuff is already looked at in the OnlineUpdater class
            Debug.Assert(!order.IsManual && shipment.Processed && !shipment.Voided);

            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

                // User/pass
                WriteOpenRequest(xmlWriter);

                // Command
                xmlWriter.WriteElementString("Command", "UpdateOrderStatus");

                xmlWriter.WriteStartElement("OrderStatus");

                xmlWriter.WriteStartElement("Order");
                xmlWriter.WriteElementString("Number", order.OrderNumber.ToString());
                xmlWriter.WriteElementString("ItemSent", "1");
                xmlWriter.WriteElementString("ItemSentDate", shipment.ShipDate.AddHours(-4).ToString("MM-dd-yyyy HH:mm:ss"));
                xmlWriter.WriteElementString("ShippedTrackingNumber", shipment.TrackingNumber);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                WriteCloseRequest(xmlWriter);

                XElement response = XElement.Parse(ProcessRequest(stringWriter.ToString()));

                XElement statusNode = response.XPathSelectElement("//Status");
                if (statusNode == null)
                {
                    throw new MarketplaceAdvisorException("ShipWorks did not receive update confirmation.");
                }

                string status = statusNode.Value;
                if (status != "Updated")
                {
                    throw new MarketplaceAdvisorException(string.Format("The status was not updated. ({0})", status));
                }
            }
        }

        /// <summary>
        /// Process the given requeist
        /// </summary>
        private string ProcessRequest(string xmlRequest)
        {
            string apiUrl = (accountType == (int)MarketplaceAdvisorAccountType.LegacyCorporate) ?
                "https://api.corporate.marketplaceadvisor.channeladvisor.com/api/apiCall.asp" :
                "https://api.marketplaceadvisor.channeladvisor.com/api/apiCall.asp";

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Uri = new Uri(apiUrl);
            request.Timeout = TimeSpan.FromMinutes(5);

            // Add the request variable
            request.Variables.Add("", xmlRequest);

            // Add interapptive security credentials when we post
            request.RequestSubmitting += (object sender, HttpRequestSubmittingEventArgs e) =>
                {
                    // Add app security headers
                    e.HttpWebRequest.Headers.Add("X-AUCTIONWORKS-API-VALIDATE",
                        SecureText.Decrypt("GSm0jw36uar7tImFZFP0Og==", "interapptive") + ";" +
                        SecureText.Decrypt("GLWw3ReI0rJ9mP0LKD8SiQ==", "interapptive"));
                };

            // Log the request
            //ApiLogEntry logger = new ApiLogEntry(ApiLogSource.MarketplaceAdvisor, XElement.Parse(xmlRequest).Descendants("Command").First().Value);
            MarketplaceAdvisorApiLogger logger = log.CreateApiLogger(XElement.Parse(xmlRequest).Descendants("Command").First().Value);
            logger.LogRequest(xmlRequest);

            try
            {
                using (IHttpResponseReader response = request.GetResponse())
                {
                    string responseXml = response.ReadResult();

                    // Log the response
                    logger.LogResponse(responseXml);

                    responseXml = XmlUtility.StripInvalidXmlCharacters(responseXml);

                    // Load the response and return it
                    XElement xmlResponse = XElement.Parse(responseXml);
                    XPathNavigator xpath = xmlResponse.CreateNavigator();

                    // Determine if an error occurred
                    int errorCode = XPathUtility.Evaluate(xpath, "//Error/ErrorCode", -1);

                    // Error occurred
                    if (errorCode != -1)
                    {
                        string description = XPathUtility.Evaluate(xpath, "//Error/Details/Description", string.Format("Error {0}", errorCode));

                        throw new MarketplaceAdvisorException(description);
                    }

                    return responseXml;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(MarketplaceAdvisorException));
            }
        }

        /// <summary>
        /// Write the opening request element and the user\pass elements
        /// </summary>
        private void WriteOpenRequest(XmlTextWriter xmlWriter)
        {
            // <Request> block
            xmlWriter.WriteStartElement("Request");

            // User \ Pass
            xmlWriter.WriteElementString("Username", username);
            xmlWriter.WriteElementString("Password", SecureText.Decrypt(password, username));
        }

        /// <summary>
        /// To be paired with WriteOpenRequest
        /// </summary>
        private void WriteCloseRequest(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
        }
    }
}
