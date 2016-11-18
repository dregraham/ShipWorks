using System;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.ComponentRegistration;
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

        /// <summary>
        /// Get an admin token for the given credentials
        /// </summary>
        public string GetToken(Uri storeUri, string username, string password)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Post, new Uri($"{storeUri.AbsoluteUri}{TokenEndpoint}"));
            request.Variables.Add(new HttpVariable(string.Empty, JsonConvert.SerializeObject(new[] {username, password})));

            return ProcessRequest<string>(request);
        }

        /// <summary>
        /// Get orders from the given store/start date
        /// </summary>
        public OrdersResponse GetOrders(DateTime start, Uri storeUri, string token)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get, new Uri($"{storeUri.AbsoluteUri}{OrdersEndpoint}"), token);
            AddOrdersSearchCriteria(request, start);

            return ProcessRequest<OrdersResponse>(request);
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
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][field]", "updated_at", false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][condition_type]", "gt", false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][value]", $"{startDate:yyyy-MM-dd HH:mm:ff}", false));
        }

        /// <summary>
        /// Process the request and deserialize the response
        /// </summary>
        private T ProcessRequest<T>(HttpVariableRequestSubmitter request)
        {
            using (IHttpResponseReader response = request.GetResponse())
            {
                return DeserializeResponse<T>(response.ReadResult());
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