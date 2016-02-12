using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IUpgradePlanDlgFactory
    {
        IDialog Create(string message, ICustomerLicense customerLicense, IWin32Window owner);
    }
}