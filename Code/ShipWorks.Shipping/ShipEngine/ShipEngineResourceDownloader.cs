using System;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using System.Net;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Download resources from ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineResourceDownloader : IShipEngineResourceDownloader
    {
        private readonly ILogEntryFactory apiLogEntryFactory;
        private readonly IHttpRequestSubmitterFactory httpRequestFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineResourceDownloader(ILogEntryFactory apiLogEntryFactory, IHttpRequestSubmitterFactory httpRequestFactory)
        {
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.httpRequestFactory = httpRequestFactory;
        }

        /// <summary>
        /// Download the resourcce at the given uri
        /// </summary>
        public byte[] Download(Uri uri, ApiLogSource logSource, string actionName)
        {
            try
            {
                IHttpRequestSubmitter requestSubmitter = httpRequestFactory.GetHttpVariableRequestSubmitter();
                requestSubmitter.Uri = uri;
                requestSubmitter.Verb = HttpVerb.Get;

                IApiLogEntry logEntry = apiLogEntryFactory.GetLogEntry(logSource, actionName, LogActionType.Other);
                logEntry.LogRequest(requestSubmitter);


                using (IHttpResponseReader responseReader = requestSubmitter.GetResponse())
                {
                    string response = responseReader.ReadResult();
                    logEntry.LogResponse(response, "txt");

                    return Encoding.ASCII.GetBytes(response);
                }
            }
            catch (Exception ex)
            {
                throw new ShipEngineException($"An error occured while attempting to download reasource from {logSource}.");
            }
        }
    }
}
