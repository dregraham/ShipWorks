using System;
using System.Collections.Generic;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Linq;
using Interapptive.Shared.Utility;

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
        private readonly IHttpRequestSubmitterFactory httpRequestSubmitterFactory;
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";
        private const string GetOrdersUrl = "https://marketplace.walmartapis.com/v3/orders";
        private const string AcknowledgeOrderUrl = "https://marketplace.walmartapis.com/v3/orders/{0}/acknowledge";
        private const int DownloadOrderCountLimit = 200;
        private const string UpdateShipmentUrl =
            "https://marketplace.walmartapis.com/v3/orders/{0}/shipping";


        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartWebClient"/> class.
        /// </summary>
        public WalmartWebClient(IWalmartRequestSigner requestSigner,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
           IHttpRequestSubmitterFactory httpRequestSubmitterFactory)
        {
            this.requestSigner = requestSigner;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.httpRequestSubmitterFactory = httpRequestSubmitterFactory;
        }

        /// <summary>
        /// Tests the connection to Walmart, throws if invalid credentials
        /// </summary>
        public void TestConnection(WalmartStoreEntity store)
        {
            IHttpRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(TestConnectionUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            ProcessRequest(store, requestSubmitter, "TestConnection");
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private string ProcessRequest(WalmartStoreEntity store, IHttpRequestSubmitter submitter, string action)
        {
            submitter.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
            submitter.Headers.Add("WM_CONSUMER.ID", store.ConsumerID);
            submitter.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", store.ChannelType);
            submitter.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());
            
            requestSigner.Sign(submitter, store);

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

        /// <summary>
        /// Process the request and deserialize the response
        /// </summary>
        private T ProcessRequest<T>(WalmartStoreEntity store, IHttpRequestSubmitter submitter, string action)
        {
            try
            {
                string response = ProcessRequest(store, submitter, action);

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlReader reader = XmlReader.Create(new StringReader(response));
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex) when (ex.GetType() != typeof(WalmartException))
            {
                throw new WalmartException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get orders from the given start date
        /// </summary>
        public ordersListType GetOrders(WalmartStoreEntity store, DateTime start)
        {
            IHttpVariableRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(GetOrdersUrl);
            requestSubmitter.Variables.Add("createdStartDate", start.ToString("s"));
            requestSubmitter.Variables.Add("limit", DownloadOrderCountLimit.ToString());

            return GetOrders(store, requestSubmitter);
        }

        /// <summary>
        /// Get orders using the next cursor token
        /// </summary>
        public ordersListType GetOrders(WalmartStoreEntity store, string nextCursor)
        {
            IHttpVariableRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri($"{GetOrdersUrl}{nextCursor}");

            return GetOrders(store, requestSubmitter);
        }

        /// <summary>
        /// Gets orders using the given request submitter
        /// </summary>
        private ordersListType GetOrders(WalmartStoreEntity store, IHttpRequestSubmitter requestSubmitter)
        {
            requestSubmitter.Verb = HttpVerb.Get;

            ordersListType ordersResponse = ProcessRequest<ordersListType>(store, requestSubmitter, "GetOrders");
            AcknowledgeOrders(store, ordersResponse);

            return ordersResponse;
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UpdateShipmentDetails(WalmartStoreEntity store, orderShipment shipment, string purchaseOrderID)
        {
            string serializedShipment = SerializationUtility.SerializeToXml(shipment, false);

            IHttpRequestSubmitter requestSubmitter =
                httpRequestSubmitterFactory.GetHttpTextPostRequestSubmitter(serializedShipment, @"application/xml");

            requestSubmitter.Uri = new Uri(string.Format(UpdateShipmentUrl, purchaseOrderID));
            requestSubmitter.Verb = HttpVerb.Post;

            ProcessRequest(store, requestSubmitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Acknowledge the given purchase order
        /// </summary>
        private void AcknowledgeOrders(WalmartStoreEntity store, ordersListType ordersResponse)
        {
            IEnumerable<Order> ordersToAcknowledge = ordersResponse.elements
                .Where(o => o.orderLines.Any(oi => oi.orderLineStatuses.Any(ols => ols.status == orderLineStatusValueType.Created)));

            foreach (Order order in ordersToAcknowledge)
            {
                IHttpRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpTextPostRequestSubmitter(string.Empty, "application/xml");
                requestSubmitter.Uri = new Uri(string.Format(AcknowledgeOrderUrl, order.purchaseOrderId));
                requestSubmitter.Verb = HttpVerb.Post;

                ProcessRequest(store, requestSubmitter, "AcknowledgeOrder");
            }
        }
    }
}