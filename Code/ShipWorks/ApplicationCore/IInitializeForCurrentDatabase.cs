namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Mark a class as needing initialization
    /// </summary>
    public interface IInitializeForCurrentDatabase
    {
        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        void InitializeForCurrentDatabase(ExecutionMode.ExecutionMode executionMode);
    }
}
