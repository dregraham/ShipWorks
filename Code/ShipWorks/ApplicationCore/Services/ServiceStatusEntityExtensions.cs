using System.Collections.Generic;
using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Linq;
using ShipWorks.Users;


namespace ShipWorks.ApplicationCore.Services
{
    public static class ServiceStatusEntityExtensions
    {
        /// <summary>
        /// Gets the run status of service.
        /// </summary>
        /// <param name="instance">The service instance.</param>
        /// <returns>The service status.</returns>
        public static ServiceStatus GetStatus(this ServiceStatusEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!instance.LastStartDateTime.HasValue)
            {
                return ServiceStatus.NeverStarted;
            }

            if (instance.LastStopDateTime.HasValue &&
                instance.LastStopDateTime > instance.LastStartDateTime)
            {
                return ServiceStatus.Stopped;
            }

            if (!instance.LastCheckInDateTime.HasValue || 
                instance.LastCheckInDateTime.Value <= DateTime.UtcNow.Add(-ServiceStatusManager.NotRunningTimeSpan))
            {
                return ServiceStatus.NotResponding;
            }

            return ServiceStatus.Running;
        }
    }
}
