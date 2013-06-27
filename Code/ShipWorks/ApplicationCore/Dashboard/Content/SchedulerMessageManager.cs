using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public class SchedulerMessageManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerMessageManager"/> class.
        /// </summary>
        public SchedulerMessageManager()
        { }

        /// <summary>
        /// Gets the stopped scheduling services by based on the last checked-in time of the service and
        /// whether there are any scheduled actions the machine could potentially execute. Any service that 
        /// hasn't checked in within 10 minutes is considered stopped.
        /// </summary>
        public List<WindowsServiceEntity> StoppedSchedulingServices
        {
            get
            {
                return new List<WindowsServiceEntity>();
            }
        }
    }
}
