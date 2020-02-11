using System;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using System.Net;
using log4net;
using ShipWorks.Api.Configuration;

namespace ShipWorks.Api
{
    /// <summary> 
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    [Component(SingleInstance = true)]
    public class ApiService : IApiService
    {
        private IDisposable server;
        private bool isDisposing;
        private readonly ILog log;
        private readonly IApiStartupConfiguration apiStartup;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(IApiStartupConfiguration apiStartup, Func<Type, ILog> loggerFactory)
        {
            log = loggerFactory(typeof(ApiService));
            this.apiStartup = apiStartup;
        }

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        public void Start()
        {
            if (server == null && !IsRunning())
            {
                try
                {
                    server = WebApp.Start("http://+:8081/", apiStartup.Configuration);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while starting ShipWorks Api", ex);
                }
            }
        }

        /// <summary>
        /// Check to see if there is an ApiSrvice running
        /// </summary>
        /// <returns></returns>
        private bool IsRunning()
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

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                server?.Dispose();

                server = null;
            }
        }
    }
}
