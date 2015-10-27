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

        public YahooApiWebClient(YahooStoreEntity store)
        {
            yahooStoreID = store.YahooStoreID;
            token = store.AccessToken;
            yahooOrderEndpoint = $"https://{yahooStoreID}.order.store.yahooapis.com/V1";
            yahooCatalogEndpoint = $"https://{yahooStoreID}.catalog.store.yahooapis.com/V1";
        }

        public string GetOrder(string orderID)
        {
            return ProcessRequest(CreateGetOrderRequest(orderID), "GetOrder");
        }

        public string ValidateCredentials()
        {
            return ProcessRequest(CreateGetItemRangeRequest(1, 2, "keyword"), "GetItemRange");
        }

        /// <summary>
        ///     Setup a get request.
        /// </summary>
        private HttpTextPostRequestSubmitter CreateGetOrderRequest(string orderID)
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml");
            submitter.Verb = HttpVerb.Post;
            submitter.Uri = new Uri(yahooOrderEndpoint + "/order");
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

            HttpTextPostRequestSubmitter submitter = new HttpTextPostRequestSubmitter(body, "xml");
            submitter.Verb = HttpVerb.Post;
            submitter.Uri = new Uri(yahooCatalogEndpoint + "/CatalogQuery");
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
