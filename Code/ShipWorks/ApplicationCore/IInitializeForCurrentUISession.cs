namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Service initialization for UI sessions
    /// </summary>
    public interface IInitializeForCurrentUISession
    {
        /// <summary>
        /// Initialize for the current session
        /// </summary>
        void InitializeForCurrentSession();
    }
}
