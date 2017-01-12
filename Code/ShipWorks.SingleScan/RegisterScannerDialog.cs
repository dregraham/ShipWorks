using System.Windows;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Dialog to prompt user to register scanner.
    /// </summary>
    [NamedComponent("RegisterScannerDialog", typeof(Form))]
    public partial class RegisterScannerDialog : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterScannerDialog(IRegisterScannerControlViewModel viewModel)
        {
            viewModel.CloseDialog = Close;
            InitializeComponent();
            registrationControl.DataContext = viewModel;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
