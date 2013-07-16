using log4net;
using ShipWorks.ApplicationCore.WindowsServices;
using System;
using System.ServiceProcess;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public class WindowsServiceExecutionStrategy : IServiceExecutionStrategy
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceExecutionStrategy));

        readonly ShipWorksServiceBase service;

        public WindowsServiceExecutionStrategy(ShipWorksServiceBase service)
        {
            if (null == service)
                throw new ArgumentNullException("service");
            this.service = service;
        }

        /// <summary>
        /// Sends a start command to the service manager if run interactively; otherwise, runs the service.
        /// </summary>
        public void Run()
        {
            if (Environment.UserInteractive)
            {
                var manager = new ShipWorksServiceManager(service);

                log.InfoFormat("Starting the '{0}' Windows service via the service manager.", service.ServiceName);
                if (manager.GetServiceStatus() == ServiceControllerStatus.Stopped)
                    manager.StartService();
            }
            else
            {
                log.InfoFormat("Running the '{0}' Windows service.", service.ServiceName);
                ShipWorksServiceBase.Run(service);
            }
        }

        /// <summary>
        /// Sends a stop command to the service manager.
        /// </summary>
        public void Stop()
        {
            var manager = new ShipWorksServiceManager(service);

            log.InfoFormat("Stopping the '{0}' Windows service via the service manager.", service.ServiceName);
            if (manager.GetServiceStatus() == ServiceControllerStatus.Running)
                manager.StopService();
        }
    }
}
