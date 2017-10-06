using System;
using System.ComponentModel;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Linq;
using System.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

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
        private const string ChannelType = "a7a7db08-682f-488a-a005-921af89d7e9b";
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";
        private const string GetOrdersUrl = "https://marketplace.walmartapis.com/v3/orders";
        private const string GetOrderUrl = "https://marketplace.walmartapis.com/v3/orders/{0}";
        private const string AcknowledgeOrderUrl = "https://marketplace.walmartapis.com/v3/orders/{0}/acknowledge";
        private const int DownloadOrderCountLimit = 200;
        private const string UpdateShipmentUrl =
            "https://marketplace.walmartapis.com/v3/orders/{0}/shipping";

        private const string BaseErrorMessage = "ShipWorks encountered an error communicating with Walmart";


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
        public void TestConnection(IWalmartStoreEntity store)
        {
            IHttpRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(TestConnectionUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            ProcessRequest<string>(store, requestSubmitter, "TestConnection");
        }

        /// <summary>
        /// Get the given PurchaseOrderId
        /// </summary>
        public Order GetOrder(IWalmartStoreEntity store, string purchaseOrderId)
        {
            IHttpVariableRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(string.Format(GetOrderUrl, purchaseOrderId));
            requestSubmitter.Verb = HttpVerb.Get;

            return ProcessRequest<Order>(store, requestSubmitter, $"GetOrder {purchaseOrderId}");
        }

        /// <summary>
        /// Get orders from the given start date
        /// </summary>
        public ordersListType GetOrders(IWalmartStoreEntity store, DateTime start)
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
        public ordersListType GetOrders(IWalmartStoreEntity store, string nextCursor)
        {
            IHttpVariableRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri($"{GetOrdersUrl}{nextCursor}");

            return GetOrders(store, requestSubmitter);
        }

        /// <summary>
        /// Gets orders using the given request submitter
        /// </summary>
        private ordersListType GetOrders(IWalmartStoreEntity store, IHttpRequestSubmitter requestSubmitter)
        {
            requestSubmitter.Verb = HttpVerb.Get;

            ordersListType ordersResponse = ProcessRequest<ordersListType>(store, requestSubmitter, "GetOrders");
            AcknowledgeOrders(store, ordersResponse);

            return ordersResponse;
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public Order UpdateShipmentDetails(IWalmartStoreEntity store, orderShipment shipment, string purchaseOrderID)
        {
            string serializedShipment = SerializationUtility.SerializeToXml(shipment, true);

            IHttpRequestSubmitter requestSubmitter =
                httpRequestSubmitterFactory.GetHttpTextPostRequestSubmitter(serializedShipment, @"application/xml");

            requestSubmitter.Uri = new Uri(string.Format(UpdateShipmentUrl, purchaseOrderID));
            requestSubmitter.Verb = HttpVerb.Post;

            return ProcessRequest<Order>(store, requestSubmitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Acknowledge the given purchase order
        /// </summary>
        private void AcknowledgeOrders(IWalmartStoreEntity store, ordersListType ordersResponse)
        {
            for (int i = 0; i < ordersResponse.elements.Length; i++)
            {
                Order order = ordersResponse.elements[i];
                if (order.orderLines.Any(oi => oi.orderLineStatuses.Any(ols => ols.status == orderLineStatusValueType.Created)))
                {
                    IHttpRequestSubmitter requestSubmitter = httpRequestSubmitterFactory.GetHttpTextPostRequestSubmitter(string.Empty, "application/xml");
                    requestSubmitter.Uri = new Uri(string.Format(AcknowledgeOrderUrl, order.purchaseOrderId));
                    requestSubmitter.Verb = HttpVerb.Post;

                    Order acknowledgedOrder = ProcessRequest<Order>(store, requestSubmitter, "AcknowledgeOrder");
                    ordersResponse.elements[i] = acknowledgedOrder;
                }
            }
        }

        /// <summary>
        /// Process the request and deserialize the response
        /// </summary>
        private T ProcessRequest<T>(IWalmartStoreEntity store, IHttpRequestSubmitter submitter, string action)
        {
            string response = string.Empty;
            try
            {
                submitter.AllowHttpStatusCodes(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized);
                submitter.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
                submitter.Headers.Add("WM_CONSUMER.ID", store.ConsumerID);
                submitter.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", ChannelType);
                submitter.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());

                requestSigner.Sign(submitter, store);

                IApiLogEntry logEntry = apiLogEntryFactory(ApiLogSource.Walmart, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader responseReader = submitter.GetResponse())
                {
                    response = responseReader.ReadResult();
                    logEntry.LogResponse(response, "xml");

                    // The reason we allow 400 and 401 above but then throw for everything that is not 200 here is
                    // because we need to retain both the error DTO from Walmart as well as the HTTP status code
                    // description. If we didn't allow them, we'd lose the Walmart message.
                    if (responseReader.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException($"{(int)responseReader.HttpWebResponse.StatusCode} {responseReader.HttpWebResponse.StatusDescription}");
                    }

                    return DeserializeResponse<T>(response);
                }
            }
            catch (Exception ex) when (ex.GetType() == typeof(WebException) ||
                                       ex.GetType() == typeof(InvalidOperationException) ||
                                       ex.GetType() == typeof(ArgumentNullException))
            {
                throw new WalmartException(GetErrorMessage(response, ex.Message), ex);
            }
        }

        /// <summary>
        /// Deserializes the response.
        /// </summary>
        private static T DeserializeResponse<T>(string response)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader xmlReader = XmlReader.Create(new StringReader(response));

            return typeof(T) == typeof(string) ?
                (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(response):
                (T) serializer.Deserialize(xmlReader);
        }

        /// <summary>
        /// Gets the error message to display to the user
        /// </summary>
        /// <remarks>
        /// Attempts to extract the message from the walmart error dto, if that fails, return the original exception message.
        /// </remarks>
        private string GetErrorMessage(string response, string exceptionMessage)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Errors));
                XmlReader reader = XmlReader.Create(new StringReader(response));

                Errors errors = (Errors) serializer.Deserialize(reader);

                // First check to see if it is the error we see during walmart outages, see TP#19684 or RNT 170907-003474
                if (errors.error.Length == 1)
                {
                    error error = errors.error[0];

                    if (error.code == "INVALID_REQUEST.GMP_ORDER_API" &&
                        error.field == "data" &&
                        error.description == "Invalid Request" &&
                        error.info == "Request invalid." &&
                        error.severity == errorSeverity.ERROR &&
                        error.category == errorCategory.DATA &&
                        error.causes?.Length == 0)
                    {
                        // Since walmart gives us no reason for the error, just tell the customer to try again later
                        return $"{BaseErrorMessage}. Please try again later. {exceptionMessage}";
                    }
                }

                // Exception message + Walmart message
                return $"{BaseErrorMessage}:{Environment.NewLine}{exceptionMessage}{Environment.NewLine}" +
                       $"{string.Join(", ", errors.error.Select(e => e.description).Distinct())}";
            }
            catch (Exception ex) when (ex.GetType() == typeof(InvalidOperationException) ||
                                       ex.GetType() == typeof(ArgumentNullException))
            {
                // If there was an error deserializing the walmart error response, just return the original exception message
                return $"{BaseErrorMessage}: {Environment.NewLine}{exceptionMessage}";
            }
        }
    }
}