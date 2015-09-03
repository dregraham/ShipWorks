using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Groupon.DTO;
using System.Net;


namespace ShipWorks.Stores.Platforms.LemonStand
{
    class LemonStandWebClient
    {
        //LemonStand API endpoint
        private static string LemonStandEndpoint = "https://shipworks.lemonstand.com/api/v2";
        //private string apiKey = "7hMk9yBK4sRdV9SGC2m0KGQYytlWjirWRDN2E6jH";
        //private string accessToken = "mR5xLW3j1lChB6QPOm1UN5lAT6tq6zIUZUZtgQwr";

        private string dateFormat = "yyyy-MM-ddthh:mm:ssZ";

        private readonly LemonStandStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandWebClient(LemonStandStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Download orders from LemonStand
        /// </summary>
        public JToken GetOrders(DateTime updatedAtMin, int page) 
        { 
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Headers.Add(HttpRequestHeader.Authorization, "Bearer mR5xLW3j1lChB6QPOm1UN5lAT6tq6zIUZUZtgQwr");
            DateTime end = updatedAtMin.AddHours(23);

            ConfigureGetRequest(submitter, updatedAtMin, end, "orders", page);

            return ProcessRequest(submitter, "GetOrders");

        }

        private void ConfigureGetRequest(HttpVariableRequestSubmitter submitter, DateTime updatedAtMin, DateTime updatedAtMax, string operationName, int page)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(LemonStandEndpoint + "/" + operationName);
            submitter.Variables.Add("sort", "updated_at");
            //submitter.Variables.Add("updated_at_min", updatedAtMin.ToString(dateFormat));
            //submitter.Variables.Add("updated_at_max", updatedAtMax.ToString(dateFormat));
            //submitter.Variables.Add("page", page.ToString());
        }

        /// <summary>
        /// Setup a post request 
        /// </summary>
        private static void ConfigurePostRequest(HttpVariableRequestSubmitter submitter, string operationName, Dictionary<string, string> parameters)
        {
            submitter.Verb = HttpVerb.Post;

            submitter.Uri = new Uri(LemonStandEndpoint + "/" + operationName);

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
