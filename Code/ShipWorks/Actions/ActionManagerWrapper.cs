using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration.Ordering;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Manages and provides access to the actions in the system
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 1)]
    public class ActionManagerWrapper : IActionManager, IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        /// <summary>
        /// Initialize table synchronizer
        /// </summary>
        public void InitializeForCurrentSession() => ActionManager.InitializeForCurrentSession();

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public void CheckForChangesNeeded() => ActionManager.CheckForChangesNeeded();
    }
}
