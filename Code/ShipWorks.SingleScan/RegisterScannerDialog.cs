using System.Windows;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    [NamedComponent("RegisterScannerDialog", typeof(Form))]
    public partial class RegisterScannerDialog : Form
    {
        private readonly IRegisterScannerDlgViewModel viewmodel;

        public RegisterScannerDialog(IRegisterScannerDlgViewModel viewmodel)
        {
            this.viewmodel = viewmodel;
            viewmodel.CloseDialog = Close;
            InitializeComponent();
            registrationControl.DataContext = this.viewmodel;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
