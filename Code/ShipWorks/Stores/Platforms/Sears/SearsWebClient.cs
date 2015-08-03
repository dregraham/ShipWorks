using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.Xml;
using System.Text.RegularExpressions;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Shipping;
using log4net;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Used to interact with a sears.com store
    /// </summary>
    public class SearsWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SearsWebClient));

        const string searsOrdersUrl = "https://seller.marketplace.sears.com/SellerPortal/api/oms/purchaseorder/v11";
        const string searsUpdateUrl = "https://seller.marketplace.sears.com/SellerPortal/api/oms/asn/v5";

        SearsStoreEntity searsStore;

        DateTime downloadPageStart = DateTime.MinValue;
        DateTime downloadPageCurrent = DateTime.MinValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsWebClient(SearsStoreEntity searsStore)
        {
            if (searsStore == null)
            {
                throw new ArgumentNullException("searsStore");
            }

            this.searsStore = searsStore;
        }

        /// <summary>
        /// Test to see if the credentials for the current store are valid
        /// </summary>
        public void TestConnection()
        {
            try
            {
                InitializeForDownload(DateTime.UtcNow);

                // Just try to get today's worth of orders as a test
                GetNextOrdersPage();
            }
            catch (SearsException ex)
            {
                throw new SearsException("ShipWorks could not connect to your Sears account with the given email address and password.", ex);
            }
        }

        /// <summary>
        /// Initialize for downloading
        /// </summary>
        public void InitializeForDownload(DateTime startDate)
        {
            // Sears does things based on their local time - this gets us as close as we can get, using this PC's local time
            downloadPageStart = SearsUtility.ConvertUtcToSearsTimeZone(startDate).Date;
            downloadPageCurrent = downloadPageStart;
        }

        /// <summary>
        /// Return the next page of orders based on the given lastModified date
        /// </summary>
        public SearsOrdersPage GetNextOrdersPage()
        {
            if (downloadPageStart == DateTime.MinValue)
            {
                throw new InvalidOperationException("The webclient has not be initialized for paging.");
            }

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Uri = new Uri(searsOrdersUrl);
            request.Verb = HttpVerb.Get;

            GetCredentialsHttpVariables().ToList().ForEach(v => request.Variables.Add(v));

            request.Variables.Add("fromdate", downloadPageCurrent.ToString("yyyy-MM-dd"));
            request.Variables.Add("todate", downloadPageCurrent.ToString("yyyy-MM-dd"));

            // Get the response and the navigator
            XmlDocument response = ProcessRequest(request, string.Format("GetOrders [{0}]", request.Variables["fromdate"]));
            XPathNavigator xpath = response.CreateNavigator();

            SearsOrdersPage page = new SearsOrdersPage(
                ((int) (downloadPageCurrent - downloadPageStart).TotalDays) + 1,
                ((int) (SearsUtility.ConvertUtcToSearsTimeZone(DateTime.UtcNow).Date - downloadPageStart).TotalDays) + 1,
                xpath);

            // Advance the date
            downloadPageCurrent = downloadPageCurrent.AddDays(1);

            return page;
        }

        /// <summary>
        /// Upload the details of the given shipment to Sears
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            XDocument xDocument = GenerateShipmentFeedXml(shipment);

            HttpXmlVariableRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter();
            submitter.Uri = new Uri(searsUpdateUrl + "?" + QueryStringUtility.GetQueryString(GetCredentialsHttpVariables()));
            submitter.Verb = HttpVerb.Put;
            submitter.ContentType = "application/xml";

            submitter.Variables.Add(string.Empty, xDocument.ToString());

            ProcessRequest(submitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Generate the XML to upload to sears for the given shipment
        /// </summary>
        private XDocument GenerateShipmentFeedXml(ShipmentEntity shipment)
        {
            SearsOrderEntity order = (SearsOrderEntity) shipment.Order;

            XNamespace nsDefault = XNamespace.Get("http://seller.marketplace.sears.com/oms/v5");
            XNamespace nsXsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement(nsDefault + "shipment-feed",
                    new XAttribute(XNamespace.Xmlns + "xsi", nsXsi)));

            XElement xShipment = new XElement(nsDefault + "shipment",
                new XElement(nsDefault + "header",
                    new XElement(nsDefault + "asn-number", order.PoNumber + "1000"),
                    new XElement(nsDefault + "po-number", order.PoNumber),
                    new XElement(nsDefault + "po-date", SearsUtility.ConvertUtcToSearsTimeZone(order.OrderDate).ToString("yyyy-MM-dd"))));

            foreach (SearsOrderItemEntity orderItem in DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderItemEntity).OfType<SearsOrderItemEntity>())
            {
                xShipment.Add(
                    new XElement(nsDefault + "detail",
                        new XElement(nsDefault + "tracking-number", shipment.TrackingNumber.Trim()),
                        new XElement(nsDefault + "ship-date", shipment.ShipDate.ToString("yyyy-MM-dd")),
                        new XElement(nsDefault + "shipping-carrier", SearsUtility.GetShipmentCarrierCode(shipment)),
                        new XElement(nsDefault + "shipping-method", SearsUtility.GetShpmentServiceCode(shipment)),
                        new XElement(nsDefault + "package-detail",
                            new XElement(nsDefault + "line-number", orderItem.LineNumber),
                            new XElement(nsDefault + "item-id", orderItem.ItemID),
                            new XElement(nsDefault + "quantity", orderItem.Quantity))));
            }

            xDoc.Root.Add(xShipment);

            return xDoc;
        }

        /// <summary>
        /// Return the collection of HTTP variables for authenticating
        /// </summary>
        private HttpVariableCollection GetCredentialsHttpVariables()
        {
            HttpVariableCollection credentials = new HttpVariableCollection();
            credentials.Add("email", searsStore.Email);
            credentials.Add("password", SecureText.Decrypt(searsStore.Password, searsStore.Email));

            return credentials;
        }

        /// <summary>
        /// Process a given request, logged based on the specified action
        /// </summary>
        private XmlDocument ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            // log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Sears, action);
            logger.LogRequest(request);

            // execute the request
            try
            {
                using (IHttpResponseReader postResponse = request.GetResponse())
                {
                    string resultXml = postResponse.ReadResult();

                    // log the response
                    logger.LogResponse(resultXml);

                    // Strip invalid input characters. (Don't trust anyone)
                    resultXml = XmlUtility.StripInvalidXmlCharacters(resultXml);

                    // Stripping the default namespace will make XPath querying much much much easier
                    resultXml = StripDefaultNamespace(resultXml);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(resultXml);

                    CheckForErrors(xmlDocument);

                    return xmlDocument;
                }
            }
            catch (XmlException ex)
            {
                throw new SearsException("Sears returned an invalid response to ShipWorks.", ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(SearsException));
            }
        }

        /// <summary>
        /// Strip the default namesapace, if any
        /// </summary>
        private string StripDefaultNamespace(string resultXml)
        {
            return Regex.Replace(resultXml, "xmlns=\".+?\"", "");
        }

        /// <summary>
        /// Check the given XmlDocument for errors
        /// </summary>
        private void CheckForErrors(XmlDocument xmlDocument)
        {
            if (xmlDocument.DocumentElement.Name == "api-response")
            {
                XmlNode errorNode = xmlDocument.GetElementsByTagName("error-detail").OfType<XmlNode>().FirstOrDefault();
                if (errorNode != null)
                {
                    // No POs found isn't an error, it just means there aren't any orders to download
                    if (errorNode.InnerText == "No POs found")
                    {
                        return;
                    }
                    else
                    {
                        throw new SearsException(errorNode.InnerText);
                    }
                }
                else
                {
                    XmlNode documentIdNode = xmlDocument.GetElementsByTagName("document-id").OfType<XmlNode>().FirstOrDefault();
                    if (documentIdNode != null && !string.IsNullOrWhiteSpace(documentIdNode.InnerText))
                    {
                        log.InfoFormat("Sears returned document ID {0} as the response.", documentIdNode.InnerText);
                        return;
                    }

                    throw new SearsException("An unknown error was returned by Sears.com.");
                }
            }
        }
    }
}
