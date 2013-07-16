using log4net;
using ShipWorks.ApplicationCore.WindowsServices;
using System;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public class BackgroundServiceExecutionStrategy : IServiceExecutionStrategy
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BackgroundServiceExecutionStrategy));

        readonly ShipWorksServiceBase service;

        public BackgroundServiceExecutionStrategy(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");
            this.service = service;
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
    }
}
