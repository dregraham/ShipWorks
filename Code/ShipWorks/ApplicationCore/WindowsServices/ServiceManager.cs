using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ShipWorks.Actions.Scheduling;
using log4net;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    public class ServiceManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ServiceManager));

        private readonly ShipWorksSchedulerService schedulerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceManager"/> class.
        /// </summary>
        /// <param name="schedulerService">The scheduler service.</param>
        public ServiceManager(ShipWorksSchedulerService schedulerService)
        {
            this.schedulerService = schedulerService;
        }

        public void StartService()
        {
            ServiceController service = new ServiceController(schedulerService.ServiceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch(Exception ex)
            {
                log.Error("Can't start service " + schedulerService.ServiceName, ex);

                throw;
            }
        }

    }
}
