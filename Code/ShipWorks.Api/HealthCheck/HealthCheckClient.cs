using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net.RestSharp;
using log4net;
using RestSharp;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Calls HealthCheck Endpoint
    /// </summary>
    [Component(SingleInstance = true)]
    public class HealthCheckClient : IHealthCheckClient
    {
        private readonly IRestClientFactory clientFactory;
        private readonly IRestRequestFactory requestFactory;
        private readonly ILog log;
        private const int PortNumber = 8081;

        /// <summary>
        /// Constructor
        /// </summary>
        public HealthCheckClient(IRestClientFactory clientFactory, IRestRequestFactory requestFactory,
                                 Func<Type, ILog> loggerFactory)
        {
            this.clientFactory = clientFactory;
            this.requestFactory = requestFactory;
            log = loggerFactory(typeof(HealthCheckClient));
        }

        /// <summary>
        /// Returns true if running, else false
        /// </summary>
        public bool IsRunning()
        {
            IRestClient client = clientFactory.Create();
            IRestRequest request =
                requestFactory.Create($"http://{Environment.MachineName}:{PortNumber}/shipworks/api/v1/healthcheck", Method.GET);
            request.Timeout = 2000;

            try
            {
                HttpStatusCode statusCode = client.Execute(request).StatusCode;

                log.Debug($"Api healthcheck response status {statusCode}");

                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                log.Debug("Exception while performing ShipWorks api healthcheck", ex);
                return false;
            }
        }
    }
}
