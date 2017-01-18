using System.Windows;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Dialog to prompt user to register scanner.
    /// </summary>
    [NamedComponent("ScannerRegistrationDialog", typeof(Form))]
    public partial class ScannerRegistrationDialog : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerRegistrationDialog(IScannerRegistrationControlViewModel viewModel)
        {
            viewModel.CloseDialog = Close;
            InitializeComponent();
            registrationRegistrationControl.DataContext = viewModel;
        }
    }
}
