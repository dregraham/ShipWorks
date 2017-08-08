using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Json request
    /// </summary>
    [Component]
    public class JsonRequest : IJsonRequest
    {
        private readonly ILogEntryFactory apiLogEntryFactory;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonRequest(ILogEntryFactory apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Process the request
        /// </summary>
        public T ProcessRequest<T>(string action, ApiLogSource logSource, IHttpRequestSubmitter request)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory.GetLogEntry(logSource, action, LogActionType.Other);
            apiLogEntry.LogRequest(request);
            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<T>(result, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new WebException(ex.Message);
            }
        }
    }
}