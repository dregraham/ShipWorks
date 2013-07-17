using log4net;
using ShipWorks.ApplicationCore.WindowsServices;
using System;
using System.ServiceProcess;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Hosts a ShipWorks service as a Windows service.
    /// </summary>
    public class WindowsServiceHost : IServiceHost
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceHost));

        readonly ShipWorksServiceBase service;

        public WindowsServiceHost(ShipWorksServiceBase service)
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

        /// <summary>
        /// Does nothing.  The exception is automatically logged to the system event logs
        /// if AutoLog is enabled for the service.
        /// </summary>
        public void OnUnhandledException(Exception exception) { }
    }
}
