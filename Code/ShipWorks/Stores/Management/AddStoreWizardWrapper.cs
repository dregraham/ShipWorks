using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Wrapper around the add store wizard
    /// </summary>
    [Component]
    public class AddStoreWizardWrapper : IAddStoreWizard
    {
        /// <summary>
        /// Run the setup wizard
        /// </summary>
        /// <remarks>
        /// Will return false if the user doesn't have permissions, the user canceled,
        /// or if the Wizard was not able to run because it was already running on another computer.
        /// </remarks>
        public bool RunWizard(IWin32Window owner, OpenedFromSource openedFrom) =>
            AddStoreWizard.RunWizard(owner, openedFrom);
    }
}
