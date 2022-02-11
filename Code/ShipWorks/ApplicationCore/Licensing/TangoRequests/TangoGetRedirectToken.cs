using System;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get a Tango redirect token for the WebReg customer
    /// </summary>
    [Component]
    public class TangoGetRedirectToken : ITangoGetRedirectToken
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ITangoWebRequestClient webRequestClient;
        private readonly ITangoWebClient tangoWebClient;
        private readonly ICustomerLicenseReader customerLicenseReader;
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoGetRedirectToken(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient,
            ITangoWebClient tangoWebClient,
            ICustomerLicenseReader customerLicenseReader,
            ILicenseService licenseService)
        {
            this.tangoWebClient = tangoWebClient;
            this.webRequestClient = webRequestClient;
            this.requestSubmitterFactory = requestSubmitterFactory;
            this.customerLicenseReader = customerLicenseReader;
            this.licenseService = licenseService;
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        public GenericResult<TokenResponse> GetRedirectToken()
        {
            
            string customerKey = licenseService.GetCustomerLicenseKey();

            if (string.IsNullOrEmpty(customerKey))
            {
                return GenericResult.FromError<TokenResponse>("Could not retrieve customer key");
            }

            return GetRedirectToken(customerKey);
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        private GenericResult<TokenResponse> GetRedirectToken(string customerKey)
        {
            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "createRedirectToken");
            request.Variables.Add("customerlicense", customerKey);

            request.ForcePreCallCertificateValidation = false;

            GenericResult<TokenResponse> tokenResponse = webRequestClient.ProcessXmlRequest<TokenResponse>(request, "GetRedirectToken", true);

            return tokenResponse;
        }
    }
}
