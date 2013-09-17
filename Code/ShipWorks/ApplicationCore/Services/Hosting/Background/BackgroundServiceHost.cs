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

        /// <summary>
        /// Constructs a new instance with the given service
        /// </summary>
        public BackgroundServiceHost(ShipWorksServiceBase service)
        {
            if (null == service)
            {
                throw new ArgumentNullException("service");
            }

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
            BackgroundServiceController.RunInBackground(service);
        }

        /// <summary>
        /// Signals the background process to stop.
        /// </summary>
        public void Stop()
        {
            log.InfoFormat("Stopping the '{0}' background service.", service.ServiceName);
            BackgroundServiceController.StopInBackground(service.ServiceType);
        }

        /// <summary>
        /// Handles a crash in the background process by trying to restart the process ("Phoenix" mode).
        /// </summary>
        /// <param name="serviceCrash">The service crash.</param>
        public void HandleServiceCrash(int recoveryCount)
        {
            int minutesToWait = Math.Min(recoveryCount + 1, 10);

            // Wait a minute for each time we've tried to recover, up to 10 minutes
            log.InfoFormat("Waiting {0} minutes to reluanch afterbackground service after crash.", minutesToWait);

            // Without this we could end up in a tight crash loop
            Thread.Sleep(TimeSpan.FromMinutes(minutesToWait));

            try
            {
                // We want to restart using the same arguments as before, except incrementing the value of the recovery attempts
                // to let the new process know it is being started as an attempt to recover from a crash
                List<string> commandArgs = Environment.GetCommandLineArgs().ToList().Where(s => !s.Contains("recovery")).ToList();
                commandArgs.Add(string.Format("/recovery={0}", recoveryCount + 1));

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
