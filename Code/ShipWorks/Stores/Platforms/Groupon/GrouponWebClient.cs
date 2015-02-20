﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using System.Collections.Specialized;
using Interapptive.Shared.Net;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.IO;
using System.Web;
using System.Globalization;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Platforms.Groupon.DTO;
using System.Threading;


namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponWebClient
    {

        //Groupon API Endpoint
        //private static string GrouponEndpoint = "https://scm.commerceinterface.com/api/v2";
        private static string GrouponEndpoint = "http://10.1.10.132/json/json/";


        // the store instance
        GrouponStoreEntity store;

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
        public JToken GetOrders(int page)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            ConfigureGetRequest(submitter, "/get_orders", page);

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Mark lineitems as exported
        /// </summary>

        /// <summary>
        /// Uploads a batch of shipments to Groupon
        /// </summary>
        public void UploadShipmentDetails(List<GrouponTracking> trackingItmes)
        {
            List<string> trackingList = new List<string>();
            foreach(GrouponTracking trackingItem in trackingItmes)
            {
                trackingList.Add(JsonConvert.SerializeObject(trackingItem));
            }

            string trackingParameter = "[" + String.Join(",", trackingList) + "]";

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();

            Dictionary<string, string> parameters = new Dictionary<string,string>();

            parameters.Add("supplier_id", store.SupplierID);
            parameters.Add("token", store.Token);
            parameters.Add("tracking_info", trackingParameter);

            ConfigurePostRequest(submitter, "/tracking_notification", parameters);

            ProcessRequest(submitter, "UploadShipmentDetails");

            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Groupon, "TESTLOG");
            logEntry.LogResponse(submitter.GetPostContent().ToString(), "txt");

        }
           

        /// <summary>
        /// Setup a get request 
        /// </summary>
        private void ConfigureGetRequest(HttpVariableRequestSubmitter submitter, string operationName, int page)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(GrouponEndpoint + operationName);
            submitter.Variables.Add("supplier_id", store.SupplierID);
            submitter.Variables.Add("token", store.Token);
            submitter.Variables.Add("page", page.ToString());
        }

        /// <summary>
        /// Setup a post request 
        /// </summary>
        private static void ConfigurePostRequest(HttpVariableRequestSubmitter submitter,string operationName , Dictionary<string,string> parameters)
        {
            submitter.Verb = HttpVerb.Post;

            submitter.Uri = new Uri(GrouponEndpoint + operationName);

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
