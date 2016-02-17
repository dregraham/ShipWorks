using System.Windows.Forms;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitDlgFactory
    {
        IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner, EditionFeature feature);
    }
}