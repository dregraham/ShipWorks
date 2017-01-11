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
            viewmodel.CloseDialog = CloseDialog;
            InitializeComponent();
            registrationControl.DataContext = this.viewmodel;
            StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="result">The result.</param>
        private void CloseDialog(DialogResult result)
        {
            DialogResult = result;
        }
    }
}
