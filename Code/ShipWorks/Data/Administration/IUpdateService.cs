using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;

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
        /// update shipworks
        /// </summary>
        /// <returns></returns>
        Result Update(Version version);

        /// <summary>
        /// Send a message to the update service
        /// </summary>
        Result SendMessage(string message);

        /// <summary>
        /// Listen for the auto update to start
        /// </summary>
        void ListenForAutoUpdateStart(IMainForm mainForm);
    }
}