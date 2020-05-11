using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net.RestSharp;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore;

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
        private readonly IShipWorksSession session;
        private readonly ILog log;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public HealthCheckClient(IRestClientFactory clientFactory, IRestRequestFactory requestFactory,
                                 IShipWorksSession session, Func<Type, ILog> loggerFactory)
        {
            this.clientFactory = clientFactory;
            this.requestFactory = requestFactory;
            this.session = session;
            log = loggerFactory(typeof(HealthCheckClient));
        }

        /// <summary>
        /// Returns true if running, else false
        /// </summary>
        public bool IsRunning(long portNumber, bool useHttps)
        {
            IRestClient client = clientFactory.Create();
            string s = useHttps ? "s" : string.Empty;
            IRestRequest request =
                requestFactory.Create($"http{s}://{Environment.MachineName}:{portNumber}/shipworks/api/v1/healthcheck", Method.GET);
            request.Timeout = 2000;

            try
            {
                var response = client.Execute<HealthCheckResponse>(request);
                HttpStatusCode statusCode = response.StatusCode;

                log.Debug($"Api healthcheck response status {statusCode} on port ${portNumber}");

                if (response.StatusCode != HttpStatusCode.OK)
                {                    
                    return false;
                }

                if (response.Data.InstanceId != session.InstanceID)
                {
                    log.Debug($"Api healthcheck responded with wrong instance code: {response.Data.InstanceId}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Debug($"Exception while performing ShipWorks api healthcheck on portNumber {portNumber}", ex);
                return false;
            }
        }
    }
}
