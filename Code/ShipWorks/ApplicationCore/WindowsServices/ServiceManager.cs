using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ShipWorks.Actions.Scheduling;
using log4net;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    /// <summary>
    /// Manages the starting and stopping of a ShipWorks service.
    /// </summary>
    public class ShipWorksServiceManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksServiceManager));

        private readonly ShipWorksServiceBase shipWorksService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksServiceManager" /> class.
        /// </summary>
        /// <param name="shipWorksService">The service to manage.</param>
        public ShipWorksServiceManager(ShipWorksServiceBase shipWorksService)
        {
            this.shipWorksService = shipWorksService;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        public void StartService()
        {
            try
            {
                using (ServiceController service = new ServiceController(shipWorksService.ServiceName))
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch (Exception ex)
            {
                log.Error("Can't start service " + shipWorksService.ServiceName, ex);

                throw;
            }
        }
    }
}
