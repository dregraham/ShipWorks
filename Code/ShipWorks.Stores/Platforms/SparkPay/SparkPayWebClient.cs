﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using ShipWorks.Stores.Platforms.SparkPay.Enums;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using HttpJsonVariableRequestSubmitter = ShipWorks.Stores.Platforms.SparkPay.Factories.HttpJsonVariableRequestSubmitter;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// The SparkPay web client
    /// </summary>
    [Component]
    public class SparkPayWebClient : ISparkPayWebClient
    {
        private readonly int OverApiLimitStatusCode = 429;
        private readonly ISparkPayWebClientRequestThrottle throttler;
        private readonly ISparkPayShipmentFactory shipmentFactory;

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SparkPayWebClient));

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayWebClient(ISparkPayWebClientRequestThrottle throttler,
            ISparkPayShipmentFactory shipmentFactory)
        {
            this.throttler = throttler;
            this.shipmentFactory = shipmentFactory;
        }

        /// <summary>
        /// Gets orders that have an updated_at that is greater than the given start
        /// </summary>
        public OrdersResponse GetOrders(ISparkPayStoreEntity store, DateTime start, IProgressReporter progressReporter)
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
        public OrderStatusResponse GetStatuses(ISparkPayStoreEntity store)
        {
            log.Debug("start getting statuses");
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.GetStatuses;
            log.Debug("set call");
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            log.Debug("create submitter and configure request");
            ConfigureRequest(submitter, store, call, null, HttpVerb.Get);
            log.Debug("configured request");
            return ProcessRequest<OrderStatusResponse>(submitter, call);
        }

        /// <summary>
        /// Gets the address of the given Id
        /// </summary>
        public AddressesResponse GetAddress(ISparkPayStoreEntity store, int addressId, IProgressReporter progressReporter)
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
        public Task AddShipment(ISparkPayStoreEntity store, ShipmentEntity shipmentEntity, long orderNumber)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.AddShipment;

            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();

            Shipment shipment = shipmentFactory.Create(shipmentEntity, orderNumber);
            string shipmentJson = JsonConvert.SerializeObject(shipment, GetSerializerSettings());

            ConfigureRequest(submitter, store, call, new[] { new HttpVariable("", shipmentJson) }, HttpVerb.Post);

            return ProcessRequestAsync<OrdersResponse>(submitter, call);
        }

        /// <summary>
        /// Updates the orders status
        /// </summary>
        public Order UpdateOrderStatus(ISparkPayStoreEntity store, long orderNumber, int statusId)
        {
            SparkPayWebClientApiCall call = SparkPayWebClientApiCall.UpdateOrderStatus;

            HttpJsonVariableRequestSubmitter submitter = new HttpJsonVariableRequestSubmitter();
            string path = string.Format(EnumHelper.GetApiValue(call), orderNumber);

            ConfigureRequest(submitter, store, path, new[] { new HttpVariable("", $"{{\"order_status_id\":{statusId}}}") }, HttpVerb.Put);

            return ProcessRequest<Order>(submitter, call);
        }

        /// <summary>
        /// Gets the stores from the given URL
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
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, ISparkPayStoreEntity store, SparkPayWebClientApiCall call, IEnumerable<HttpVariable> variables, HttpVerb verb)
        {
            ConfigureRequest(submitter, store, EnumHelper.GetApiValue(call), variables, verb);
        }

        /// <summary>
        /// Setup a get request
        /// </summary>
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, ISparkPayStoreEntity store, string operationName, IEnumerable<HttpVariable> variables, HttpVerb verb)
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
                ApiLogEntry logEntry = GetLogEntry(submitter, call);
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
                log.Debug("Getting log entry");
                ApiLogEntry logEntry = GetLogEntry(submitter, call);
                log.Debug("Getting response");
                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    log.Debug("reading response");
                    string responseData = reader.ReadResult();
                    log.Debug("logging response");
                    logEntry.LogResponse(responseData, "txt");
                    log.Debug($"deserializing response {typeof(T)}");
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
        private async Task<T> ProcessRequestAsync<T>(HttpRequestSubmitter submitter, SparkPayWebClientApiCall call)
        {
            try
            {
                log.Debug("Getting log entry");
                ApiLogEntry logEntry = GetLogEntry(submitter, call);
                log.Debug("Getting response");
                using (IHttpResponseReader reader = await submitter.GetResponseAsync().ConfigureAwait(false))
                {
                    log.Debug("reading response");
                    string responseData = await reader.ReadResultAsync().ConfigureAwait(false);
                    log.Debug("logging response");
                    logEntry.LogResponse(responseData, "txt");
                    log.Debug($"deserializing response {typeof(T)}");
                    return DeserializeResponse<T>(responseData);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(SparkPayException));
            }
        }

        /// <summary>
        /// Gets the log entry for the request
        /// </summary>
        private ApiLogEntry GetLogEntry(HttpRequestSubmitter submitter, SparkPayWebClientApiCall call)
        {
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.SparkPay, EnumHelper.GetDescription(call));
            logEntry.LogRequest(submitter);

            return logEntry;
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
                if (webResponse != null && webResponse.StatusCode == (HttpStatusCode) OverApiLimitStatusCode)
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
