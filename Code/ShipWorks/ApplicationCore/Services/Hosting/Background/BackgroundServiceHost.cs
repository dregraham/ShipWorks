using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ShipWorks.ApplicationCore.Crashes;
using System.Collections.Generic;


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
        /// Handles a crash in the background process by trying to restart the process ("Phoenix" mode).
        /// </summary>
        /// <param name="serviceCrash">The service crash.</param>
        public void HandleServiceCrash(ServiceCrash serviceCrash)
        {
            log.InfoFormat("Attempting to restart the '{0}' background service.", service.ServiceName);

            try
            {
                // We want to restart using the same arguments as before, except incrementing the value of the recovery attempts
                // to let the new process know it is being started as an attempt to recover from a crash
                List<string> commandArgs = Environment.GetCommandLineArgs().ToList().Where(s => !s.Contains("recovery")).ToList();
                commandArgs.Add(string.Format("/recovery={0}", serviceCrash.ServiceExecutionMode.RecoveryAttempts + 1));

                ProcessStartInfo restartInfo = new ProcessStartInfo
                {
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
