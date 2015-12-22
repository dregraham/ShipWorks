using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.IO.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Handles communication with Amazon MWS (Marketplace Web Service) for
    /// retrieving orders and uploading shipment information
    /// </summary>
    public sealed class AmazonMwsClient : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonMwsClient));

        public const int MaxItemsPerProductDetailsRequest = 5;

        // MWS settings class
        AmazonMwsWebClientSettings mwsSettings;

        // the store/account we are working with
        AmazonStoreEntity store;

        // Throttling request submitter
        AmazonMwsRequestThrottle throttler;

        // Progress reporting
        IProgressReporter progress;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsClient(AmazonStoreEntity store, IProgressReporter progress)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
            this.mwsSettings = new AmazonMwsWebClientSettings(store as IAmazonCredentials);
            this.throttler = new AmazonMwsRequestThrottle();
            this.progress = progress;
        }

        /// <summary>
        /// Constructor for no throttling
        /// </summary>
        public AmazonMwsClient(AmazonStoreEntity store)
            : this(store, null)
        {

        }

        /// <summary>
        /// The store the webClient is operating on behalf of
        /// </summary>
        public AmazonStoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Makes an api call to make sure the MWS system is not RED (down)
        /// </summary>
        public void TestServiceStatus()
        {
            log.Info("Calling GetServiceStatus to see if the Order api is available.");

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            IHttpResponseReader responseReader = ExecuteRequest(request, AmazonMwsApiCall.GetServiceStatus);

            // check the response for fatal errors
            AmazonMwsResponseHandler.RaiseErrors(AmazonMwsApiCall.GetServiceStatus, responseReader, mwsSettings);

            // read the service status, and log it
            AmazonMwsServiceStatus serviceStatus = ReadServiceStatus(AmazonMwsApiCall.GetServiceStatus, responseReader);
            log.InfoFormat("ServiceStatus responded with '{0}', message = '{1}'", serviceStatus.StatusColor, serviceStatus.Message);

            // only fail if service status is Red
            if (serviceStatus.StatusColor == AmazonMwsServiceStatusColor.Red)
            {
                string errorMessage = "Amazon has reported the Order service is unavailable.";

                if (serviceStatus.Message.Length > 0)
                {
                    errorMessage += " Additional Information: " + serviceStatus.Message;
                }

                throw new AmazonException(errorMessage, null);
            }
        }

        /// <summary>
        /// Makes an api call to see if we can connect with the credentials
        /// </summary>
        public void TestCredentials()
        {
            string dummyNumber = "SHIPWORKS_CONNECT_ATTEMPT";

            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Variables.Add("AmazonOrderId", dummyNumber);

                IHttpResponseReader reader = ExecuteRequest(request, AmazonMwsApiCall.ListOrderItems);
                reader.ReadResult();
            }
            catch (AmazonException ex)
            {
                // if we received the expected InvalidParameterValue, we authenticated just fine
                if (String.Compare(ex.Code, "InvalidParameterValue", StringComparison.OrdinalIgnoreCase) == 0 &&
                                  !ex.Message.Contains(dummyNumber))
                {
                    throw new AmazonException("Unable to access your Amazon MWS account.  Please grant ShipWorks access.", ex);
                }
            }
        }

        /// <summary>
        /// Get the list of marketplaces associated with the given merchantID
        /// </summary>
        public List<AmazonMwsMarketplace> GetMarketplaces()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("SellerId", store.MerchantID);
            request.Variables.Add("MWSAuthToken", store.AuthToken);

            AmazonMwsApiCall apiCall = AmazonMwsApiCall.ListMarketplaceParticipations;

            IHttpResponseReader response = ExecuteRequest(request, apiCall);

            string responseXml = response.ReadResult();
            XDocument xDocument = XDocument.Parse(responseXml);

            List<AmazonMwsMarketplace> marketplaces = new List<AmazonMwsMarketplace>();

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("amz", mwsSettings.GetApiNamespace(apiCall).NamespaceName);

            XNamespace amz = mwsSettings.GetApiNamespace(apiCall).NamespaceName;

            foreach (var xElement in xDocument.XPathSelectElements("//amz:ListMarketplaces/amz:Marketplace", namespaceManager))
            {
                string id = (string) xElement.Element(amz + "MarketplaceId");
                string name = (string) xElement.Element(amz + "Name");
                string domain = (string) xElement.Element(amz + "DomainName");

                marketplaces.Add(new AmazonMwsMarketplace { MarketplaceID = id, Name = name, DomainName = domain });
            }

            return marketplaces;
        }

        /// <summary>
        /// Executes a request for more orders
        /// </summary>
        public IEnumerable<XPathNamespaceNavigator> GetOrders(DateTime? startDate)
        {
            string nextToken = null;

            DateTime timeToSend = startDate.HasValue ? startDate.Value : DateTime.UtcNow.AddYears(-2);

            // Amazon will reject it if the value is within 2 minutes from the time the api call is made
            if (timeToSend > DateTime.UtcNow - TimeSpan.FromMinutes(2))
            {
                timeToSend = DateTime.UtcNow - TimeSpan.FromMinutes(2);
            }

            // api request
            do
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Variables.Add("MarketplaceId.Id.1", store.MarketplaceID);

                // When testing paging, set that we only want a single result
                // request.Variables.Add("MaxResultsPerPage", "2");

                // call different apis based on whether or not we have a nextToken
                AmazonMwsApiCall apiCall = nextToken == null ? AmazonMwsApiCall.ListOrders : AmazonMwsApiCall.ListOrdersByNextToken;

                // add the appropriate Date parameter
                if (apiCall == AmazonMwsApiCall.ListOrders)
                {
                    request.Variables.Add("LastUpdatedAfter", FormatDate(timeToSend));
                }
                else
                {
                    request.Variables.Add("NextToken", nextToken);
                }

                // See if we are excluding FBA
                if (store.ExcludeFBA)
                {
                    request.Variables.Add("FulfillmentChannel.Channel.1", "MFN");
                }

                // Do not download Pending orders - they don't have data
                request.Variables.Add("OrderStatus.Status.1", "Unshipped");
                request.Variables.Add("OrderStatus.Status.2", "PartiallyShipped");
                request.Variables.Add("OrderStatus.Status.3", "Shipped");
                request.Variables.Add("OrderStatus.Status.4", "Canceled");
                request.Variables.Add("OrderStatus.Status.5", "Unfulfillable");

                // make the call
                IHttpResponseReader response = ExecuteRequest(request, apiCall);
                var xpath = AmazonMwsResponseHandler.GetXPathNavigator(response, apiCall, mwsSettings);

                // find the token for the next request
                nextToken = ReadNextToken(xpath);

                // done
                yield return xpath;
            }
            while (nextToken != null && nextToken.Length > 0);
        }

        /// <summary>
        /// Reads the NextToken for use in subsequent requests
        /// </summary>
        private static string ReadNextToken(XPathNamespaceNavigator xpath)
        {
            return XPathUtility.Evaluate(xpath, "//amz:NextToken", "");
        }

        /// <summary>
        /// Retrieves an order's items
        /// </summary>
        public IEnumerable<XPathNavigator> GetOrderItems(string amazonOrderID)
        {
            string nextToken = null;

            do
            {
                // create request
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

                // determine which API to use
                AmazonMwsApiCall apiCall = nextToken == null ? AmazonMwsApiCall.ListOrderItems : AmazonMwsApiCall.ListOrderItemsByNextToken;

                // add the appropriate parameters
                switch (apiCall)
                {
                    case AmazonMwsApiCall.ListOrderItems:
                        request.Variables.Add("AmazonOrderId", amazonOrderID);
                        break;

                    case AmazonMwsApiCall.ListOrderItemsByNextToken:
                        request.Variables.Add("NextToken", nextToken);
                        break;
                }

                // execute the request
                IHttpResponseReader response = ExecuteRequest(request, apiCall);
                var xpath = AmazonMwsResponseHandler.GetXPathNavigator(response, apiCall, mwsSettings);

                // find the token for the next request
                nextToken = ReadNextToken(xpath);

                // done
                yield return xpath;

            }
            while (nextToken != null && nextToken.Length > 0);
        }


        /// <summary>
        /// Gets additional details from Amazon for the given order items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>An XPathNamespaceNavigator object.</returns>
        public XPathNamespaceNavigator GetProductDetails(List<AmazonOrderItemEntity> items)
        {

            if (items.Count > MaxItemsPerProductDetailsRequest)
            {
                string message = string.Format("There is a {0} item limit on the number of products the Amazon API allows to be retrieved in a single request", MaxItemsPerProductDetailsRequest);
                throw new InvalidOperationException(message);
            }

            // create request
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("IdType", "ASIN");
            request.Variables.Add("MarketplaceId", store.MarketplaceID);

            for (int i = 0; i < items.Count; i++)
            {
                // Add a request variable for each of the items in our list
                request.Variables.Add(string.Format("IdList.Id.{0}", i + 1), items[i].ASIN);
            }

            // execute the request
            IHttpResponseReader response = ExecuteRequest(request, AmazonMwsApiCall.GetMatchingProductForId);
            var xpath = AmazonMwsResponseHandler.GetXPathNavigator(response, AmazonMwsApiCall.GetMatchingProductForId, mwsSettings);

            // Add the additional namespace so weight, image URL, and other data about the
            // product can be extracted
            xpath.Namespaces.AddNamespace("details",
                string.Format("http://mws.amazonservices.com/schema/Products/{0}/default.xsd",
                mwsSettings.GetApiVersion(AmazonMwsApiCall.GetMatchingProductForId)));

            // done
            return xpath;
        }

        /// <summary>
        /// Upload shipments
        /// </summary>
        public void UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            if (shipments == null || shipments.Count == 0)
            {
                return;
            }

            SubmitFulfillmentFeed(CreateFulfillmentFeed(shipments));
        }

        /// <summary>
        /// Submits the fulfillment feed XML
        /// </summary>
        private void SubmitFulfillmentFeed(string feedXml)
        {
            AmazonMwsFeedRequestSubmitter request = new AmazonMwsFeedRequestSubmitter();

            // we work with a single marketplace at once
            request.Variables.Add("MarketplaceId.Id.1", store.MarketplaceID);

            // The feed apparently takes the old Merchant Token or the new Merchant ID (aka Seller ID)
            request.Variables.Add("Merchant", store.MerchantID);

            // Uploading fulfillment
            request.Variables.Add("FeedType", "_POST_ORDER_FULFILLMENT_DATA_");

            // the request itself will generate the MD5 hash
            request.FeedContent = feedXml;

            // execute the request
            IHttpResponseReader response = ExecuteRequest(request, AmazonMwsApiCall.SubmitFeed);

            // extract the feed submission ID for logging
            string responseText = response.ReadResult();

            XNamespace ns = mwsSettings.GetApiNamespace(AmazonMwsApiCall.SubmitFeed);
            XDocument xdoc = XDocument.Parse(responseText);

            string feedSubmissionId = (string) xdoc.Descendants(ns + "FeedSubmissionId").FirstOrDefault();
            if (feedSubmissionId != null)
            {
                log.InfoFormat("Document submitted to Amazon received SubmissionId {0}", feedSubmissionId);
            }
        }

        /// <summary>
        /// Write the common header information for the fulfillment report.
        /// </summary>
        private void WriteFulfillmentStart(XmlTextWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("AmazonEnvelope");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "amzn-envelope.xsd");

            writer.WriteStartElement("Header");
            writer.WriteElementString("DocumentVersion", "1.01");
            writer.WriteElementString("MerchantIdentifier", store.MerchantID);
            writer.WriteEndElement();

            writer.WriteElementString("MessageType", "OrderFulfillment");
        }

        /// <summary>
        /// Creates the Amazon Feed Xml for submitting and returns the path it is written to
        /// </summary>
        [NDependIgnoreLongMethod]
        private string CreateFulfillmentFeed(List<ShipmentEntity> shipments)
        {
            using (TextWriter textWriter = new EncodingStringWriter(Encoding.UTF8))
            {
                XmlTextWriter writer = new XmlTextWriter(textWriter);
                writer.Formatting = Formatting.Indented;

                WriteFulfillmentStart(writer);

                // Write each shipment
                foreach (ShipmentEntity shipment in shipments)
                {
                    AmazonOrderEntity amazonOrder = shipment.Order as AmazonOrderEntity;
                    if (amazonOrder == null)
                    {
                        log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is not for an Amazon order.", shipment.ShipmentID);
                        continue;
                    }

                    if (amazonOrder.IsManual)
                    {
                        log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is manual.", shipment.ShipmentID);
                        continue;
                    }

                    if (!shipment.Processed || shipment.Voided)
                    {
                        log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it is not processed or it is voided.", shipment.ShipmentID);
                        continue;
                    }

                    try
                    {
                        ShippingManager.EnsureShipmentLoaded(shipment);
                    }
                    catch (ObjectDeletedException)
                    {
                        log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it or it's related info has been deleted.", shipment.ShipmentID);
                        continue;
                    }
                    catch (SqlForeignKeyException)
                    {
                        log.WarnFormat("Shipment '{0}' was not uploaded to Amazon because it or it's related info has been deleted.", shipment.ShipmentID);
                        continue;
                    }

                    writer.WriteStartElement("Message");

                    // Can't use order number here since the Message ID must be unique per submission, so using the shipment ID by definition
                    // will always be unique regardless of the number of shipments processed for a single order in this submission
                    writer.WriteElementString("MessageID", shipment.ShipmentID.ToString());

                    writer.WriteStartElement("OrderFulfillment");
                    writer.WriteElementString("AmazonOrderID", amazonOrder.AmazonOrderID);

                    DateTime shipDate = shipment.ShipDate.ToLocalTime();

                    // shipdate can't be before the order was placed
                    if (shipDate < shipment.Order.OrderDate)
                    {
                        // set it 10 minutes after it was placed
                        shipDate = shipment.Order.OrderDate.AddMinutes(10);
                    }

                    // shipment can't be in the future
                    if (shipDate > DateTime.Now)
                    {
                        shipDate = DateTime.Now;
                    }

                    writer.WriteElementString("FulfillmentDate", shipDate.ToString("yyyy-MM-ddTHH:mm:sszzzz"));

                    writer.WriteStartElement("FulfillmentData");

                    // Per an email on 9/11/07, Amazon will only respond correctly if the code is in upper case, and if its also apart of the method.
                    ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;
                    string trackingNumber = shipment.TrackingNumber;

                    // Get the service used and strip out any non-ascii characters
                    string serviceUsed = ShippingManager.GetServiceUsed(shipment);
                    serviceUsed = Regex.Replace(serviceUsed, @"[^\u001F-\u007F]", string.Empty);

                    // Get the carrier based on what we currently know, we'll check it in the DetermineAlternateTracking below
                    string carrier = GetCarrierName(shipment, shipmentType);

                    // Adjust tracking details per Mail Innovations and others
                    WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                    {
                        if (track.Length > 0)
                        {
                            trackingNumber = track;
                            carrier = "UPS Mail Innovations";
                        }
                        else
                        {
                            shipmentType = ShipmentTypeCode.Other;
                        }

                    });

                    writer.WriteElementString("CarrierName", carrier);
                    writer.WriteElementString("ShippingMethod", serviceUsed);

                    writer.WriteElementString("ShipperTrackingNumber", trackingNumber);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Gets the carrier for the shipment.  If the shipment type is Other, it will use Other.Carrier.
        /// </summary>
        /// <param name="shipment">The shipment for which to get the carrier name.</param>
        /// <param name="shipmentTypeCode">The shipment type code for this shipment.</param>
        /// <returns>The carrier name of the shipment type, unless it is of type Other, then the Other.Carrier is returned.</returns>
        public static string GetCarrierName(ShipmentEntity shipment, ShipmentTypeCode shipmentTypeCode)
        {
            string carrier;
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Other)
            {
                carrier = ShippingManager.GetOtherCarrierDescription(shipment).Name;
            }
            else if (ShipmentTypeManager.ShipmentTypeCodeSupportsDhl((ShipmentTypeCode) shipment.ShipmentType))
            {
                PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                // The shipment is an Endicia or Stamps shipment, check to see if it's DHL
                if (ShipmentTypeManager.IsDhl(service))
                {
                    // The DHL carrier for Endicia/Stamps is:
                    carrier = "DHL Global Mail";
                }
                else if (ShipmentTypeManager.IsConsolidator(service))
                {
                    carrier = "Consolidator";
                }
                else
                {
                    // Use the default carrier for other Endicia types
                    carrier = ShippingManager.GetCarrierName(shipmentTypeCode);
                }
            }
            else
            {
                carrier = ShippingManager.GetCarrierName(shipmentTypeCode);
            }

            return carrier;
        }

        /// <summary>
        /// Determines if the local system clock is in sync with Amazon's servers.
        /// ONLY fails if we receive a time from Amazon and we are for sure out of sync.
        /// </summary>
        public bool ClockInSyncWithMWS()
        {
            try
            {
                // a simple GET against the MWS url provides xml containing the server time
                HttpRequestSubmitter submitter = new HttpVariableRequestSubmitter();
                submitter.Uri = new Uri(mwsSettings.Endpoint);
                submitter.Verb = HttpVerb.Get;

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string response = reader.ReadResult();
                    if (!string.IsNullOrEmpty(response))
                    {
                        XDocument document = XDocument.Parse(response);
                        XElement root = document.Root;

                        XElement timestampElement = root.Element("Timestamp");
                        if (timestampElement != null)
                        {
                            string timeString = (string) timestampElement.Attribute("timestamp");
                            DateTime serverTime;

                            if (!string.IsNullOrEmpty(timeString) && DateTime.TryParse(timeString, out serverTime))
                            {
                                // go to UTC
                                serverTime = serverTime.ToUniversalTime();

                                // verify we are within 10 minutes of Amazon's server time.  They say 15 minutes is the max.
                                TimeSpan difference = (serverTime - DateTime.UtcNow).Duration();
                                if (difference.TotalMinutes > 10)
                                {
                                    log.ErrorFormat("The local computer's time is {0} off of Amazon's server's time.  Max variation is 10 minutes.", difference);

                                    return false;
                                }
                            }
                        }
                    }
                }

                // defaulting to the time being OK
                return true;
            }
            catch (Exception)
            {
                log.WarnFormat("Unable to determine if local clock is in sync with Amazon Services, continuing under the assumption it is.");

                // don't want any "ping" failure to prevent downloads and uploads if possible
                return true;
            }
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        [NDependIgnoreLongMethod]
        private IHttpResponseReader ExecuteRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall)
        {
            try
            {
                DateTime timestamp = DateTime.UtcNow;

                string endpointPath = mwsSettings.GetApiEndpointPath(amazonMwsApiCall);

                request.Uri = new Uri(mwsSettings.Endpoint + endpointPath);
                request.VariableEncodingCasing = QueryStringEncodingCasing.Upper;

                request.Variables.Add("Action", mwsSettings.GetActionName(amazonMwsApiCall));

                // For the ListParticipations call, this is exactly the data we are trying to find
                if (amazonMwsApiCall != AmazonMwsApiCall.ListMarketplaceParticipations)
                {
                    request.Variables.Add("SellerId", store.MerchantID);
                    request.Variables.Add("Marketplace", store.MarketplaceID);
                }

                if (amazonMwsApiCall != AmazonMwsApiCall.GetAuthToken && amazonMwsApiCall != AmazonMwsApiCall.ListMarketplaceParticipations)
                {
                    AddMwsAuthToken(request);
                }

                request.Variables.Add("SignatureMethod", "HmacSHA256");
                request.Variables.Add("SignatureVersion", "2");
                request.Variables.Add("Timestamp", FormatDate(timestamp));
                request.Variables.Add("Version", mwsSettings.GetApiVersion(amazonMwsApiCall));
                request.Variables.Add("AWSAccessKeyId", Decrypt(mwsSettings.InterapptiveAccessKeyID));

                // now construct the signature parameter
                string verbString = request.Verb == HttpVerb.Get ? "GET" : "POST";
                string queryString = QueryStringUtility.GetQueryString(
                    request.Variables.OrderBy(v => v.Name, StringComparer.Ordinal),
                    QueryStringEncodingCasing.Upper);

                string parameterString = String.Format("{0}\n{1}\n{2}\n{3}", verbString, request.Uri.Host, endpointPath, queryString);

                // sign the string and add it to the request
                string signature = RequestSignature.CreateRequestSignature(parameterString, Decrypt(mwsSettings.InterapptiveSecretKey), SigningAlgorithm.SHA256);
                request.Variables.Add("Signature", signature);

                // add a User Agent header
                request.Headers.Add("x-amazon-user-agent", String.Format("ShipWorks/{0} (Language=.NET)", Assembly.GetExecutingAssembly().GetName().Version));

                // business logic failures are handled through status codes
                request.AllowHttpStatusCodes(new HttpStatusCode[] { HttpStatusCode.BadRequest });

                log.InfoFormat("Submitting request for {0}", amazonMwsApiCall);

                ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Amazon, mwsSettings.GetActionName(amazonMwsApiCall));

                // log the request
                logger.LogRequest(request);

                // Feed uploads are a combination of querystring params AND POST data, which isn't handled by typical request logging
                AmazonMwsFeedRequestSubmitter feedRequest = request as AmazonMwsFeedRequestSubmitter;
                if (feedRequest != null)
                {
                    logger.LogRequestSupplement(feedRequest.GetPostContent(), "FeedDocument", "xml");
                }

                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(amazonMwsApiCall, request, progress);

                IHttpResponseReader response =
                    throttler.ExecuteRequest<HttpRequestSubmitter, IHttpResponseReader>(requestThrottleArgs, MakeRequest);

                // log the response
                logger.LogResponse(response.ReadResult());

                // check response for errors
                AmazonMwsResponseHandler.RaiseErrors(amazonMwsApiCall, response, mwsSettings);

                return response;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmazonException));
            }
        }

        /// <summary>
        /// Adds the MWS authentication token - throws if no value.
        /// </summary>
        /// <exception cref="AmazonException">No MWS Auth Token. Go to store settings to enter Token Value.</exception>
        private void AddMwsAuthToken(HttpVariableRequestSubmitter request)
        {
            if (string.IsNullOrWhiteSpace(store.AuthToken))
            {
                GetAuthToken();
            }

            request.Variables.Add("MWSAuthToken", store.AuthToken);
        }

        /// <summary>
        /// Gets the MWS authentication token. - Will only work before 3/13/2015
        /// </summary>
        /// <returns></returns>
        private void GetAuthToken()
        {
            // create request
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            try
            {
                IHttpResponseReader response = ExecuteRequest(request, AmazonMwsApiCall.GetAuthToken);

                XPathNamespaceNavigator responseNavigator = AmazonMwsResponseHandler.GetXPathNavigator(response, AmazonMwsApiCall.GetAuthToken, mwsSettings);
                XPathNavigator selectSingleNode = responseNavigator.SelectSingleNode("//amz:MWSAuthToken");
                if (selectSingleNode == null)
                {
                    throw new AmazonException(typeof(AmazonMwsClient), "Token not returned");
                }

                store.AuthToken = selectSingleNode.Value;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(store);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmazonException));
            }
        }

        /// <summary>
        /// Submit an Amazon request, throttled so we don't over-call
        /// </summary>
        /// <typeparam name="THttpRequestSubmitter">Needed by the throttler.  The type of the request to send to the api via throttler.</typeparam>
        /// <typeparam name="THttpResponseReader">Needed by the throttler.  The type of the response that will be received by the api via throttler.</typeparam>
        /// <param name="request">The actual request to make.</param>
        /// <returns>HttpResponseReader received from the call</returns>
        private IHttpResponseReader MakeRequest<THttpRequestSubmitter>(THttpRequestSubmitter request)
            where THttpRequestSubmitter : HttpRequestSubmitter
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                IHttpResponseReader responseReader = request.GetResponse();

                return responseReader;
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = ex.Response as HttpWebResponse;
                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    throw new RequestThrottledException(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Reads the status of the Amazon MWS api out of a GetServiceStatus repsonse
        /// </summary>
        private AmazonMwsServiceStatus ReadServiceStatus(AmazonMwsApiCall api, IHttpResponseReader reader)
        {
            try
            {
                XNamespace ns = mwsSettings.GetApiNamespace(api);

                string responseText = reader.ReadResult();
                XDocument xdoc = XDocument.Parse(responseText);

                string colorString = (string) xdoc.Descendants(ns + "Status").FirstOrDefault() ?? "";
                AmazonMwsServiceStatusColor statusColor = AmazonMwsServiceStatusColor.Green;
                string message = "";

                var firstMessage = (from e in xdoc.Descendants(ns + "Message")
                                    select new
                                    {
                                        Text = (string) e.Element(ns + "Text"),
                                    }).FirstOrDefault();


                // get the message text
                if (firstMessage != null)
                {
                    message = firstMessage.Text;
                }

                // translate the color string
                switch (colorString)
                {
                    case "GREEN":
                        statusColor = AmazonMwsServiceStatusColor.Green;
                        break;

                    case "GREEN_I":
                        statusColor = AmazonMwsServiceStatusColor.GreenI;
                        break;

                    case "YELLOW":
                        statusColor = AmazonMwsServiceStatusColor.Yellow;
                        break;

                    case "RED":
                        statusColor = AmazonMwsServiceStatusColor.Red;
                        break;

                    default:
                        statusColor = AmazonMwsServiceStatusColor.Yellow;
                        message = string.Format("Unknown status color '{0}', message: '{1}'", colorString, message);
                        break;
                }

                return new AmazonMwsServiceStatus() { StatusColor = statusColor, Message = message };
            }
            catch (XmlException)
            {
                // we don't want to totally cause a failure (so we avoid Red).
                return new AmazonMwsServiceStatus() { Message = "Invalid service status response returned.", StatusColor = AmazonMwsServiceStatusColor.Yellow };
            }
        }

        /// <summary>
        /// Formats a date to make it appropriate/safe for Amazon
        /// </summary>
        private static string FormatDate(DateTime dateTime)
        {
            // not including milliseconds
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Returns the decrypted Interapptive Developer Access Key
        /// </summary>
        private static string Decrypt(string encrypted)
        {
            return SecureText.Decrypt(encrypted, "Interapptive");
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            throttler.Dispose();
        }
    }
}
