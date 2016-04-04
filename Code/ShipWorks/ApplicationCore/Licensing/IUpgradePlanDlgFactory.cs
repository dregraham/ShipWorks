using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Prompts the user to upgrade
    /// </summary>
    public interface IUpgradePlanDlgFactory
    {
        /// <summary>
        /// Creates a dialog prompting the user to upgrade
        /// </summary>
        IDialog Create(string message, IWin32Window owner);
    }
}