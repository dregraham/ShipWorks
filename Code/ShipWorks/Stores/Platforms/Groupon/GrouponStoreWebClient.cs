using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Specialized;
using Interapptive.Shared.Net;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Shipping.Carriers.Postal;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.IO;
using ShipWorks.Shipping;
using System.Web;
using System.Globalization;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.Groupon
{
    class GrouponStoreWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponStoreWebClient));

        //Groupon API Endpoint
        private static string GrouponEndpoint = "http://10.1.10.132/json";

        // the store instance
        GrouponStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponStoreWebClient(GrouponStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Dwonload orders from Groupon
        /// </summary>
        public JObject GetOrders(int page)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            ConfigureRequest(submitter, "/get_orders", page);

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Mark lineitems as exported
        /// </summary>


        /// <summary>
        /// Uploads shipment details to Groupon
        /// </summary>


        /// <summary>
        /// Setup a request 
        /// </summary>
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, string operationName, int page)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = new Uri(GrouponEndpoint + operationName);
            submitter.Variables.Add("supplier_id", store.SupplierID);
            submitter.Variables.Add("token", store.Token);
            submitter.Variables.Add("page", page.ToString());
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private JObject ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Groupon, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {

                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "txt");

                    JObject document = JObject.Parse(responseData);
                    return document;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(GrouponStoreException));
            }
        }
    }
}
