using System;
using System.Collections.Generic;
using System.Net;
using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;
using Quartz.Util;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

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
        /// <param name="orderID">The LemonStand order ID</param>
        /// <returns>Order invoice in Json</returns>
        public JToken GetOrderInvoice(string orderID)
        {
            return ProcessRequest(CreateGetRequest("order/" + orderID + "?embed=invoices"), "GetOrderInvoice");
        }

        /// <summary>
        ///     Gets the shipment.
        /// </summary>
        /// <param name="invoiceID">The LemonStand invoice id</param>
        /// <returns>Shipment in Json</returns>
        public JToken GetShipment(string invoiceID)
        {
            return ProcessRequest(CreateGetRequest("invoices/" + invoiceID + "?embed=shipments"), "GetShipment");
        }

        /// <summary>
        ///     Gets the shipping address.
        /// </summary>
        /// <param name="shipmentID">The LemonStand shipment id</param>
        /// <returns>Shipping Address in Json</returns>
        public JToken GetShippingAddress(string shipmentID)
        {
            return ProcessRequest(CreateGetRequest("shipment/" + shipmentID + "?embed=shipping_address"),
                "GetShippingAddress");
        }

        /// <summary>
        ///     Gets the billing address.
        /// </summary>
        /// <param name="customerID">The LemonStand customer id</param>
        /// <returns>Billing Address in Json</returns>
        public JToken GetBillingAddress(string customerID)
        {
            return ProcessRequest(CreateGetRequest("customer/" + customerID + "?embed=billing_addresses"),
                "GetBillingAddress");
        }

        /// <summary>
        ///     Gets the product.
        /// </summary>
        /// <param name="productID">The LemonStand product id</param>
        /// <returns>Product in Json</returns>
        public JToken GetProduct(string productID)
        {
            return ProcessRequest(CreateGetRequest("product/" + productID + "?embed=images,categories,attributes"),
                "GetProduct");
        }

        /// <summary>
        ///     Gets all of the possible order statuses for this store
        /// </summary>
        /// <returns></returns>
        public JToken GetOrderStatuses()
        {
            return ProcessRequest(CreateGetRequest("orderstatuses"), "GetOrderStatuses");
        }

        /// <summary>
        ///     Uploads tracking number to LemonStand
        /// </summary>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shipmentID">The LemonStand shipment id.</param>
        public void UploadShipmentDetails(string trackingNumber, string shipmentID)
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
                ProcessRequest(CreatePostRequest("shipment/" + shipmentID + "/trackingcode", parameters),
                    "UploadShipmentDetails");
            }
            catch (Exception ex)
            {
                throw new LemonStandException(ex.Message);
            }
        }

        /// <summary>
        ///     Upload order status to LemonStand
        /// </summary>
        /// <param name="orderID">The LemonStand order ID</param>
        /// <param name="onlineStatus">The online status to set for this order</param>
        /// <exception cref="LemonStandException">
        /// The status is not a possible option.
        /// or
        /// This order's status can't transition to \ + onlineStatus + \ from it's current order status
        /// </exception>
        public void UpdateOrderStatus(string orderID, string onlineStatus)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            try
            {
                parameters.Add("status", onlineStatus);
                ProcessRequest(CreatePatchRequest("order/" + orderID, parameters), "UpdateOrderStatus");
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("The remote server returned an error: (400) Bad Request."))
                {
                    throw new LemonStandException("The status is not a possible option.", ex);
                }
                if (ex.Message.Equals("The remote server returned an error: (500) Internal Server Error."))
                {
                    throw new LemonStandException("This order's status can't transition to \"" + onlineStatus + "\" from it's current order status", ex);
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