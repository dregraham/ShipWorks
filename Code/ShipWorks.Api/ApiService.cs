using System;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using System.Timers;
using ShipWorks.Api.Configuration;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Api.Infrastructure;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Api
{
    /// <summary> 
    /// An local web server leveraging Owin infrastructure that can be
    /// self-hosted within ShipWorks.
    /// </summary>
    [Component(SingleInstance = true)]
    public class ApiService : IInitializeForCurrentDatabase, IApiService
    {
        private const double TimerInterval = 5000;
        private IDisposable server;
        private bool isDisposing;
        private readonly ILog log;
        private IApiStartupConfiguration apiStartup;
		private readonly ITimer timer;
        private readonly IApiSettingsRepository settingsRepository;
        private readonly Func<IApiStartupConfiguration> apiStartupFactory;
        private readonly IHealthCheckClient healthCheckClient;
        private readonly IWebApp webApp;

        private long? port;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiService(Func<IApiStartupConfiguration> apiStartupFactory,
            IHealthCheckClient healthCheckClient,
            IWebApp webApp,
            ITimer timer,
            IApiSettingsRepository settingsRepository,
            Func<Type, ILog> loggerFactory)
        {
            this.timer = timer;
            this.settingsRepository = settingsRepository;
            timer.Interval = TimerInterval;
            log = loggerFactory(typeof(ApiService));
            this.apiStartupFactory = apiStartupFactory;
            this.healthCheckClient = healthCheckClient;
            this.webApp = webApp;
        }

        /// <summary>
        /// Is the service running
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// The port the service is currently running on
        /// </summary>
        public long? Port => port;

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

            var settings = settingsRepository.Load();
            if (settings.Enabled)
            {
                StopIfPortChanged(settings);
                StartIfNotRunning();
            }
            else
            {
                Stop();
            }            

            timer.Start();
        }

        /// <summary>
        /// Stop If Port Changed
        /// </summary>
        private void StopIfPortChanged(ApiSettings settings)
        {
            if (port.HasValue && port != settings.Port)
            {
                IsRunning = false;
                Stop();
            }

            port = settings.Port;
        }

        /// <summary>
        /// Start the ShipWorks API
        /// </summary>
        private void StartIfNotRunning()
        {
            if (healthCheckClient.IsRunning(port.Value))
            {
                IsRunning = true;
            }
            else
            {
                IsRunning = false;

                Stop();

                try
                {
                    apiStartup = apiStartupFactory();
                    server = webApp.Start($"http://+:{port}/", apiStartup.Configuration);
                    log.Info("ShipWorks.API has started");
                    IsRunning = true;
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
