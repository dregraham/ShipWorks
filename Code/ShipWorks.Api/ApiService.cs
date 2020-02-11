using System;
using Interapptive.Shared.ComponentRegistration;
using Microsoft.Owin.Hosting;
using System.Net;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using System.Timers;
using ShipWorks.Api.Configuration;
using Autofac;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Api.Infrastructure;
using Interapptive.Shared.Utility;

namespace ShipWorks.Api
{
    /// <summary> 
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    [Component(SingleInstance = true)]
    public class ApiService : IApiService, IInitializeForCurrentDatabase
    {
        double timerInterval = 5000;
        private IDisposable server;
        private bool isDisposing;
        private readonly ILog log;
        private IApiStartupConfiguration apiStartup;
		private ITimer timer;
        private readonly Func<IApiStartupConfiguration> apiStartupFactory;
        private readonly IHealthCheckClient healthCheckClient;
        private readonly IWebApp webApp;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(Func<IApiStartupConfiguration> apiStartupFactory,
            IHealthCheckClient healthCheckClient,
            IWebApp webApp,
            ITimer timer,
            Func<Type, ILog> loggerFactory)
        {
            this.timer = timer;
            timer.Interval = timerInterval;
            log = loggerFactory(typeof(ApiService));
            this.apiStartupFactory = apiStartupFactory;
            this.healthCheckClient = healthCheckClient;
            this.webApp = webApp;
        }

        /// <summary>
        /// Initialize the API for the current database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode)
        {
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        /// <summary>
        /// Ensure that the api is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            StartIfNotRunning();

            timer.Start();
        }

        /// <summary>
        /// Start the Shipworks Api
        /// </summary>
        private void StartIfNotRunning()
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
                    server = webApp.Start("http://+:8081/", apiStartup.Configuration);
                }
                catch (Exception ex)
                {
                    log.Debug("Unable to start the ShipWorks Api.", ex);
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
