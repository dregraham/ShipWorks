using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitDlgFactory
    {
        IChannelLimitDlg GetChannelLimitDlg(IWin32Window owner);
    }
}