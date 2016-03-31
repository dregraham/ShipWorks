using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestWebClient : IThreeDCartRestWebClient
    {
        private IProgressReporter progressReporter;
        private const string HttpHost = "https://apirest.3dcart.com/3dCartWebAPI";
        private const string OrderApiVersion = "v1";
        private const string OrderUrlExtension = "Orders";
        private const string ProductApiVersion = "v1";
        private const string ProductUrlExtension = "Products";
        private const string ShipmentUrlExtension = "Shipments";
        private const string ContentType = "application/json";
        private const string GetOrderLimit = "50";
        private const string PrivateKey = "c9fc5ce5b29d27121753baae724968b1";
        private readonly Type exceptionToRethrow = typeof (ThreeDCartException);
        private readonly string secureUrl;
        private readonly string token;
        private readonly ThreeDCartWebClientRequestThrottle throttler = new ThreeDCartWebClientRequestThrottle();

        private readonly HttpJsonVariableRequestSubmitter submitter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestWebClient"/> class.
        /// </summary>
        public ThreeDCartRestWebClient(ThreeDCartStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "store");

            secureUrl = store.StoreUrl;
            token = store.ApiUserKey;

            submitter = new HttpJsonVariableRequestSubmitter();
            submitter.Headers.Add($"Content-Type: {ContentType}");
            submitter.Headers.Add($"SecureUrl: {secureUrl}");
            submitter.Headers.Add($"PrivateKey: {PrivateKey}");
            submitter.Headers.Add($"Token: {token}");
        }

        /// <summary>
        /// Loads the progress reporter. Needed to display throttling message.
        /// </summary>
        public void LoadProgressReporter(IProgressReporter progressReporter)
        {
            this.progressReporter = progressReporter;
        }

        /// <summary>
        /// Attempt to get an order count to test connecting to ThreeDCart.  If any error, assume connection failed.
        /// </summary>
        public void TestConnection()
        {
            GetOrderCount(DateTime.Today, 0);
        }

        /// <summary>
        /// Gets the order count.
        /// </summary>
        public int GetOrderCount(DateTime startDate, int offset)
        {
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = new Uri($"{HttpHost}/{OrderApiVersion}/{OrderUrlExtension}");
            submitter.Variables.Add("datestart", startDate.ToShortDateString());
            submitter.Variables.Add("limit", GetOrderLimit);
            submitter.Variables.Add("offset", offset.ToString());
            submitter.Variables.Add("countonly", "1");

            string response = string.Empty;

            throttler.ExecuteRequest(new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrderCount, null, progressReporter), () =>
            {
                response = submitter.ProcessRequest(CreateLogEntry("GetOrderCount"), exceptionToRethrow);
            });

            ThreeDCartOrder order = JsonConvert.DeserializeObject<ThreeDCartOrder>(response);

            return order.TotalCount;
        }

        /// <summary>
        /// Gets the orders.
        /// </summary>
        public IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate, int offset)
        {
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = new Uri($"{HttpHost}/{OrderApiVersion}/{OrderUrlExtension}");
            submitter.Variables.Add("datestart", startDate.ToShortDateString());
            submitter.Variables.Add("limit", GetOrderLimit);
            submitter.Variables.Add("offset", offset.ToString());

            string response = string.Empty;

            throttler.ExecuteRequest(new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrders, null, progressReporter), () =>
            {
                response = submitter.ProcessRequest(CreateLogEntry("GetOrders"), exceptionToRethrow);
            });

            IEnumerable<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<IEnumerable<ThreeDCartOrder>>(response);

            return orders;
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        public ThreeDCartProduct GetProduct(int catalogID)
        {
            LruCache<int, ThreeDCartProduct> cache = ThreeDCartProductCache.Instance.GetStoreProductCache(secureUrl, token);

            ThreeDCartProduct productFromCache = cache[catalogID];

            if (productFromCache == null)
            {
                submitter.Verb = HttpVerb.Get;
                submitter.Uri = new Uri($"{HttpHost}/{ProductApiVersion}/{ProductUrlExtension}/{catalogID}");

                string response = string.Empty;

                throttler.ExecuteRequest(new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetProduct, null, progressReporter), () =>
                {
                    response = submitter.ProcessRequest(CreateLogEntry("GetProduct"), exceptionToRethrow);
                });

                ThreeDCartProduct product = JsonConvert.DeserializeObject<IEnumerable<ThreeDCartProduct>>(response).FirstOrDefault();

                // Since it was not in the cache, add it
                cache[catalogID] = product;

                return product;
            }

            return productFromCache;
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(ThreeDCartShipment shipment)
        {
            SetupUpdateRequest(shipment);

            throttler.ExecuteRequest(new RequestThrottleParameters(ThreeDCartWebClientApiCall.CreateFulfillment, null, progressReporter), () =>
            {
                submitter.ProcessRequest(CreateLogEntry("UploadShipmentDetails"), exceptionToRethrow);
            });
        }

        /// <summary>
        /// Updates the order status.
        /// </summary>
        public void UpdateOrderStatus(ThreeDCartShipment shipment)
        {
            SetupUpdateRequest(shipment);

            throttler.ExecuteRequest(new RequestThrottleParameters(ThreeDCartWebClientApiCall.UpdateOrderStatus, null, progressReporter), () =>
            {
                submitter.ProcessRequest(CreateLogEntry("UpdateOrderStatus"), exceptionToRethrow);
            });
        }


        /// <summary>
        /// Sets up the update request.
        /// </summary>
        private void SetupUpdateRequest(ThreeDCartShipment shipment)
        {
            submitter.Verb = HttpVerb.Put;
            submitter.Uri =
                new Uri(
                    $"{HttpHost}/{OrderApiVersion}/{OrderUrlExtension}/{shipment.OrderID}/{ShipmentUrlExtension}/{shipment.ShipmentID}");
            submitter.RequestBody = JsonConvert.SerializeObject(shipment, Formatting.None,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore});
        }

        /// <summary>
        /// Creates the log entry.
        /// </summary>
        private ApiLogEntry CreateLogEntry(string action)
        {
            return new ApiLogEntry(ApiLogSource.ThreeDCart, action);
        }
    }
}
