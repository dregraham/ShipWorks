using System;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using System.Net;
using log4net;
using ShipWorks.Api.Configuration;
using Autofac;
using ShipWorks.Api.HealthCheck;

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
        private IApiStartupConfiguration apiStartup;
        private readonly Func<IApiStartupConfiguration> apiStartupFactory;
        private readonly IHealthCheckClient healthCheckClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(Func<IApiStartupConfiguration> apiStartupFactory, 
            IHealthCheckClient healthCheckClient, 
            Func<Type, ILog> loggerFactory)
        {
            log = loggerFactory(typeof(ApiService));
            this.apiStartupFactory = apiStartupFactory;
            this.healthCheckClient = healthCheckClient;
        }

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        public void Start()
        {
            if (!healthCheckClient.IsRunning())
            {
                if (server != null)
                {
                    server.Dispose();
                    apiStartup.Dispose();
                }
                try
                {
                    apiStartup = apiStartupFactory();
                    server = WebApp.Start("http://+:8081/", apiStartup.Configuration);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while starting ShipWorks Api", ex);
                }
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
