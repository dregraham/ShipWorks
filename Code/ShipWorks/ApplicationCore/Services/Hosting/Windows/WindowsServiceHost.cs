using log4net;
using System;
using System.ServiceProcess;
using ShipWorks.ApplicationCore.Crashes;


namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Hosts a ShipWorks service as a Windows service.
    /// </summary>
    public class WindowsServiceHost : IServiceHost
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceHost));

        private readonly ShipWorksServiceBase service;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsServiceHost"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ArgumentNullException">service</exception>
        public WindowsServiceHost(ShipWorksServiceBase service)
        {
            if (service == null)
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
        /// Sends a start command to the service manager if run interactively; otherwise, runs the service.
        /// </summary>
        public void Run()
        {
            if (Environment.UserInteractive)
            {
                log.InfoFormat("Starting the '{0}' Windows service via the service manager.", service.ServiceName);

                WindowsServiceController controller = new WindowsServiceController(service);
                if (controller.GetServiceStatus() == ServiceControllerStatus.Stopped)
                {
                    controller.StartService();
                }
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
            WindowsServiceController controller = new WindowsServiceController(service);

            log.InfoFormat("Stopping the '{0}' Windows service via the service manager.", service.ServiceName);
            if (controller.GetServiceStatus() == ServiceControllerStatus.Running)
            {
                controller.StopService();
            }
        }

        /// <summary>
        /// Does nothing.  The exception is automatically logged to the system event logs
        /// if AutoLog is enabled for the service.
        /// </summary>
        public void HandleServiceCrash(int recoveryCount) 
        { 
        
        }
    }
}
