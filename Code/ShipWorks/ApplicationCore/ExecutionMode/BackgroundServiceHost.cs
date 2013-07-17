using log4net;
using ShipWorks.ApplicationCore.WindowsServices;
using System;
using System.Threading;


namespace ShipWorks.ApplicationCore.ExecutionMode
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

        public void OnUnhandledException(Exception exception)
        {
            //TODO: phoenix mode
            log.Fatal("And the phoenix shall rise again... well, once it's implemented.");
        }
    }
}
