namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Mark a class as needing initialization
    /// </summary>
    public interface IInitializeForCurrentSession
    {
        /// <summary>
        /// Initialize for the current session
        /// </summary>
        void InitializeForCurrentSession();
    }
}
