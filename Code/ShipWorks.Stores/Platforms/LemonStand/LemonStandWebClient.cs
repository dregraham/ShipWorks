using System;
using System.Collections.Generic;
using System.Net;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;
using Quartz.Util;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    public class LemonStandWebClient : ILemonStandWebClient
    {
        private const int itemsPerPage = 50;
        //LemonStand API endpoint
        private static string lemonStandEndpoint;
        private static string accessToken;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandWebClient(LemonStandStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            lemonStandEndpoint = store.StoreURL + "/api/v2";
            accessToken = store.Token;
        }

        /// <summary>
        ///     Get all orders from LemonStand
        /// </summary>
        /// <returns>Orders in Json</returns>
        public JToken GetOrders(int page, string start)
        {
            return
                ProcessRequest(
                    CreateGetRequest("orders?updated_at_min=" + start +
                                     "&sort=updated_at&order=desc&embed=invoices,customer,items&limit=" + itemsPerPage +
                                     "&page=" + page + "&is_quote=0"), "GetOrders");
        }

        /// <summary>
        ///     Gets a single order with invoice information.
        /// </summary>
        /// <param name="orderId">The LemonStand order ID</param>
        /// <returns>Order in Json</returns>
        public JToken GetOrderInvoice(string orderId)
        {
            return ProcessRequest(CreateGetRequest("order/" + orderId + "?embed=invoices"), "GetOrderInvoice");
        }

        /// <summary>
        ///     Gets the shipment.
        /// </summary>
        /// <param name="invoiceId">The LemonStand invoice id</param>
        /// <returns>Shipment in Json</returns>
        public JToken GetShipment(string invoiceId)
        {
            return ProcessRequest(CreateGetRequest("invoices/" + invoiceId + "?embed=shipments"), "GetShipment");
        }

        /// <summary>
        ///     Gets the shipping address.
        /// </summary>
        /// <param name="shipmentId">The LemonStand shipment id</param>
        /// <returns>Shipping Address in Json</returns>
        public JToken GetShippingAddress(string shipmentId)
        {
            return ProcessRequest(CreateGetRequest("shipment/" + shipmentId + "?embed=shipping_address"),
                "GetShippingAddress");
        }

        /// <summary>
        ///     Gets the billing address.
        /// </summary>
        /// <param name="customerId">The LemonStand customer id</param>
        /// <returns>Billing Address in Json</returns>
        public JToken GetBillingAddress(string customerId)
        {
            return ProcessRequest(CreateGetRequest("customer/" + customerId + "?embed=billing_addresses"),
                "GetBillingAddress");
        }

        /// <summary>
        ///     Gets the product.
        /// </summary>
        /// <param name="productId">The LemonStand product id</param>
        /// <returns>Product in Json</returns>
        public JToken GetProduct(string productId)
        {
            return ProcessRequest(CreateGetRequest("product/" + productId + "?embed=images,categories,attributes"),
                "GetProduct");
        }

        /// <summary>
        ///     Uploads tracking number and order status to LemonStand
        /// </summary>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shipmentId">The LemonStand shipment id.</param>
        /// <param name="onlineStatus">The online order status.</param>
        /// <param name="orderNumber">The LemonStand order number.</param>
        public void UploadShipmentDetails(string trackingNumber, string shipmentId, string onlineStatus,
            string orderNumber)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // LemonStand returns a bad request error if a blank tracking number is uploaded
            if (trackingNumber.IsNullOrWhiteSpace())
            {
                trackingNumber = "No tracking number was entered";
            }
            parameters.Add("tracking_code", trackingNumber);
            try
            {
                ProcessRequest(CreatePostRequest("shipment/" + shipmentId + "/trackingcode", parameters),
                        "UploadShipmentDetails");

                parameters.Clear();

                parameters.Add("status", onlineStatus);
                ProcessRequest(CreatePatchRequest("order/" + orderNumber, parameters), "UploadShipmentDetails");
            }
            catch (LemonStandException ex)
            {
                if (ex.Message.Equals("The remote server returned an error: (400) Bad Request."))
                {
                    throw new LemonStandException("The status is not a possible option.", ex);
                }

                throw;
            }
        }

        /// <summary>
        ///     Setup a get request.
        /// </summary>
        private static HttpVariableRequestSubmitter CreateGetRequest(string operationName)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);

            return submitter;
        }

        /// <summary>
        ///     Setup a post request
        /// </summary>
        private static HttpVariableRequestSubmitter CreatePostRequest(string operationName,
            Dictionary<string, string> parameters)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            submitter.Verb = HttpVerb.Post;
            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);
            submitter.AllowHttpStatusCodes(HttpStatusCode.Created);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                submitter.Variables.Add(parameter.Key, parameter.Value);
            }

            return submitter;
        }

        /// <summary>
        ///     Setup a patch request
        /// </summary>
        private static HttpVariableRequestSubmitter CreatePatchRequest(string operationName,
            Dictionary<string, string> parameters)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            submitter.Verb = HttpVerb.Patch;
            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);
            submitter.AllowHttpStatusCodes(HttpStatusCode.Created);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                submitter.Variables.Add(parameter.Key, parameter.Value);
            }

            return submitter;
        }

        /// <summary>
        ///     Executes a request
        /// </summary>
        private static JToken ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.LemonStand, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    JToken document = JToken.Parse(responseData);
                    return document;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof (LemonStandException));
            }
        }
    }
}