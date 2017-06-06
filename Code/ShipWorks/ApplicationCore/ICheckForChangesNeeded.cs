using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Check for any changes made in the database since initialization or the last check
    /// </summary>
    [Service(SingleInstance = true)]
    public interface ICheckForChangesNeeded
    {
        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        void CheckForChangesNeeded();
    }
}
