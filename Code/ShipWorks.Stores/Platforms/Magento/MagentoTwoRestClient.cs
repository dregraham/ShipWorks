using System;
using System.Security.Cryptography;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento Two REST Web Client
    /// </summary>
    public class MagentoTwoRestClient : IMagentoTwoRestClient
    {
        private readonly MagentoStoreEntity store;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private const string TokenEndpoint = "rest/V1/integration/admin/token";
        private const string OrdersEndpoint = "rest/V1/orders";
        private const string ShipmentEndpoint = "rest/V1/order/{0}/ship";
        private const string HoldEndpoint = "rest/V1/orders/{0}/hold";
        private const string UnholdEndpoint = "rest/V1/orders/{0}/unhold";
        private const string CancelEndpoint = "rest/V1/orders/{0}/cancel";
        private const string CommentEndpoint = "rest/V1/orders/{0}/comments";
        private const string InvoiceEndpoint = "rest/V1/invoices";
        private const int PageSize = 5;

        private string token;
        private readonly Uri storeUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestClient"/> class.
        /// </summary>
        public MagentoTwoRestClient(MagentoStoreEntity store, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.store = store;
            this.encryptionProviderFactory = encryptionProviderFactory;
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

            return ProcessRequest(request).Trim('"');
        }

        /// <summary>
        /// Get orders from the given store/start date
        /// </summary>
        public IOrdersResponse GetOrders(DateTime? start, int currentPage)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}"));
            AddOrdersSearchCriteria(request, start, currentPage);

            string response = ProcessRequest(request);
            return DeserializeResponse<IOrdersResponse, OrdersResponse, DTO.MagentoTwoDotZero.OrdersResponse>(response);
        }

        public IOrder GetOrder(long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}/{magentoOrderId}"));

            string response = ProcessRequest(request);
            return DeserializeResponse<IOrder, Order, DTO.MagentoTwoDotZero.Order>(response);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(string shipmentDetailsJson, string invoice, long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(ShipmentEndpoint, magentoOrderId)}"));

            submitter.RequestBody = shipmentDetailsJson;

            ProcessRequest(submitter);

            submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{InvoiceEndpoint}"));

            submitter.RequestBody = invoice;

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Uploads comments only
        /// </summary>
        public void UploadComments(string comments, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CommentEndpoint, magentoOrderID)}"));

            submitter.RequestBody = $"\"comments\":\"{comments}\"";

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        public void HoldOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(HoldEndpoint, magentoOrderID)}"));

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Take hold off of a Magento order
        /// </summary>
        public void UnholdOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(UnholdEndpoint, magentoOrderID)}"));

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        public void CancelOrder(long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CancelEndpoint, magentoOrderID)}"));

            ProcessRequest(submitter);
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
                request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][field]",
                    "updated_at", false));
                request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][condition_type]",
                    "gt", false));
                request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][value]",
                    $"{startDate:yyyy-MM-dd HH:mm:ss}", false));
            }

            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][field]", "updated_at", false));
            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][direction]", "asc", false));
            request.Variables.Add(new HttpVariable("searchCriteria[pageSize]", PageSize.ToString(), false));
            request.Variables.Add(new HttpVariable("searchCriteria[currentPage]", currentPage.ToString(), false));
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private string ProcessRequest(HttpVariableRequestSubmitter request)
        {
            try
            {
                return request.GetResponse().ReadResult();
            }
            catch (Exception ex)
            {
                throw new MagentoException(ex);
            }
        }

        /// <summary>
        /// Deserializes the response
        /// </summary>
        /// <typeparam name="TCommonInterface">The interface being returned.</typeparam>
        /// <typeparam name="TFirstTypeToTry">An Implementation of TCommonInterface. We attempt to deserialize the string as this type first.</typeparam>
        /// <typeparam name="TSecondTypeToTry">
        /// An Implementation of TCommonInterface.
        /// If deserialization to the first type fails, we attempt to deserialize the string as this type.
        /// </typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The deserialized string as either TfirstTypeToTry or TSecondTypeToTry if the first fails</returns>
        private TCommonInterface DeserializeResponse<TCommonInterface, TFirstTypeToTry, TSecondTypeToTry>(string response)
            where TFirstTypeToTry : TCommonInterface where TSecondTypeToTry : TCommonInterface
        {
            try
            {
                return JsonConvert.DeserializeObject<TFirstTypeToTry>(response);
            }
            catch (Exception ex)
            {
                try
                {
                    return JsonConvert.DeserializeObject<TSecondTypeToTry>(response);
                }
                catch (Exception)
                {
                    throw new MagentoException($"Failed to deserialize {typeof(TCommonInterface)}", ex);
                }
            }
        }
    }
}