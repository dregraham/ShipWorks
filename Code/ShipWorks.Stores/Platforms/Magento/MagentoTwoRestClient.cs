using System;
using System.Net;
using System.Security.Cryptography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento Two REST Web Client
    /// </summary>
    [Component]
    public class MagentoTwoRestClient : IMagentoTwoRestClient
    {
        private readonly MagentoStoreEntity store;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IMagentoProductCache magentoProductCache;
        private readonly ILog log;
        private const string TokenEndpoint = "rest/V1/integration/admin/token";
        private const string OrdersEndpoint = "rest/V1/orders";
        private const string ItemsEndpoint = "rest/V1/orders/items";
        private const string ProductEndpoint = "rest/V1/products/{0}";
        private const string ShipmentEndpoint = "rest/V1/order/{0}/ship";
        private const string HoldEndpoint = "rest/V1/orders/{0}/hold";
        private const string UnholdEndpoint = "rest/V1/orders/{0}/unhold";
        private const string CancelEndpoint = "rest/V1/orders/{0}/cancel";
        private const string CommentEndpoint = "rest/V1/orders/{0}/comments";
        private const string InvoiceEndpoint = "rest/V1/order/{0}/invoice";
        private const int PageSize = 50;

        private string token;
        private readonly Uri storeUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestClient"/> class.
        /// </summary>
        public MagentoTwoRestClient(MagentoStoreEntity store, IEncryptionProviderFactory encryptionProviderFactory, IMagentoProductCache magentoProductCache, Func<Type, ILog> logFactory)
        {
            this.store = store;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.magentoProductCache = magentoProductCache;
            log = logFactory(typeof(MagentoTwoRestClient));
            storeUri = new Uri(store.ModuleUrl);
        }

        /// <summary>
        /// Magento api access token
        /// </summary>
        private string Token
        {
            get
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = GetToken();
                }
                return token;
            }
        }

        /// <summary>
        /// Get an admin token for the given credentials
        /// </summary>
        public string GetToken()
        {
            string password;
            try
            {
                password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ModuleUsername)
                    .Decrypt(store.ModulePassword);
            }
            catch (CryptographicException)
            {
                throw new MagentoException("Error decrypting Magento password");
            }

            HttpJsonVariableRequestSubmitter request = new HttpJsonVariableRequestSubmitter
            {
                Uri = new Uri($"{storeUri.AbsoluteUri}/{TokenEndpoint}"),
                Verb = HttpVerb.Post,
                RequestBody = $"{{\"username\":\"{store.ModuleUsername}\",\"password\":\"{password}\"}}"
            };

            string tokenResponse = ProcessRequest("GetToken", request);

            if (tokenResponse.IsNullOrWhiteSpace() || !tokenResponse.EndsWith("\"", StringComparison.Ordinal))
            {
                throw new MagentoException("An error occurred when communicating with Magento");
            }

            return tokenResponse.Trim('"');
        }

        /// <summary>
        /// Get orders from the given store/start date
        /// </summary>
        public OrdersResponse GetOrders(DateTime? start, int currentPage)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}"));
            AddOrdersSearchCriteria(request, start, currentPage);

            string response = ProcessRequest("GetOrders", request);
            return DeserializeResponse<OrdersResponse>(response);
        }

        /// <summary>
        /// Gets a single Magento order with detailed information (attributes)
        /// </summary>
        public Order GetOrder(long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}/{magentoOrderId}"));

            string response = ProcessRequest("GetOrder", request);
            return DeserializeResponse<Order>(response);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(string shipment, string invoice, long magentoOrderId)
        {
            string baseErrorMessage = "Error uploading shipment details.";
            string errorMessage = baseErrorMessage;

            try
            {
                UploadShipment(shipment, magentoOrderId);
            }
            catch (MagentoException ex)
            {
                errorMessage += $"\nMagento returned an error when creating the shipment: \n{ex.Message}\n";
            }

            try
            {
                UploadInvoice(invoice, magentoOrderId);
            }
            catch (MagentoException ex)
            {
                log.Error($"\nMagento returned an error when creating the invoice: \n{ex.Message}\n");
            }

            if (errorMessage != baseErrorMessage)
            {
                throw new MagentoException(errorMessage);
            }
        }

        /// <summary>
        /// Uploads the invoice.
        /// </summary>
        private void UploadInvoice(string invoice, long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(InvoiceEndpoint, magentoOrderId)}"));

            submitter.RequestBody = invoice;

            ProcessRequest("UploadInvoice", submitter);
        }

        /// <summary>
        /// Uploads the shipment.
        /// </summary>
        private void UploadShipment(string shipmentDetailsJson, long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(ShipmentEndpoint, magentoOrderId)}"));

            submitter.RequestBody = shipmentDetailsJson;

            ProcessRequest("UploadShipment", submitter);
        }

        /// <summary>
        /// Uploads comments only
        /// </summary>
        public void UploadComments(string comments, long magentoOrderID, bool commentsOnly)
        {
            string status = string.Empty;

            if (commentsOnly)
            {
                status = $", \"status\":\"{GetOrder(magentoOrderID).Status}\"";
            }

            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CommentEndpoint, magentoOrderID)}"));

            submitter.RequestBody = $"{{\"statusHistory\":{{\"comment\":\"{comments}\"{status}}}}}";

            ProcessRequest("UploadComments", submitter);
        }

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        public void HoldOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(HoldEndpoint, magentoOrderID)}"));

            ProcessRequest("HoldOrder", submitter);
        }

        /// <summary>
        /// Take hold off of a Magento order
        /// </summary>
        public void UnholdOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(UnholdEndpoint, magentoOrderID)}"));

            ProcessRequest("UnholdOrder", submitter);
        }

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        public void CancelOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CancelEndpoint, magentoOrderID)}"));

            ProcessRequest("CancelOrder", submitter);
        }

        /// <summary>
        /// Gets the specified item
        /// </summary>
        public Item GetItem(long itemID)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{ItemsEndpoint}/{itemID}"));

            string response = ProcessRequest("GetItem", request);

            return DeserializeResponse<Item>(response);
        }

        /// <summary>
        /// Gets the product for the speified sku
        /// </summary>
        public Product GetProduct(string sku)
        {
            LruCache<string, Product> productCache = magentoProductCache.GetStoreProductCache(store.StoreID);

            Product product = productCache[sku];
            if (product != null)
            {
                return productCache[sku];
            }

            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(ProductEndpoint, sku)}"));

            string response = ProcessRequest("GetProduct", request);

            product = DeserializeResponse<Product>(response);
            productCache[sku] = product;

            return product;
        }

        /// <summary>
        /// Create a request submitter with the given parameters
        /// </summary>
        private HttpJsonVariableRequestSubmitter GetRequestSubmitter(HttpVerb verb, Uri uri)
        {
            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter
            {
                Uri = uri,
                Verb = verb
            };

            submitter.Headers.Add("Authorization", $"Bearer {Token}");

            return submitter;
        }

        /// <summary>
        /// Add the order search criteria to the request
        /// </summary>
        private void AddOrdersSearchCriteria(HttpVariableRequestSubmitter request, DateTime? startDate, int currentPage)
        {
            if (startDate.HasValue)
            {
                string dateFilterPrefix = "searchCriteria[filter_groups][0][filters][0]";
                request.Variables.Add(new HttpVariable($"{dateFilterPrefix}[field]", "updated_at", false));
                request.Variables.Add(new HttpVariable($"{dateFilterPrefix}[condition_type]", "gt", false));
                request.Variables.Add(new HttpVariable($"{dateFilterPrefix}[value]", $"{startDate:yyyy-MM-dd HH:mm:ss}", false));
            }

            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][field]", "updated_at", false));
            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][direction]", "asc", false));
            request.Variables.Add(new HttpVariable("searchCriteria[pageSize]", PageSize.ToString(), false));
            request.Variables.Add(new HttpVariable("searchCriteria[currentPage]", currentPage.ToString(), false));
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private string ProcessRequest(string action, HttpVariableRequestSubmitter request)
        {
            ApiLogEntry apiLogEntry = new ApiLogEntry(ApiLogSource.Magento, action);
            apiLogEntry.LogRequest(request);

            try
            {
                request.AllowAutoRedirect = false;
                request.AllowHttpStatusCodes(HttpStatusCode.BadRequest);
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                ValidateResult(httpResponseReader, result);

                return result;
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new MagentoException(ex);
            }
        }

        /// <summary>
        /// Validates the result.
        /// </summary>
        private static void ValidateResult(IHttpResponseReader httpResponseReader, string result)
        {
            if (httpResponseReader.HttpWebResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                JObject badResult = JObject.Parse(result);
                string message = (string) badResult["message"];
                throw new MagentoException(message ?? "An error occurred when communicating with Magento");
            }
        }

        /// <summary>
        /// Deserializes the response
        /// </summary>
        private T DeserializeResponse<T>(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception ex)
            {
                throw new MagentoException($"Failed to deserialize {typeof(T)}", ex);
            }
        }
    }
}