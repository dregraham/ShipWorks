using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using ShipWorks.Stores.Platforms.SparkPay.Enums;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayWebClient
    {
        private int OverApiLimitStatusCode = 429;
        private SparkPayWebClientRequestThrottle throttler;

        public SparkPayWebClient(SparkPayWebClientRequestThrottle throttler)
        {
            this.throttler = throttler;
        }

        /// <summary>
        /// Gets orders that have an updated_at that is greater than the given start
        /// </summary>
        public OrdersResponse GetOrders(SparkPayStoreEntity store, DateTime start, IProgressReporter progressReporter)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.GetOrders;

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            HttpVariable startVar = new HttpVariable("updated_at", $"gt:{start.ToString("s")}.{start.ToString("fff")}-00:00");
            HttpVariable expandedVar = new HttpVariable("expand", "payments,items,shipments,custom_fields");

            ConfigureRequest(submitter, store, call, new[] { startVar, expandedVar }, HttpVerb.Get);

            return ProcessThrottledRequest<OrdersResponse>(submitter, call, progressReporter);
        }

        /// <summary>
        /// Gets a list of order statuses for the given store
        /// </summary>
        public OrderStatusResponse GetStatuses(SparkPayStoreEntity store)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.GetStatuses;

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            ConfigureRequest(submitter, store, call, null, HttpVerb.Get);

            return ProcessThrottledRequest<OrderStatusResponse>(submitter, call, null);
        }

        /// <summary>
        /// Gets the address of the given Id
        /// </summary>
        public AddressesResponse GetAddress(SparkPayStoreEntity store, int addressId, IProgressReporter progressReporter)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.GetAddresses;

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            HttpVariable startVar = new HttpVariable("id", addressId.ToString());

            ConfigureRequest(submitter, store, call, new[] { startVar }, HttpVerb.Get);

            return ProcessThrottledRequest<AddressesResponse>(submitter, call, progressReporter);
        }

        /// <summary>
        /// Adds the given shipment to the order
        /// </summary>
        public void AddShipment(SparkPayStoreEntity store, Shipment shipment)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.AddShipment;

            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();
            string shipmentJson = JsonConvert.SerializeObject(shipment, GetSerializerSettings());

            ConfigureRequest(submitter, store, SparkPayWebClientApiCall.AddShipment, new[] { new HttpVariable("", shipmentJson) }, HttpVerb.Post);

            ProcessThrottledRequest<OrdersResponse>(submitter, call, null);
        }

        /// <summary>
        /// Updates the orders status
        /// </summary>
        public Order UpdateOrderStatus(SparkPayStoreEntity store, long orderNumber, int statusId)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.AddShipment;

            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();
            string path = string.Format(EnumHelper.GetApiValue(SparkPayWebClientApiCall.UpdateOrderStatus), orderNumber);

            ConfigureRequest(submitter, store, path, new[] { new HttpVariable("", $"{{\"order_status_id\":{statusId}}}") }, HttpVerb.Put);

            return ProcessThrottledRequest<Order>(submitter, call, null);
        }

        /// <summary>
        /// Gets the stores from the given url
        /// </summary>
        public StoresResponse GetStores(string token, string url)
        {
            HttpVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter
            {
                Verb = HttpVerb.Get,
                Uri = new Uri(url)
            };

            submitter.Headers.Add("X-AC-Auth-Token", token);

            return ProcessRequest<StoresResponse>(submitter, SparkPayWebClientApiCall.GetStores);
        }

        /// <summary>
        /// Configures the request based on the call being made
        /// </summary>
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, SparkPayStoreEntity store, SparkPayWebClientApiCall call, IEnumerable<HttpVariable> variables, HttpVerb verb)
        {
            ConfigureRequest(submitter, store, EnumHelper.GetApiValue(call), variables, verb);
        }

        /// <summary>
        /// Setup a get request 
        /// </summary>
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, SparkPayStoreEntity store, string operationName, IEnumerable<HttpVariable> variables, HttpVerb verb)
        {
            submitter.Verb = verb;

            // add the store url and auth token
            submitter.Uri = new Uri($"{store.StoreUrl}/{operationName}");
            submitter.Headers.Add("X-AC-Auth-Token", store.Token);
            
            variables?.ToList().ForEach(submitter.Variables.Add);
            
            submitter.AllowHttpStatusCodes(new[] { HttpStatusCode.Created });
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private T ProcessThrottledRequest<T>(HttpRequestSubmitter submitter, SparkPayWebClientApiCall call, IProgressReporter progressReporter)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.SparkPay, EnumHelper.GetDescription(call));
                logEntry.LogRequest(submitter);

                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(call, submitter, progressReporter);

                using (IHttpResponseReader reader = throttler.ExecuteRequest<HttpRequestSubmitter, IHttpResponseReader>(requestThrottleArgs, MakeRequest))
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    return DeserializeResponse<T>(responseData);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(SparkPayException));
            }
        }
        
        /// <summary>
        /// Executes a request
        /// </summary>
        private T ProcessRequest<T>(HttpRequestSubmitter submitter, SparkPayWebClientApiCall call)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.SparkPay, EnumHelper.GetDescription(call));
                logEntry.LogRequest(submitter);
                
                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    return DeserializeResponse<T>(responseData);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(SparkPayException));
            }
        }

        /// <summary>
        /// Submit a SparkPay request, throttled so we don't over-call
        /// </summary>
        /// <typeparam name="THttpRequestSubmitter">Needed by the throttler.  The type of the request to send to the api via throttler.</typeparam>
        /// <typeparam name="THttpResponseReader">Needed by the throttler.  The type of the response that will be received by the api via throttler.</typeparam>
        /// <param name="request">The actual request to make.</param>
        /// <returns>HttpResponseReader received from the call</returns>
        private IHttpResponseReader MakeRequest<THttpRequestSubmitter>(THttpRequestSubmitter request)
            where THttpRequestSubmitter : HttpRequestSubmitter
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            try
            {
                IHttpResponseReader responseReader = request.GetResponse();

                return responseReader;
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = ex.Response as HttpWebResponse;
                if (webResponse != null && webResponse.StatusCode == (HttpStatusCode)OverApiLimitStatusCode)
                {
                    throw new RequestThrottledException(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Serializer Settings to be used when serializing json
        /// </summary>
        /// <returns></returns>
        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
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
                throw new SparkPayException($"Failed to deserializes {typeof(T)}", ex);
            }
        }
    }
}
