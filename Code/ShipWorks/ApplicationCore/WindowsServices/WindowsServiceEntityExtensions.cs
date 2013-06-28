using ShipWorks.Data.Model.EntityClasses;
using System;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    public static class WindowsServiceEntityExtensions
    {
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
                instance.LastCheckInDateTime.Value <= DateTime.UtcNow.AddMinutes(-10)
            )
                return ServiceStatus.NotResponding;

            return ServiceStatus.Running;
        }
    }
}
