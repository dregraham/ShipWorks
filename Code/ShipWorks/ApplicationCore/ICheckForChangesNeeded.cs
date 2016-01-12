namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Check for any changes made in the database since initialization or the last check
    /// </summary>
    public interface ICheckForChangesNeeded
    {
        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        void CheckForChangesNeeded();
    }
}
