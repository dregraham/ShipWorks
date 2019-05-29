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
    public class TangoGetReleaseByVersionRequest : ITangoGetReleaseByVersionRequest
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ITangoWebRequestClient webRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoGetReleaseByVersionRequest(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient)
        {
            this.webRequestClient = webRequestClient;
            this.requestSubmitterFactory = requestSubmitterFactory;
        }

        /// <summary>
        /// Get release info for a specific version
        /// </summary>
        public GenericResult<ShipWorksReleaseInfo> GetReleaseInfo(Version version)
        {
            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "getreleasebyversion");
            request.Variables.Add("version", version.ToString());

            return webRequestClient.ProcessXmlRequest<ShipWorksReleaseInfo>(request, "GetReleaseByVersion", false);
        }
    }
}
