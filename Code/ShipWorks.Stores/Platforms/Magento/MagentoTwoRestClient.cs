using System;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento Two REST Web Client
    /// </summary>
    public class MagentoTwoRestClient : IMagentoTwoRestClient
    {
        private const string TokenEndpoint = "rest/V1/integration/admin/token";
        private const string OrdersEndpoint = "rest/V1/orders";
        private const string ShipmentEndpoint = "rest/V1/order/{0}/ship";
        private const string HoldEndpoint = "rest/V1/orders/{0}/hold";
        private const string CancelEndpoint = "rest/V1/orders/{0}/cancel";

        /// <summary>
        /// Get an admin token for the given credentials
        /// </summary>
        public string GetToken(Uri storeUri, string username, string password)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{TokenEndpoint}"));
            request.RequestBody = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            return ProcessRequest<string>(request);
        }

        /// <summary>
        /// Get orders from the given store/start date
        /// </summary>
        public OrdersResponse GetOrders(DateTime start, Uri storeUri, string token)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}"), token);
            AddOrdersSearchCriteria(request, start);

            return ProcessRequest<OrdersResponse>(request);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        /// <param name="shipmentDetailsJson">The shipment details.</param>
        /// <param name="storeUri">The store URI.</param>
        /// <param name="token">The token.</param>
        /// <param name="magentoOrderId"></param>
        public void UploadShipmentDetails(string shipmentDetailsJson, Uri storeUri, string token, long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(ShipmentEndpoint, magentoOrderId)}"), token);

            submitter.RequestBody = shipmentDetailsJson;

            ProcessRequest<string>(submitter);
        }

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        public void HoldOrder(Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(HoldEndpoint, magentoOrderID)}"), token);

            ProcessRequest<string>(submitter);
        }

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        public void CancelOrder(Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CancelEndpoint, magentoOrderID)}"), token);

            ProcessRequest<string>(submitter);
        }

        /// <summary>
        /// Create a request submitter with the given parameters
        /// </summary>
        private HttpJsonVariableRequestSubmitter GetRequestSubmitter(HttpVerb verb, Uri uri)
        {
            return GetRequestSubmitter(verb, uri, string.Empty);
        }

        /// <summary>
        /// Create a request submitter with the given parameters
        /// </summary>
        private HttpJsonVariableRequestSubmitter GetRequestSubmitter(HttpVerb verb, Uri uri, string authToken)
        {
            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter
            {
                Uri = uri,
                Verb = verb
            };

            if (authToken != string.Empty)
            {
                submitter.Headers.Add("Authorization", $"Bearer {authToken}");
            }

            return submitter;
        }

        /// <summary>
        /// Add the order search criteria to the request
        /// </summary>
        private void AddOrdersSearchCriteria(HttpVariableRequestSubmitter request, DateTime startDate)
        {
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][field]", "updated_at",
                false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][condition_type]", "gt",
                false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][value]",
                $"{startDate:yyyy-MM-dd HH:mm:ff}", false));
        }

        /// <summary>
        /// Process the request and deserialize the response
        /// </summary>
        private T ProcessRequest<T>(HttpVariableRequestSubmitter request)
        {
            try
            {
                using (IHttpResponseReader response = request.GetResponse())
                {
                    return DeserializeResponse<T>(response.ReadResult());
                }
            }
            catch (Exception ex)
            {
                throw new MagentoException(ex);
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
                throw new Exception($"Failed to deserializes {typeof(T)}", ex);
            }
        }
    }
}