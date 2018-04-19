using System;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.ZipResponse;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Authentication
{
    /// <summary>
    /// Authenticates credentials against OnTrac
    /// </summary>
    public class OnTracAuthentication : OnTracRequest
    {
        private readonly HttpRequestSubmitter requestSubmitter;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracAuthentication(int accountId, string password)
            : base(accountId, password, "OnTracAuthenticationRequest")
        {
            requestSubmitter = new HttpVariableRequestSubmitter();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracAuthentication(long accountId, string password, HttpRequestSubmitter requestSubmitter, ILogEntryFactory logEntryFactory)
            : base(accountId, password, logEntryFactory, ApiLogSource.OnTrac, "OnTracAuthenticationRequest", LogActionType.Other)
        {
            this.requestSubmitter = requestSubmitter;
        }

        /// <summary>
        /// Authenticate OnTrac User. Throws OnTracException cannot validate credentials.
        /// </summary>
        public void IsValidUser()
        {
            // We are testing OnTrac credentials by requesting any zip codes that were added after
            // 1/1/3000. There shouldn't be any, so No Zip Updates will be available. This is the request with 
            // the least amount of overhead to test credentials according to OnTrac.
            string requestUrl = $"{BaseUrlUsedToCallOnTrac}{AccountNumber}/zips?pw={OnTracPassword}&lastupdate=3000-1-1";

            requestSubmitter.Uri = new Uri(requestUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            try
            {
                ExecuteLoggedRequest<OnTracZipResponse>(requestSubmitter);
            }
            catch (OnTracException ex)
            {
                // "No Zip Updates Available" is what we'd expect
                if (!ex.Message.Contains("Updates"))
                {
                    throw;
                }
            }
        }
    }
}