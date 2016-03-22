using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestWebClient : IThreeDCartRestWebClient
    {
        private const string HttpHost = "https://apirest.3dcart.com/3dCartWebAPI/";
        private const string OrderApiVersion = "v1/";
        private const string OrderUrlExtension = "Orders/";
        private const string ProductApiVersion = "v1/";
        private const string ProductUrlExtension = "Products/";
        private const string ShipmentUrlExtension = "Shipments/";
        private const string ContentType = "application/json";
        private const string GetOrderLimit = "600";
        private readonly string secureUrl;
        private readonly string privateKey;
        private readonly string token;

        private HttpVariableRequestSubmitter submitter;

        public ThreeDCartRestWebClient(ThreeDCartStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "store");

            submitter = new HttpVariableRequestSubmitter();
            submitter.ContentType = ContentType;
            submitter.Headers.Add(HttpRequestHeader.Authorization, $"SecureUrl: {secureUrl}");
            submitter.Headers.Add(HttpRequestHeader.Authorization, $"PrivateKey: {privateKey}");
            submitter.Headers.Add(HttpRequestHeader.Authorization, $"Token: {token}");

        }

        /// <summary>
        /// Gets the orders.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        public IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate)
        {
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = new Uri($"{HttpHost}{OrderApiVersion}{OrderUrlExtension}");

            submitter.Variables.Add("datestart", startDate.ToShortDateString());
            submitter.Variables.Add("limit", GetOrderLimit);
            string response = ProcessRequest("GetOrders");

            IEnumerable<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<IEnumerable<ThreeDCartOrder>>(response);

            return orders;
        }

        public ThreeDCartProduct GetProduct(int catalogID)
        {
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = new Uri($"{HttpHost}{ProductApiVersion}{ProductUrlExtension}{catalogID}");

            string response = ProcessRequest("GetProduct");

            ThreeDCartProduct product = JsonConvert.DeserializeObject<IEnumerable<ThreeDCartProduct>>(response).FirstOrDefault();

            return product;
        }

        public void UploadShipmentDetails(int orderID, ThreeDCartShipment shipment)
        {
            submitter.Verb = HttpVerb.Put;
            submitter.Uri = new Uri($"{HttpHost}{OrderApiVersion}{OrderUrlExtension}{orderID}{ShipmentUrlExtension}{shipment.ShipmentID}");

            string body = "";
            submitter.Variables.Add(string.Empty, body);

            ProcessRequest("UploadShipmentDetails");
        }

        public void UpdateOrderStatus(int orderID, int statusID)
        {
            submitter.Verb = HttpVerb.Put;
            submitter.Uri = new Uri($"{HttpHost}{OrderApiVersion}{OrderUrlExtension}{orderID}");

            string body = $"{{OrderStatusID: {statusID}}}";

            submitter.Variables.Add(string.Empty, body);

            ProcessRequest("UpdateOrderStatus");
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private string ProcessRequest(string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ThreeDCart, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    return responseData;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
            }
        }
    }
}
