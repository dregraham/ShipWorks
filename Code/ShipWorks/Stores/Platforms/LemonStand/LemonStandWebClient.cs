using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using System.Net;


namespace ShipWorks.Stores.Platforms.LemonStand
{
    public class LemonStandWebClient
    {
        //LemonStand API endpoint
        private static string lemonStandEndpoint;
        private static string accessToken;
        
        private readonly LemonStandStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandWebClient(LemonStandStoreEntity store)
        {
            this.store = store;
            lemonStandEndpoint = store.StoreURL + "/api/v2";
            accessToken = store.Token;
        }

        /// <summary>
        /// Get all orders from LemonStand
        /// </summary>
        /// <returns>Orders in Json</returns>
        public JToken GetOrders() 
        { 
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "orders?sort=updated_at&order=desc&embed=invoices,customer,items");

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Gets a single order with invoice information.
        /// </summary>
        /// <param name="orderId">The LemonStand order ID</param>
        /// <returns>Order in Json</returns>
        public JToken GetOrderInvoice(string orderId)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "order/" + orderId + "?embed=invoices");

            return ProcessRequest(submitter, "GetOrderInvoice");
        }
        /// <summary>
        /// Gets the shipment.
        /// </summary>
        /// <param name="invoiceId">The LemonStand invoice id</param>
        /// <returns>Shipment in Json</returns>
        public JToken GetShipment(string invoiceId) 
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "invoices/" + invoiceId + "?embed=shipments");

            return ProcessRequest(submitter, "GetShipment");            
        }

        /// <summary>
        /// Gets the shipping address.
        /// </summary>
        /// <param name="shipmentId">The LemonStand shipment id</param>
        /// <returns>Shipping Address in Json</returns>
        public JToken GetShippingAddress(string shipmentId) 
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "shipment/" + shipmentId + "?embed=shipping_address");

            return ProcessRequest(submitter, "GetShippingAddress");
        }

        /// <summary>
        /// Gets the billing address.
        /// </summary>
        /// <param name="customerId">The LemonStand customer id</param>
        /// <returns>Billing Address in Json</returns>
        public JToken GetBillingAddress(string customerId) 
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "customer/" + customerId + "?embed=billing_addresses");

            return ProcessRequest(submitter, "GetBillingAddress");        
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The LemonStand product id</param>
        /// <returns>Product in Json</returns>
        public JToken GetProduct(string productId) 
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            ConfigureGetRequest(submitter, "product/" + productId);

            return ProcessRequest(submitter, "GetBillingAddress");        
        }

        /// <summary>
        /// Uploads tracking number and order status to LemonStand
        /// </summary>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shipmentId">The LemonStand shipment id.</param>
        /// <param name="onlineStatus">The online order status.</param>
        /// <param name="orderNumber">The LemonStand order number.</param>
        public void UploadShipmentDetails(string trackingNumber, string shipmentId, string onlineStatus, string orderNumber)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add("tracking_code", trackingNumber);
            ConfigurePostRequest(submitter, "shipment/" + shipmentId + "/trackingcode/" , parameters);
            ProcessRequest(submitter, "UploadShipmentDetails");


            submitter = new HttpVariableRequestSubmitter();
            parameters.Clear();
            
            parameters.Add("status", onlineStatus);
            ConfigurePatchRequest(submitter, "order/" + orderNumber, parameters);
            ProcessRequest(submitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Setup a get request.
        /// </summary>
        private void ConfigureGetRequest(HttpVariableRequestSubmitter submitter, string operationName)
        {
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);

            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);
            
        }

        /// <summary>
        /// Setup a post request 
        /// </summary>
        private static void ConfigurePostRequest(HttpVariableRequestSubmitter submitter, string operationName, Dictionary<string, string> parameters)
        {
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);

            submitter.Verb = HttpVerb.Post;

            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);

            submitter.AllowHttpStatusCodes(HttpStatusCode.Created);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                submitter.Variables.Add(parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// Setup a patch request 
        /// </summary>
        private static void ConfigurePatchRequest(HttpVariableRequestSubmitter submitter, string operationName, Dictionary<string, string> parameters)
        {
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);

            submitter.Verb = HttpVerb.Patch;

            submitter.Uri = new Uri(lemonStandEndpoint + "/" + operationName);

            submitter.AllowHttpStatusCodes(HttpStatusCode.Created);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                submitter.Variables.Add(parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// Executes a request
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
                throw WebHelper.TranslateWebException(ex, typeof(LemonStandException));
            }
        }
    }
}
