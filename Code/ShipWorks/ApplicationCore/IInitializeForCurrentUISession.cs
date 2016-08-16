using System;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Service initialization for UI sessions
    /// </summary>
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
