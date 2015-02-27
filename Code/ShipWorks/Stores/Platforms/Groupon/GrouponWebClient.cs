using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Groupon.DTO;


namespace ShipWorks.Stores.Platforms.Groupon
{
    public class GrouponWebClient
    {

        //Groupon API Endpoint
        private static string GrouponEndpoint = "https://scm.commerceinterface.com/api/v2";
        //private static string GrouponEndpoint = "http://10.1.10.132/json";


        // the store instance
        private readonly GrouponStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponWebClient(GrouponStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Dwonload orders from Groupon
        /// </summary>
        public JToken GetOrders(DateTime start, int page)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            //Groupon requires the start and end date to be within 24 hours of  
            //eachother adding 6 hours to keep page count from getting too high 
            DateTime end = start.AddHours(23);

            ConfigureGetRequest(submitter, start, end, "get_orders", page);

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Uploads a batch of shipments to Groupon
        /// </summary>
        public void UploadShipmentDetails(List<GrouponTracking> trackingItems)
        {
            string trackingParameter = JsonConvert.SerializeObject(trackingItems);

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("supplier_id", store.SupplierID);
            parameters.Add("token", store.Token);
            parameters.Add("tracking_info", trackingParameter);

            ConfigurePostRequest(submitter, "tracking_notification", parameters);

            ProcessRequest(submitter, "UploadShipmentDetails");
        }
           

        /// <summary>
        /// Setup a get request 
        /// </summary>
        private void ConfigureGetRequest(HttpVariableRequestSubmitter submitter, DateTime start, DateTime end, string operationName, int page)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(GrouponEndpoint + "/" + operationName);
            submitter.Variables.Add("supplier_id", store.SupplierID);
            submitter.Variables.Add("token", store.Token);
            submitter.Variables.Add("start_datetime", start.ToString("MM/dd/yyyy HH:MM"));
            submitter.Variables.Add("end_datetime", end.ToString("MM/dd/yyyy HH:MM"));
            submitter.Variables.Add("page", page.ToString());
        }

        /// <summary>
        /// Setup a post request 
        /// </summary>
        private static void ConfigurePostRequest(HttpVariableRequestSubmitter submitter, string operationName, Dictionary<string, string> parameters)
        {
            submitter.Verb = HttpVerb.Post;

            submitter.Uri = new Uri(GrouponEndpoint + "/" + operationName);

            foreach(KeyValuePair<string, string> parameter in parameters)
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
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Groupon, action);
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
                throw WebHelper.TranslateWebException(ex, typeof(GrouponException));
            }
        }
    }
}
