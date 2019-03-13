using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    [Component]
    class TangoGetReleaseByCustomerRequest : ITangoGetReleaseByCustomerRequest
    {
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly ITangoWebRequestClient webRequestClient;
        private readonly ITangoWebClient tangoWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoGetReleaseByCustomerRequest(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient,
            ITangoWebClient tangoWebClient)
        {
            this.webRequestClient = webRequestClient;
            this.tangoWebClient = tangoWebClient;
            this.requestSubmitterFactory = requestSubmitterFactory;
        }

        /// <summary>
        /// Get release info for the current customer
        /// </summary>
        public GenericResult<ShipWorksReleaseInfo> GetReleaseInfo()
        {
            string customerID = tangoWebClient.GetTangoCustomerId();
            if (string.IsNullOrEmpty(customerID))
            {
                return GenericResult.FromError<ShipWorksReleaseInfo>("Could not retrieve customer id");
            }

            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "getreleasebyuser");
            request.Variables.Add("customerid", customerID);
            request.Variables.Add("version", currentVersion.ToString());

            return webRequestClient.ProcessXmlRequest<ShipWorksReleaseInfo>(request, "GetReleaseByVersion", true);
        }
    }
}
