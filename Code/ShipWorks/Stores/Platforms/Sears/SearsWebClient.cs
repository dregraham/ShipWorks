﻿using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Used to interact with a sears.com store
    /// </summary>
    public class SearsWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SearsWebClient));

        private readonly SearsStoreEntity searsStore;
        private DateTime downloadPageStart = DateTime.MinValue;
        private DateTime downloadPageCurrent = DateTime.MinValue;

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
        /// Determines if we should connect to the live server
        /// </summary>
        public static bool UseLiveServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("SearsLiveServer", true);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("SearsLiveServer", value);
            }
        }

        /// <summary>
        /// Gets the sears orders URL.
        /// </summary>
        private string SearsOrdersUrl => $"https://{HostName}/SellerPortal/api/oms/purchaseorder/v11";

        /// <summary>
        /// Gets the sears update URL.
        /// </summary>
        private string SearsUpdateUrl => $"https://{HostName}/SellerPortal/api/oms/asn/v7";


        /// <summary>
        /// Gets the name of the host.
        /// </summary>
        private string HostName
        {
            get {
                return UseLiveServer ? "seller.marketplace.sears.com" : "sellersandbox.sears.com";
            }
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
                throw new SearsException("ShipWorks could not connect to your Sears account with the given email, seller ID, and secret key.", ex);
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
            request.Uri = new Uri(SearsOrdersUrl);
            request.Verb = HttpVerb.Get;

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
        public void UploadShipmentDetails(SearsOrderDetail orderDetail, IEnumerable<SearsTracking> searsTrackingEntries)
        {
            XDocument xDocument = GenerateShipmentFeedXml(orderDetail, searsTrackingEntries);

            HttpXmlVariableRequestSubmitter submitter = new HttpXmlVariableRequestSubmitter();
            submitter.Uri = new Uri(SearsUpdateUrl);
            submitter.Verb = HttpVerb.Put;
            submitter.ContentType = "application/xml";

            submitter.Variables.Add(string.Empty, xDocument.ToString());

            ProcessRequest(submitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Generate the XML to upload to sears for the given shipment
        /// </summary>
        private XDocument GenerateShipmentFeedXml(SearsOrderDetail orderDetail, IEnumerable<SearsTracking> searsTrackingEntries)
        {
            XNamespace nsDefault = XNamespace.Get("http://seller.marketplace.sears.com/oms/v7");
            XNamespace nsXsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement(nsDefault + "shipment-feed",
                    new XAttribute(XNamespace.Xmlns + "xsi", nsXsi)));

            XElement xShipment = new XElement(nsDefault + "shipment",
                new XElement(nsDefault + "header",
                    new XElement(nsDefault + "asn-number", orderDetail.PoNumber + "1000"),
                    new XElement(nsDefault + "po-number", orderDetail.PoNumber),
                    new XElement(nsDefault + "po-date", SearsUtility.ConvertUtcToSearsTimeZone(orderDetail.OrderDate).ToString("yyyy-MM-dd"))));

            foreach (SearsTracking searsTracking in searsTrackingEntries)
            {
                string trackingNumber = searsTracking.TrackingNumber.Trim();
                if (string.IsNullOrEmpty(trackingNumber))
                {
                    trackingNumber = "Tracking unavailable";
                }
                xShipment.Add(
                    new XElement(nsDefault + "detail",
                        new XElement(nsDefault + "tracking-number", trackingNumber),
                        new XElement(nsDefault + "ship-date", searsTracking.ShipDate.ToString("yyyy-MM-dd")),
                        new XElement(nsDefault + "shipping-carrier", searsTracking.Carrier),
                        new XElement(nsDefault + "shipping-method", searsTracking.Method),
                        new XElement(nsDefault + "package-detail",
                            new XElement(nsDefault + "line-number", searsTracking.LineNumber),
                            new XElement(nsDefault + "item-id", searsTracking.ItemID),
                            new XElement(nsDefault + "quantity", searsTracking.Quantity))));
            }

            xDoc.Root.Add(xShipment);

            return xDoc;
        }

        /// <summary>
        /// Process a given request, logged based on the specified action
        /// </summary>
        private XmlDocument ProcessRequest(HttpVariableRequestSubmitter request, string action)
        {
            // Add the credentials
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                // Resolve the SearsCredentials using our store entity and request parameters
                TypedParameter storeParameter = new TypedParameter(typeof(SearsStoreEntity), searsStore);
                TypedParameter requestParameter = new TypedParameter(typeof(HttpVariableRequestSubmitter), request);

                SearsCredentials credentials = lifetimeScope.Resolve<SearsCredentials>(storeParameter, requestParameter);
                credentials.AddCredentials();
            }

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
