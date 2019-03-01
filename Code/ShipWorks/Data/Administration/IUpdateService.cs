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
        bool IsAvailable();

        /// <summary>
        /// Try to update shipworks
        /// </summary>
        /// <returns></returns>
        Result TryUpdate();

        /// <summary>
        /// Send a message to the update service
        /// </summary>
        Result SendMessage(string message);
    }
}