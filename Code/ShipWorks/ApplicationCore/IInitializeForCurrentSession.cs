using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Mark a class as needing initialization
    /// </summary>
    [Service(SingleInstance = true)]
    public interface IInitializeForCurrentSession
    {
        /// <summary>
        /// Initialize for the current session
        /// </summary>
        void InitializeForCurrentSession();
    }
}
