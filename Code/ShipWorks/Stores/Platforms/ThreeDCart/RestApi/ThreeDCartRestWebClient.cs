﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.Responses;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestWebClient
    {
        private const string HttpHost = "https://apirest.3dcart.com/3dCartWebAPI/";
        private const string GetOrderApiVersion = "v1/";
        private const string OrderUrlExtension = "Orders/";
        private const string ContentType = "application/json";
        private const string GetOrderLimit = "600";
        private readonly Uri GetOrderUri;
        private readonly string secureUrl;
        private readonly string privateKey;
        private readonly string token;

        private HttpVariableRequestSubmitter submitter;

        public ThreeDCartRestWebClient(ThreeDCartStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, "store");

            submitter = new HttpVariableRequestSubmitter();
            submitter.ContentType = ContentType;
            submitter.Headers.Add($"SecureUrl: {secureUrl}");
            submitter.Headers.Add($"PrivateKey: {privateKey}");
            submitter.Headers.Add($"Token: {token}");

            GetOrderUri = new Uri($"{HttpHost}{GetOrderApiVersion}{OrderUrlExtension}");
        }

        public IEnumerable<ThreeDCartOrder> GetOrders(DateTime startDate)
        {
            submitter.Verb = HttpVerb.Get;
            submitter.Uri = GetOrderUri;

            submitter.Variables.Add("datestart", startDate.ToShortDateString());

            string response = ProcessRequest("GetOrders");

            IEnumerable<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<IEnumerable<ThreeDCartOrder>>(response);

            return orders;
        }

        /// <summary>
        ///     Executes a request
        /// </summary>
        private string ProcessRequest(string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.LemonStand, action);
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
