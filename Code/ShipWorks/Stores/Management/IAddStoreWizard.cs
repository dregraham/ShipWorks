using System.Windows.Forms;
using Interapptive.Shared.Metrics;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Add a new store
    /// </summary>
    public interface IAddStoreWizard
    {
        /// <summary>
        /// Run the setup wizard
        /// </summary>
        /// <remarks>
        /// Will return false if the user doesn't have permissions, the user canceled, or if the Wizard was not able to run because
        /// it was already running on another computer.
        /// </remarks>
        bool RunWizard(IWin32Window owner, OpenedFromSource openedFrom);
    }
}