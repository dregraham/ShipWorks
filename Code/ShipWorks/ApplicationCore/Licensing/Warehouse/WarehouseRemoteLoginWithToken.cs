using System;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    [Component]
    public class WarehouseRemoteLoginWithToken : IWarehouseRemoteLoginWithToken
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ITangoWebRequestClient webRequestClient;
        private readonly ITangoGetRedirectToken tangoGetRedirectToken;
        private readonly ICustomerLicenseReader customerLicenseReader;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRemoteLoginWithToken(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient,
            ITangoGetRedirectToken tangoGetRedirectToken)
        {
            this.tangoGetRedirectToken = tangoGetRedirectToken;
            this.webRequestClient = webRequestClient;
            this.requestSubmitterFactory = requestSubmitterFactory;
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        public GenericResult<TokenResponse> RemoteLoginWithToken()
        {
            GenericResult<TokenResponse> redirectToken = tangoGetRedirectToken.GetRedirectToken();

            if (redirectToken.Failure)
            {
                return GenericResult.FromError<TokenResponse>("Could not retrieve redirect token");
            }

            return RemoteLoginWithToken(redirectToken.Value.redirectToken);
        }

        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        private GenericResult<TokenResponse> RemoteLoginWithToken(string redirectToken)
        {
            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "remoteloginwithtoken");
            request.Variables.Add("redirectToken", redirectToken);

            request.ForcePreCallCertificateValidation = false;

            GenericResult<TokenResponse> tokenResponse = webRequestClient.ProcessXmlRequest<TokenResponse>(request, "RemoteLoginWithToken", true);

            return tokenResponse;
        }
    }
}
