using System.Windows.Forms;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Interface for setup wizards that are used for One Balance accounts
    /// </summary>
    public interface IOneBalanceSetupWizard
    {
        /// <summary>
        /// Open an account setup wizard for adding a One Balance carrier account
        /// </summary>
        DialogResult SetupOneBalanceAccount(IWin32Window owner);
    }
}
