using System.Windows.Forms;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// WinForms shell for hosting the Confirmation XAML
    /// </summary>
    /// <remarks>This is required for us to receive messages from our message filter</remarks>
    public partial class AutoPrintConfirmationDialog : Form, IAutoPrintConfirmationDialog
    {
        private readonly IAutoPrintConfirmationDlgViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintConfirmationDialog(IAutoPrintConfirmationDlgViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            viewModel.Close = SetDlgResultAndClose;
            confirmationControl.DataContext = viewModel;
        }

        /// <summary>
        /// Wire up close and return dialog result
        /// </summary>
        private void SetDlgResultAndClose(bool confirm)
        {
            DialogResult = confirm ? DialogResult.OK : DialogResult.Cancel;
            viewModel.Dispose();
            Close();
        }
    }
}
