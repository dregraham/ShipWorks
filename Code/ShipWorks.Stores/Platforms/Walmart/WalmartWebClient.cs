using System;
using System.Collections.Generic;
using System.Globalization;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Web client for interacting with Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Walmart.IWalmartWebClient" />
    [Component]
    public class WalmartWebClient : IWalmartWebClient
    {
        private readonly IWalmartRequestSigner requestSigner;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";
        private const string GetOrdersUrl = "https://marketplace.walmartapis.com/v3/orders";
        private const string AcknowledgeOrderUrl = "https://marketplace.walmartapis.com/v3/orders/{0}/acknowledge";

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartWebClient"/> class.
        /// </summary>
        public WalmartWebClient(IWalmartRequestSigner requestSigner, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.requestSigner = requestSigner;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Tests the connection to Walmart, throws if invalid credentials
        /// </summary>
        public void TestConnection(WalmartStoreEntity store)
        {
            HttpXmlVariableRequestSubmitter requestSubmitter = new HttpXmlVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(TestConnectionUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            ProcessRequest(store, requestSubmitter, "TestConnection");
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private string ProcessRequest(WalmartStoreEntity store, HttpRequestSubmitter submitter, string action)
        {
            submitter.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
            submitter.Headers.Add("WM_CONSUMER.ID", store.ConsumerID);
            submitter.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", store.ChannelType);
            submitter.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());

            string epoch = (DateTimeUtility.ToUnixTimestamp(DateTime.UtcNow) * 1000).ToString(CultureInfo.InvariantCulture);

            requestSigner.Sign(submitter, store, epoch);

            try
            {
                IApiLogEntry logEntry = apiLogEntryFactory(ApiLogSource.Walmart, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "xml");
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(WalmartException));
            }
        }

        private T ProcessRequest<T>(WalmartStoreEntity store, HttpRequestSubmitter submitter, string action)
        {
            string response = ProcessRequest(store, submitter, action);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(new StringReader(response));
            return (T) serializer.Deserialize(reader);
        }

        /// <summary>
        /// Get orders from the given start date
        /// </summary>
        public ordersListType GetOrders(WalmartStoreEntity store, DateTime start)
        {
            HttpXmlVariableRequestSubmitter requestSubmitter = new HttpXmlVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(GetOrdersUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            requestSubmitter.Variables.Add("createdStartDate", start.ToString("s"));

            ordersListType ordersResponse = ProcessRequest<ordersListType>(store, requestSubmitter, "GetOrders");

            foreach (Order order in ordersResponse.elements)
            {
                AcknowledgeOrder(store, order.purchaseOrderId);
            }

            return ordersResponse;
        }

        /// <summary>
        /// Acknowledge the given purchase order
        /// </summary>
        private void AcknowledgeOrder(WalmartStoreEntity store, string purchaseOrderId)
        {
            HttpXmlVariableRequestSubmitter requestSubmitter = new HttpXmlVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(string.Format(AcknowledgeOrderUrl, purchaseOrderId));
            requestSubmitter.Verb = HttpVerb.Post;

            ProcessRequest(store, requestSubmitter, "AcknowledgeOrder");
        }

        public ordersListType GetOrders(WalmartStoreEntity store, string nextCursor)
        {
            throw new NotImplementedException();
        }
    }
}