using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public static class SchedulerServiceMessageManager
    {
        /// <summary>
        /// Gets the stopped scheduling services by based on the last checked-in time of the service and
        /// whether there are any scheduled actions the machine could potentially execute. Any service that 
        /// hasn't checked in within configured timespan is considered stopped.
        /// </summary>
        public static List<WindowsServiceEntity> StoppedWindowsServices
        {
            get
            {
                // Force a db check since the service is running in another process and our local cache will not be updated.
                WindowsServiceManager.CheckForChangesNeeded();

                // Find required scheduler services that are not running.
                return WindowsServiceManager.WindowsServices
                    .Where(ws =>
                        ws.ServiceType == (int)ShipWorksServiceType.Scheduler &&
                        ws.GetStatus() != ServiceStatus.Running &&
                        ws.IsRequiredToRun()
                    )
                    .ToList();
            }
        }
    }
}
