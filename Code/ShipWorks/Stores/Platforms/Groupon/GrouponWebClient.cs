using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Groupon.DTO;


namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Web client for Groupon
    /// </summary>
    [Component]
    public class GrouponWebClient : IGrouponWebClient
    {
        private readonly IGrouponWebClientConfiguration configuration;
        private readonly ILogEntryFactory logEntryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponWebClient(ILogEntryFactory logEntryFactory, IGrouponWebClientConfiguration configuration)
        {
            this.logEntryFactory = logEntryFactory;
            this.configuration = configuration;
        }

        /// <summary>
        /// Download orders from Groupon
        /// </summary>
        public Task<JToken> GetOrders(IGrouponStoreEntity store, DateTime start, int page)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            // Groupon requires the start and end date to be within 24 hours of
            // each other adding 6 hours to keep page count from getting too high
            DateTime end = start.AddHours(23);

            ConfigureGetRequest(submitter, store, start.To(end), "get_orders", page);

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Uploads a batch of shipments to Groupon
        /// </summary>
        public Task UploadShipmentDetails(IGrouponStoreEntity store, List<GrouponTracking> trackingItems)
        {
            string trackingParameter = JsonConvert.SerializeObject(trackingItems);

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("supplier_id", store.SupplierID);
            parameters.Add("token", store.Token);
            parameters.Add("tracking_info", trackingParameter);

            ConfigurePostRequest(submitter, "tracking_notification", parameters);

            return ProcessRequest(submitter, "UploadShipmentDetails");
        }

        /// <summary>
        /// Setup a get request
        /// </summary>
        private void ConfigureGetRequest(HttpVariableRequestSubmitter submitter, IGrouponStoreEntity store, Range<DateTime> dateRange, string operationName, int page)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(configuration.Endpoint + "/" + operationName);
            submitter.Variables.Add("supplier_id", store.SupplierID);
            submitter.Variables.Add("token", store.Token);
            submitter.Variables.Add("start_datetime", dateRange.Start.ToString("MM/dd/yyyy HH:MM"));
            submitter.Variables.Add("end_datetime", dateRange.End.ToString("MM/dd/yyyy HH:MM"));
            submitter.Variables.Add("page", page.ToString());
        }

        /// <summary>
        /// Setup a post request
        /// </summary>
        private void ConfigurePostRequest(HttpVariableRequestSubmitter submitter, string operationName, Dictionary<string, string> parameters)
        {
            submitter.Verb = HttpVerb.Post;

            submitter.Uri = new Uri(configuration.Endpoint + "/" + operationName);

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                submitter.Variables.Add(parameter.Key, parameter.Value);
            }
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private async Task<JToken> ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                IApiLogEntry logEntry = logEntryFactory.GetLogEntry(ApiLogSource.Groupon, action, LogActionType.Other);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = await submitter.GetResponseAsync().ConfigureAwait(false))
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
