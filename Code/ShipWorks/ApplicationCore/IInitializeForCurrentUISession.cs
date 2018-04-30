using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Service initialization for UI sessions
    /// </summary>
    [Service]
    public interface IInitializeForCurrentUISession : IDisposable
    {
        /// <summary>
        /// Initialize for the current session
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// End the current session
        /// </summary>
        void EndSession();
    }
}
