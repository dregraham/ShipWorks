using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get release info for a given version
    /// </summary>
    [Component]
    public class TangoGetReleaseByUserRequest : ITangoGetReleaseByUserRequest
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ITangoWebRequestClient webRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoGetReleaseByUserRequest(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient)
        {
            this.webRequestClient = webRequestClient;
            this.requestSubmitterFactory = requestSubmitterFactory;
        }

        /// <summary>
        /// Get release info for a user
        /// </summary>
        public GenericResult<ShipWorksReleaseInfo> GetReleaseInfo(string tangoCustomerID, Version version)
        {
            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "getreleasebyuser");
            request.Variables.Add("customerid", tangoCustomerID);
            request.Variables.Add("version", version.ToString());

            return webRequestClient.ProcessXmlRequest<ShipWorksReleaseInfo>(request, "GetReleaseByUser", true);
        }
    }
}
