using System;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;

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
        private const string UnholdEndpoint = "rest/V1/orders/{0}/unhold";
        private const string CancelEndpoint = "rest/V1/orders/{0}/cancel";
        private const string CommentEndpoint = "rest/V1/orders/{0}/comments";
        private const string InvoiceEndpoint = "rest/V1/invoices";
        private const int PageSize = 5;

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
        public IOrdersResponse GetOrders(DateTime start, Uri storeUri, string token, int currentPage)
        {
            HttpJsonVariableRequestSubmitter request = GetRequestSubmitter(HttpVerb.Get,
                new Uri($"{storeUri.AbsoluteUri}/{OrdersEndpoint}"), token);
            AddOrdersSearchCriteria(request, start, currentPage);

            return ProcessRequest<OrdersResponse>(request);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(string shipmentDetailsJson, string invoice, Uri storeUri, string token, long magentoOrderId)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(ShipmentEndpoint, magentoOrderId)}"), token);

            submitter.RequestBody = shipmentDetailsJson;

            ProcessRequest(submitter);

            submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{InvoiceEndpoint}"), token);

            submitter.RequestBody = invoice;

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Uploads comments only
        /// </summary>
        public void UploadComments(string comments, Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CommentEndpoint, magentoOrderID)}"), token);

            submitter.RequestBody = $"\"comments\":\"{comments}\"";

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        public void HoldOrder(Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(HoldEndpoint, magentoOrderID)}"), token);

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Take hold off of a Magento order
        /// </summary>
        public void UnholdOrder(Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(UnholdEndpoint, magentoOrderID)}"), token);

            ProcessRequest(submitter);
        }

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        public void CancelOrder(Uri storeUri, string token, long magentoOrderID)
        {
            HttpJsonVariableRequestSubmitter submitter = GetRequestSubmitter(HttpVerb.Post,
                new Uri($"{storeUri.AbsoluteUri}/{string.Format(CancelEndpoint, magentoOrderID)}"), token);

            ProcessRequest(submitter);
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
        private void AddOrdersSearchCriteria(HttpVariableRequestSubmitter request, DateTime startDate, int currentPage)
        {
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][field]", "updated_at",
                false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][condition_type]", "gt",
                false));
            request.Variables.Add(new HttpVariable("searchCriteria[filter_groups][0][filters][0][value]",
                $"{startDate:yyyy-MM-dd HH:mm:ff}", false));
            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][field]", "updated_at", false));
            request.Variables.Add(new HttpVariable("searchCriteria[sortOrders][0][direction]", "asc", false));
            request.Variables.Add(new HttpVariable("searchCriteria[pageSize]", PageSize.ToString(), false));
            request.Variables.Add(new HttpVariable("searchCriteria[currentPage]", currentPage.ToString(), false));
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private void ProcessRequest(HttpVariableRequestSubmitter request)
        {
            try
            {
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new MagentoException(ex);
            }
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
                throw new Exception($"Failed to deserialize {typeof(T)}", ex);
            }
        }
    }
}