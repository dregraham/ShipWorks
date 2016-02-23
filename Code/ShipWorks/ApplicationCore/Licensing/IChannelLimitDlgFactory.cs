using System.Windows.Forms;
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
        IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner, EditionFeature feature);
    }
}