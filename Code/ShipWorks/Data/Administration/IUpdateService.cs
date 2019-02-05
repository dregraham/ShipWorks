using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Represents the UpdateService
    /// </summary>
    [Service]
    public interface IUpdateService : IDisposable
    {
        /// <summary>
        /// Check to see if the update service is available
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Tell the service to update ShipWorks
        /// </summary>
        Result Update(Version version);
    }
}