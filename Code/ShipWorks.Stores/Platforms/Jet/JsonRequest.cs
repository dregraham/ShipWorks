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
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonRequest(Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Process the request
        /// </summary>
        public T ProcessRequest<T>(string action, ApiLogSource logSource, IHttpRequestSubmitter request)
        {
            IApiLogEntry apiLogEntry = apiLogEntryFactory(logSource, action);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<T>(result, jsonSerializerSettings);
            }
            catch (Exception ex) when (ex.GetType() != typeof(WebException))
            {
                throw new WebException(ex.Message);
            }
        }
    }
}