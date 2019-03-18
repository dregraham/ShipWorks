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
    public interface IUpdateService
    {
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
        /// Listen for the auto update to start
        /// </summary>
        void ListenForAutoUpdateStart(IMainForm mainForm);
    }
}