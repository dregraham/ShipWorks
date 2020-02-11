using System;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Calls HealthCheck Endpoint
    /// </summary>
    [Component(SingleInstance = true)]
    public class HealthCheckClient : IHealthCheckClient
    {
        private readonly ILog log;

        /// <summary>
        /// Returns true if running, else false
        /// </summary>
        public HealthCheckClient(Func<Type, ILog> loggerFactory)
        {
            log = loggerFactory(typeof(HealthCheckClient));
        }

        /// <summary>
        /// Returns true if running, else false
        /// </summary>
        public bool IsRunning()
        {
            WebRequest request = WebRequest.Create($"http://{Environment.MachineName}/shipworks/api/v1/healthcheck");
            request.Timeout = 2000;

            try
            {
                HttpStatusCode statusCode = ((HttpWebResponse) request.GetResponse()).StatusCode;

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
