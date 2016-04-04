using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for the ChannelLimitDlgFactory
    /// </summary>
    public interface IChannelLimitDlgFactory
    {
        /// <summary>
        /// Gets the channel limit dialog.
        /// </summary>
        IDialog GetChannelLimitDlg(IWin32Window owner, EditionFeature feature, EnforcementContext context);
    }
}