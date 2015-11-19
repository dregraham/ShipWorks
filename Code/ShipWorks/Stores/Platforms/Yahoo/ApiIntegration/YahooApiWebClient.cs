using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiWebClient
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

        public string GetOrder(long orderID)
        {
            return CleanResponse(ProcessRequest(CreateGetOrderRequest(orderID), "GetOrder"));
        }

        public string GetOrderRange(long start)
        {
            return CleanResponse(ProcessRequest(CreateGetOrderRangeRequest(start), "GetOrderRange"));
        }

        public string GetItem(string itemID)
        {
            return CleanResponse(ProcessRequest(CreateGetItemRequest(itemID), "GetItem"));
        }

        public string ValidateCredentials()
        {
            return CleanResponse(ProcessRequest(CreateGetItemRangeRequest(1, 2, "keyword"), "GetItemRange"));
        }

        private string CleanResponse(string response)
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
            string body = GetRequestBodyIntro +
                "<OrderListQuery>" +
                "<Filter>" +
                $"<Include>all</Include>" +
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
            long end = start + OrdersPerPage;
            string body = GetRequestBodyIntro +
                          "<OrderListQuery>" +
                          "<Filter>" +
                          "<Include>status</Include>" +
                          "</Filter>" +
                          "<QueryParams>" +
                          "<IntervalRange>" +
                          $"<Start>{start}</Start>" +
                          $"<End>{end}</End>" +
                          "</IntervalRange>" +
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
            string body = GetRequestBodyIntro +
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
            string body = GetRequestBodyIntro +
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

        private string GetRequestBodyIntro => "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                              "<ystorewsRequest>" +
                                              $"<StoreID>{yahooStoreID}</StoreID>" +
                                              "<SecurityHeader>" +
                                              $"<PartnerStoreContractToken>{token}</PartnerStoreContractToken>" +
                                              "</SecurityHeader>" +
                                              "<Version>1.0</Version>" +
                                              $"<Verb>get</Verb>" +
                                              "<ResourceList>";



        /// <summary>
        ///     Executes a request
        /// </summary>
        private static string ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Yahoo, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");
                    
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(YahooException));
            }
        }
    }
}
