using System;
using System.IO;
using System.Net;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Quartz.Util;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Class for interacting with the yahoo merchant API
    /// </summary>
    public class YahooApiWebClient : IYahooApiWebClient
    {
        private readonly string yahooStoreID;
        private readonly string token;
        private readonly string yahooOrderEndpoint;
        private readonly string yahooCatalogEndpoint;
        private const int OrdersPerPage = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiWebClient"/> class.
        /// </summary>
        /// <param name="store">The Yahoo Store Entity</param>
        public YahooApiWebClient(YahooStoreEntity store)
        {
            yahooStoreID = store.YahooStoreID;
            token = store.AccessToken;
            yahooOrderEndpoint = $"https://{yahooStoreID}.order.store.yahooapis.com/V1";
            yahooCatalogEndpoint = $"https://{yahooStoreID}.catalog.store.yahooapis.com/V1";
        }

        /// <summary>
        /// Validates the credentials.
        /// </summary>
        public YahooResponse ValidateCredentials()
        {
            return ProcessRequest(CreateSearchItemRangeRequest(1, 2, "keyword"), "GetItemRange");
        }

        /// <summary>
        /// Gets an order.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        public YahooResponse GetOrder(long orderID)
        {
            string body = RequestBodyIntro + GetRequestBodyIntro +
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

            return ProcessRequest(CreateOrderRequestSubmitter(body), "GetOrder");
        }

        /// <summary>
        /// Gets a "page" of orders from a starting order number
        /// </summary>
        /// <param name="start">The Yahoo Order ID to start from</param>
        public YahooResponse GetOrderRange(long startingOrderNumber)
        {
            string body = RequestBodyIntro + GetRequestBodyIntro +
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

            return ProcessRequest(CreateOrderRequestSubmitter(body), "GetOrderRange");
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="itemID">The Yahoo Item ID</param>
        public YahooResponse GetItem(string itemID)
        {
            string body = RequestBodyIntro + GetRequestBodyIntro +
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

            return ProcessRequest(CreateCatalogRequestSubmitter(body), "GetItem");
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="orderID">The order's Yahoo Order ID</param>
        /// <param name="trackingNumber">The tracking number to upload</param>
        /// <param name="shipper">The shipping carrier used</param>
        /// <param name="status">The order status to upload</param>
        public void UploadShipmentDetails(string orderID, string trackingNumber, string shipper, string status)
        {
            string body = RequestBodyIntro + UpdateRequestBodyIntro +
                          "<Order>" +
                          $"<OrderID>{orderID}</OrderID>" +
                          "<CartShipmentInfo>" +
                          $"<TrackingNumber>{trackingNumber}</TrackingNumber>";

            if (!shipper.IsNullOrWhiteSpace())
            {
                body += $"<Shipper>{shipper}</Shipper>";
            }

            body += $"<ShipState>{status}</ShipState>" +
                "</CartShipmentInfo>" +
                "</Order>" +
                "</ResourceList>" +
                "</ystorewsRequest>";

            ProcessRequest(CreateOrderRequestSubmitter(body), "UploadShipmentDetails");
        }

        /// <summary>
        /// Uploads the order status.
        /// </summary>
        /// <param name="orderID">The Yahoo Order ID</param>
        /// <param name="status">The order status to upload</param>
        public void UploadOrderStatus(string orderID, string status)
        {
            string body = RequestBodyIntro + UpdateRequestBodyIntro +
                          "<Order>" +
                          $"<OrderID>{orderID}</OrderID>" +
                          "<CartShipmentInfo>" +
                          $"<ShipState>{status}</ShipState>" +
                          "</CartShipmentInfo>" +
                          "</Order>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            ProcessRequest(CreateOrderRequestSubmitter(body), "UploadOrderStatus");
        }

        /// <summary>
        /// Creates a search item range request. Currently only used to validate credentials
        /// because it doesn't throw an error for an invalid start and end range like getting a
        /// range of orders does.
        /// </summary>
        /// <param name="start">The starting Yahoo Item ID.</param>
        /// <param name="end">The ending Yahoo Item ID</param>
        /// <param name="keyword">The keyword to search for</param>
        /// <returns></returns>
        private HttpTextPostRequestSubmitter CreateSearchItemRangeRequest(int start, int end, string keyword)
        {
            string body = RequestBodyIntro + GetRequestBodyIntro +
                "<CatalogQuery>" +
                "<SimpleSearch>" +
                $"<StartIndex>{start}</StartIndex>" +
                $"<EndIndex>{end}</EndIndex>" +
                $"<Keyword>{keyword}</Keyword>" +
                "</SimpleSearch>" +
                "</CatalogQuery>" +
                "</ResourceList>" +
                "</ystorewsRequest>";

            return CreateCatalogRequestSubmitter(body);
        }

        /// <summary>
        /// Creates the order request submitter.
        /// </summary>
        /// <param name="xmlBody">The XML body</param>
        private HttpTextPostRequestSubmitter CreateOrderRequestSubmitter(string xmlBody)
        {
            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(xmlBody, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooOrderEndpoint + "/order")
            };

            return submitter;
        }

        /// <summary>
        /// Creates the catalog request submitter.
        /// </summary>
        /// <param name="xmlBody">The XML body.</param>
        private HttpTextPostRequestSubmitter CreateCatalogRequestSubmitter(string xmlBody)
        {
            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(xmlBody, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooCatalogEndpoint + "/CatalogQuery")
            };

            return submitter;
        }

        /// <summary>
        /// Cleans the response.
        /// </summary>
        /// <param name="response">The response to clean</param>
        private string CleanResponse(string response)
        {
            response = response.Replace("<ystorews:ystorewsResponse xmlns:ystorews=\"urn:yahoo:sbs:ystorews\" >", "<ystorewsResponse>");
            response = response.Replace("</ystorews:ystorewsResponse>", "</ystorewsResponse>");

            return response;
        }

        /// <summary>
        /// The request xml intro
        /// </summary>
        private string RequestBodyIntro => "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                           "<ystorewsRequest>" +
                                           $"<StoreID>{yahooStoreID}</StoreID>" +
                                           "<SecurityHeader>" +
                                           $"<PartnerStoreContractToken>{token}</PartnerStoreContractToken>" +
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

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    return DeserializeResponse<YahooResponse>(CleanResponse(responseData));
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
                    throw WebHelper.TranslateWebException(ex, typeof (YahooException));
                }

                using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    return DeserializeResponse<YahooResponse>(CleanResponse(reader.ReadToEnd()));
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
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (InvalidOperationException ex)
            {
                throw new YahooException($"Error deserializing {typeof(T).Name}", ex);
            }
        }
    }
}
