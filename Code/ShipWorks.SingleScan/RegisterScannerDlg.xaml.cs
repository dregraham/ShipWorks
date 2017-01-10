using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Interaction logic for RegisterScannerDlg.xaml
    /// </summary>
    [NamedComponent("RegisterScannerDlg", typeof(InteropWindow))]
    public partial class RegisterScannerDlg
    {
        public RegisterScannerDlg(IWin32Window owner) : base(owner)
        {
            InitializeComponent();
        }
    }
}
