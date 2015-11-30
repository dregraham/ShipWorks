using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiWebClient : IYahooApiWebClient
    {
        private string yahooStoreID;
        private string token;
        private string yahooOrderEndpoint;
        private string yahooCatalogEndpoint;
        private const int OrdersPerPage = 50;

        public YahooApiWebClient(YahooStoreEntity store)
        {
            yahooStoreID = store.YahooStoreID;
            token = store.AccessToken;
            yahooOrderEndpoint = $"https://{yahooStoreID}.order.store.yahooapis.com/V1";
            yahooCatalogEndpoint = $"https://{yahooStoreID}.catalog.store.yahooapis.com/V1";
        }

        public YahooResponse GetOrder(long orderID)
        {
            return ProcessRequest(CreateGetOrderRequest(orderID), "GetOrder");
        }

        public YahooResponse GetOrderRange(long start)
        {
            return ProcessRequest(CreateGetOrderRangeRequest(start), "GetOrderRange");
        }

        public YahooResponse GetItem(string itemID)
        {
            return ProcessRequest(CreateGetItemRequest(itemID), "GetItem");
        }

        public YahooResponse ValidateCredentials()
        {
            return ProcessRequest(CreateGetItemRangeRequest(1, 2, "keyword"), "GetItemRange");
        }

        public void UploadShipmentDetails(string orderID, string trackingNumber, string shipper, string status)
        {
            ProcessRequest(CreateUpdateShipmentDetailsRequest(orderID, trackingNumber, shipper, status), "UploadShipmentDetails");
        }

        public void UploadOrderStatus(string orderID, string status)
        {
            ProcessRequest(CreateUploadOrderStatusRequest(orderID, status), "UploadOrderStatus");
        }

        private static string CleanResponse(string response)
        {
            response = response.Replace("<ystorews:ystorewsResponse xmlns:ystorews=\"urn:yahoo:sbs:ystorews\" >", "<ystorewsResponse>");
            response = response.Replace("</ystorews:ystorewsResponse>", "</ystorewsResponse>");

            return response;
        }

        /// <summary>
        ///     Setup a get request.
        /// </summary>
        private HttpTextPostRequestSubmitter CreateGetOrderRequest(long orderID)
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooOrderEndpoint + "/order")
            };
            return submitter;
        }

        private HttpTextPostRequestSubmitter CreateGetOrderRangeRequest(long start)
        {
            string body = RequestBodyIntro + GetRequestBodyIntro +
                          "<OrderListQuery>" +
                          "<Filter>" +
                          "<Include>status</Include>" +
                          "</Filter>" +
                          "<QueryParams>" +
                          "<CountedRange>" +
                          $"<Start>{start}</Start>" +
                          $"<Count>{OrdersPerPage}</Count>" +
                          "</CountedRange>" +
                          "</QueryParams>" +
                          "</OrderListQuery>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooOrderEndpoint + "/order")
            };

            return submitter;
        }

        /// <summary>
        ///     Setup a get request.
        /// </summary>
        private HttpTextPostRequestSubmitter CreateGetItemRangeRequest(int start, int end, string keyword)
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooCatalogEndpoint + "/CatalogQuery")
            };

            return submitter;
        }

        private HttpTextPostRequestSubmitter CreateGetItemRequest(string itemID)
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooCatalogEndpoint + "/CatalogQuery")
            };

            return submitter;
        }

        private HttpTextPostRequestSubmitter CreateUpdateShipmentDetailsRequest(string orderID, string trackingNumber, string shipper, string status)
        {
            string body = RequestBodyIntro + UpdateRequestBodyIntro +
                          "<Order>" +
                          $"<OrderID>{orderID}</OrderID>" +
                          "<CartShipmentInfo>" +
                          $"<TrackingNumber>{trackingNumber}</TrackingNumber>" +
                          $"<Shipper>{shipper}</Shipper>" +
                          $"<ShipState>{status}</ShipState>" +
                          "</CartShipmentInfo>" +
                          "</Order>" +
                          "</ResourceList>" +
                          "</ystorewsRequest>";

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooOrderEndpoint + "/order")
            };

            return submitter;
        }

        private HttpRequestSubmitter CreateUploadOrderStatusRequest(string orderID, string status)
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml")
            {
                Verb = HttpVerb.Post,
                Uri = new Uri(yahooOrderEndpoint + "/order")
            };

            return submitter;
        }

        private string RequestBodyIntro => "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                           "<ystorewsRequest>" +
                                           $"<StoreID>{yahooStoreID}</StoreID>" +
                                           "<SecurityHeader>" +
                                           $"<PartnerStoreContractToken>{token}</PartnerStoreContractToken>" +
                                           "</SecurityHeader>" +
                                           "<Version>1.0</Version>";

        private string GetRequestBodyIntro => "<Verb>get</Verb>" +
                                              "<ResourceList>";

        private string UpdateRequestBodyIntro => "<Verb>update</Verb>" +
                                              "<ResourceList>";

        /// <summary>
        /// Executes a request
        /// </summary>
        /// <param name="submitter">The submitter.</param>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        private static YahooResponse ProcessRequest(HttpRequestSubmitter submitter, string action)
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
                if (!xml.Contains("ErrorResourceList"))
                {
                    YahooResponse errorResponse = SerializationUtility.DeserializeFromXml<YahooResponse>(xml);

                    foreach (YahooError error in errorResponse.ErrorResourceList.Error)
                    {
                        throw new YahooException(error.Message, ex);
                    }
                }

                throw new YahooException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }
    }
}
