using System;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using System.Timers;
using ShipWorks.Api.Configuration;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Api.Infrastructure;
using Interapptive.Shared.Utility;

namespace ShipWorks.Api
{
    /// <summary> 
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    public class ApiService : IInitializeForCurrentDatabase
    {
        private const double TimerInterval = 5000;
        private IDisposable server;
        private bool isDisposing;
        private readonly ILog log;
        private IApiStartupConfiguration apiStartup;
		private readonly ITimer timer;
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
            timer.Interval = TimerInterval;
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
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            StartIfNotRunning();

            timer.Start();
        }

        /// <summary>
        /// Start the ShipWorks API
        /// </summary>
        private void StartIfNotRunning()
        {
            if (!healthCheckClient.IsRunning())
            {
                Stop();

                try
                {
                    apiStartup = apiStartupFactory();
                    server = webApp.Start("http://+:8081/", apiStartup.Configuration);
                    log.Info("ShipWorks.API has started");
                }
                catch (Exception ex)
                {
                    log.Debug("Unable to start the ShipWorks API.", ex);
                }
            }
        }

        /// <summary>
        /// Stop the ShipWorks API
        /// </summary>
        private void Stop()
        {
            if (server != null)
            {
                server.Dispose();
                apiStartup?.Dispose();
                apiStartup = null;
                server = null;
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
                Stop();
            }
        }
    }
}
