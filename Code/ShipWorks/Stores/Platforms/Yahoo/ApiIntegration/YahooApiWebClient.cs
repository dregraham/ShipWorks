using System;
using System.IO;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Class for interacting with the yahoo merchant API
    /// </summary>
    [Component]
    public class YahooApiWebClient : IYahooApiWebClient
    {
        private readonly ILog log;
        private const int OrdersPerPage = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiWebClient"/> class.
        /// </summary>
        public YahooApiWebClient(Func<Type, ILog> createLog)
        {
            this.log = createLog(GetType());
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        public YahooResponse ValidateCredentials(IYahooStoreEntity store)
        {
            return GetCustomOrderStatus(store, -1);
        }

        /// <summary>
        /// Gets the custom order status.
        /// </summary>
        /// <param name="statusID">The status identifier.</param>
        public YahooResponse GetCustomOrderStatus(IYahooStoreEntity store, int statusID)
        {
            string body = RequestBodyIntro(store) + GetRequestBodyIntro +
                          "<CustomOrderStatusListQuery>" +
                          "<CustomQueryParams>" +
                          $"<StatusID>{statusID}</StatusID>" +
                          "</CustomQueryParams>" +
                          "</CustomOrderStatusListQuery>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(YahooOrderEndpoint(store) + "/order")
            };

            return ProcessRequest(submitter, "GetCustomOrderStatus");
        }

        /// <summary>
        /// Gets an order.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        public YahooResponse GetOrder(IYahooStoreEntity store, long orderID)
        {
            string body = RequestBodyIntro(store) + GetRequestBodyIntro +
                "<OrderListQuery>" +
                "<Filter>" +
                "<Include>all</Include>" +
                "</Filter>" +
                "<QueryParams>" +
                $"<OrderID>{orderID}</OrderID>" +
                "</QueryParams>" +
                "</OrderListQuery>" +
                "</ResourceList>" +
                "</ystorewsRequest>";

            return ProcessRequest(CreateOrderRequestSubmitter(store, body), "GetOrder");
        }

        /// <summary>
        /// Gets a "page" of orders from a starting order number
        /// </summary>
        /// <param name="startingOrderNumber">The Yahoo Order ID to start from</param>
        public YahooResponse GetOrderRange(IYahooStoreEntity store, long startingOrderNumber)
        {
            string body = RequestBodyIntro(store) + GetRequestBodyIntro +
                          "<OrderListQuery>" +
                          "<Filter>" +
                          "<Include>status</Include>" +
                          "</Filter>" +
                          "<QueryParams>" +
                          "<CountedRange>" +
                          $"<Start>{startingOrderNumber}</Start>" +
                          $"<Count>{OrdersPerPage}</Count>" +
                          "</CountedRange>" +
                          "</QueryParams>" +
                          "</OrderListQuery>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            return ProcessRequest(CreateOrderRequestSubmitter(store, body), "GetOrderRange");
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="itemID">The Yahoo Item ID</param>
        public YahooResponse GetItem(IYahooStoreEntity store, string itemID)
        {
            log.Debug("Building item request.");
            string body = RequestBodyIntro(store) + GetRequestBodyIntro +
                          "<CatalogQuery>" +
                          "<ItemQueryList>" +
                          "<ItemIDList>" +
                          $"<ID>{itemID}</ID>" +
                          "</ItemIDList>" +
                          "<AttributesType>all</AttributesType>" +
                          "</ItemQueryList>" +
                          "</CatalogQuery>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            return ProcessRequest(CreateCatalogRequestSubmitter(store, body), "GetItem");
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="orderID">The order's Yahoo Order ID</param>
        /// <param name="trackingNumber">The tracking number to upload</param>
        /// <param name="shipper">The shipping carrier used</param>
        public void UploadShipmentDetails(IYahooStoreEntity store, string orderID, string trackingNumber, string shipper)
        {
            string body = RequestBodyIntro(store) + UpdateRequestBodyIntro +
                          "<Order>" +
                          $"<OrderID>{orderID}</OrderID>" +
                          "<CartShipmentInfo>";

            if (!trackingNumber.IsNullOrWhiteSpace())
            {
                body += $"<TrackingNumber>{trackingNumber}</TrackingNumber>";
            }

            if (!shipper.IsNullOrWhiteSpace())
            {
                body += $"<Shipper>{shipper}</Shipper>";
            }

            body += "<ShipState>shipped</ShipState>" +
                "</CartShipmentInfo>" +
                "</Order>" +
                "</ResourceList>" +
                "</ystorewsRequest>";

            ProcessRequest(CreateOrderRequestSubmitter(store, body), "UploadShipmentDetails");
        }

        /// <summary>
        /// Uploads the order status.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        /// <param name="status">The order status to upload</param>
        public void UploadOrderStatus(IYahooStoreEntity store, string orderID, string status)
        {
            string body = RequestBodyIntro(store) +
                UpdateRequestBodyIntro +
                "<Order>" +
                $"<OrderID>{orderID}</OrderID>" +
                "<CartShipmentInfo>" +
                $"<ShipState>{status}</ShipState>" +
                "</CartShipmentInfo>" +
                "</Order>" +
                "</ResourceList>" +
                "</ystorewsRequest>";

            ProcessRequest(CreateOrderRequestSubmitter(store, body), "UploadOrderStatus");
        }

        /// <summary>
        /// Creates the order request submitter.
        /// </summary>
        /// <param name="xmlBody">The XML body</param>
        private HttpTextPostRequestSubmitter CreateOrderRequestSubmitter(IYahooStoreEntity store, string xmlBody)
        {
            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(xmlBody, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(YahooOrderEndpoint(store) + "/order")
            };

            return submitter;
        }

        /// <summary>
        /// Creates the catalog request submitter.
        /// </summary>
        /// <param name="xmlBody">The XML body.</param>
        private HttpTextPostRequestSubmitter CreateCatalogRequestSubmitter(IYahooStoreEntity store, string xmlBody)
        {
            log.Debug("Creating catalog request submitter.");
            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(xmlBody, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(YahooCatalogEndpoint(store) + "/CatalogQuery")
            };

            return submitter;
        }

        /// <summary>
        /// Get order endpoint
        /// </summary>
        private string YahooOrderEndpoint(IYahooStoreEntity store) =>
            $"https://{store.YahooStoreID}.order.store.yahooapis.com/V1";

        /// <summary>
        /// Get catalog endpoint
        /// </summary>
        private string YahooCatalogEndpoint(IYahooStoreEntity store) =>
            $"https://{store.YahooStoreID}.catalog.store.yahooapis.com/V1";

        /// <summary>
        /// Cleans the response.
        /// </summary>
        /// <param name="response">The response to clean</param>
        private static string CleanResponse(string response)
        {
            response = response.Replace("<ystorews:ystorewsResponse xmlns:ystorews=\"urn:yahoo:sbs:ystorews\" >", "<ystorewsResponse>");
            response = response.Replace("</ystorews:ystorewsResponse>", "</ystorewsResponse>");

            return response;
        }

        /// <summary>
        /// The request xml intro
        /// </summary>
        private string RequestBodyIntro(IYahooStoreEntity store) =>
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<ystorewsRequest>" +
            $"<StoreID>{store.YahooStoreID}</StoreID>" +
            "<SecurityHeader>" +
            $"<PartnerStoreContractToken>{store.AccessToken}</PartnerStoreContractToken>" +
            "</SecurityHeader>" +
            "<Version>1.0</Version>";

        /// <summary>
        /// Additional xml intro for get requests
        /// </summary>
        private string GetRequestBodyIntro => "<Verb>get</Verb>" +
                                                      "<ResourceList>";

        /// <summary>
        /// Additional xml intro for update requests
        /// </summary>
        private string UpdateRequestBodyIntro => "<Verb>update</Verb>" +
                                                      "<ResourceList>";

        /// <summary>
        /// Executes a request
        /// </summary>
        /// <param name="submitter">The request submitter.</param>
        /// <param name="action">The method calling process request for logging purposes</param>
        /// <returns></returns>
        private YahooResponse ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Yahoo, action);
                logEntry.LogRequest(submitter);

                log.Debug("Submitting request.");

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    log.Debug("Reading response.");
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    return DeserializeResponse<YahooResponse>(responseData);
                }
            }
            // Unfortunately if any error occurs on Yahoo's side, they throw 400 errors causing
            // a web exception on our side. Because of that, the error codes and messages they respond with
            // are lost. We want to capture them so we can handle the errors accordingly. So here we check if
            // the exception was a web exception. If so, dig through the exception to get Yahoo's actual response,
            // deserialize it and return a YahooResponse object containing the error code so we can deal with it later.
            catch (Exception ex)
            {
                WebException webEx = ex as WebException;

                if (webEx?.Response?.GetResponseStream() == null)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(YahooException));
                }

                using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    return DeserializeResponse<YahooResponse>(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        public static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(CleanResponse(xml));
            }
            catch (InvalidOperationException ex)
            {
                throw new YahooException($"Error deserializing {typeof(T).Name}", ex);
            }
        }
    }
}
