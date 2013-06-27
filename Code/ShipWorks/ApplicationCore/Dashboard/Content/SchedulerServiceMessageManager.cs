﻿using System;
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
                List<WindowsServiceEntity> stoppedWindowsServices = new List<WindowsServiceEntity>();

                // Check if there are any scheduled actions available for any of the services to run
                IEnumerable<ActionEntity> scheduledActions = ActionManager.Actions.Where(a => a.TriggerType == (int)ActionTriggerType.Scheduled);

                // If there are scheduled actions, check to see if any are running.
                // NOTE: When we implement specifying actions to run on specific machines, we'll need to add that to the filter here.
                if (scheduledActions.Any())
                {
                    // Force a db check since the service is running in another process and our local cache will not be updated.
                    WindowsServiceManager.CheckForChangesNeeded();

                    // Find scheduler services that are older than 10 minutes.
                    stoppedWindowsServices = WindowsServiceManager.WindowsServices.Where(ws => ws.ServiceType == (int)ShipWorksServiceType.Scheduler &&
                                                                                              (!ws.LastCheckInDateTime.HasValue ||
                                                                                                ws.LastCheckInDateTime.Value.AddMinutes(WindowsServiceManager.NotRunningTimeSpan.TotalMinutes) <= DateTime.UtcNow)).ToList();
                }

                return stoppedWindowsServices;
            }
        }
    }
}
