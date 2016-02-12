using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitDlgFactory
    {
        IChannelLimitDlg GetChannelLimitDlg(ICustomerLicense customerLicense, IWin32Window owner);
    }
}