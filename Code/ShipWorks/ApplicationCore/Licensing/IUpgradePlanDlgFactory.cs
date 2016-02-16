using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IUpgradePlanDlgFactory
    {
        IDialog Create(string message, IWin32Window owner);
    }
}