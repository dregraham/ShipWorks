using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Linq;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    public static class WindowsServiceEntityExtensions
    {
        /// <summary>
        /// Gets the run status of service.
        /// </summary>
        /// <param name="instance">The service instance.</param>
        /// <returns>The service status.</returns>
        public static ServiceStatus GetStatus(this WindowsServiceEntity instance)
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            if (!instance.LastStartDateTime.HasValue)
                return ServiceStatus.NeverStarted;

            if (
                instance.LastStopDateTime.HasValue &&
                instance.LastStopDateTime > instance.LastStartDateTime
            )
                return ServiceStatus.Stopped;

            if (
                !instance.LastCheckInDateTime.HasValue ||
                instance.LastCheckInDateTime.Value <= DateTime.UtcNow.Add(-WindowsServiceManager.NotRunningTimeSpan)
            )
                return ServiceStatus.NotResponding;

            return ServiceStatus.Running;
        }

        /// <summary>
        /// Determines whether the service is required to be running.  This may be based on the current
        /// system state and thus can change based on external factors, so do not cache the result.
        /// </summary>
        /// <param name="instance">The service instance.</param>
        /// <returns>true if the service is required; otherwise false.</returns>
        public static bool IsRequiredToRun(this WindowsServiceEntity instance)
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            if (instance.ServiceType == (int)ShipWorksServiceType.Scheduler)
            {
                bool noOtherSchedulersAreRunning = WindowsServiceManager.WindowsServices
                    .Where(s => s.ServiceType == (int)ShipWorksServiceType.Scheduler && !s.Equals(instance))
                    .All(s => s.GetStatus() != ServiceStatus.Running);

                var unrunnableActions = ActionManager.Actions
                    .Where(a =>
                        a.TriggerType == (int)ActionTriggerType.Scheduled &&
                        a.Enabled && (
                            (a.ComputerLimitedType == (int)ComputerLimitationType.None && noOtherSchedulersAreRunning) ||
                            (a.ComputerLimitedType == (int)ComputerLimitationType.NamedList && a.ComputerLimitedList.Contains(instance.ComputerID))
                        )
                    );

                return unrunnableActions.Any();
            }

            return false;
        }
    }
}
