using System;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.RateResponse;

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
            // We are testing OnTrac credentials by requesting rates. The zip code request we were previously 
            // using no longer returns an error when called with bad credentials
            string requestUrl = $"{BaseUrlUsedToCallOnTrac}{AccountNumber}/rates?pw={OnTracPassword}&packages=ID1;90210;90001;false;0.00;false;0;5;4X3X10;S;0;0";

            requestSubmitter.Uri = new Uri(requestUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            try
            {
                ExecuteLoggedRequest<OnTracRateResponse>(requestSubmitter);
            }
            catch (OnTracException ex)
            {
                // "The specified account number and password are not correct." is what we'd expect if the credentials are bad
                // We ignore other errors to ensure we don't prevent them from adding their account
                // because of unrelated errors
                if (ex.Message.ToLowerInvariant().Contains("password"))
                {
                    throw;
                }
            }
        }
    }
}