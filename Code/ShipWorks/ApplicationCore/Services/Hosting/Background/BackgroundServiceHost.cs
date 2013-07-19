using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;


namespace ShipWorks.ApplicationCore.Services.Hosting.Background
{
    /// <summary>
    /// Hosts a ShipWorks service as a background process.
    /// </summary>
    public class BackgroundServiceHost : IServiceHost
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceHost));

        readonly ShipWorksServiceBase service;

        public BackgroundServiceHost(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");
            this.service = service;
        }

        /// <summary>
        /// Gets the service being hosted.
        /// </summary>
        public ShipWorksServiceBase Service
        {
            get { return service; }
        }

        /// <summary>
        /// Runs the service as a background process.
        /// </summary>
        public void Run()
        {
            log.InfoFormat("Running the '{0}' background service.", service.ServiceName);
            ShipWorksServiceBase.RunInBackground(service);
        }

        /// <summary>
        /// Signals the background process to stop.
        /// </summary>
        public void Stop()
        {
            log.InfoFormat("Stopping the '{0}' background service.", service.ServiceName);
            ShipWorksServiceBase.StopInBackground(service.ServiceType);
        }

        /// <summary>
        /// Restarts the background process ("phoenix mode").
        /// </summary>
        public void OnUnhandledException(Exception exception)
        {
            log.InfoFormat("Attempting to restart the '{0}' background service.", service.ServiceName);
            try
            {
                var commandArgs = Environment.GetCommandLineArgs();

                var restartInfo = new ProcessStartInfo {
                    FileName = commandArgs[0],
                    Arguments = string.Join(" ", commandArgs.Skip(1))
                };

                Process.Start(restartInfo);
                log.Info("Restart succeeded.");
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                log.Error("Restart failed.", ex);
            }
        }
    }
}
