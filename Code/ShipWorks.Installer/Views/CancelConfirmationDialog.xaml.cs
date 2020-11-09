using System.Windows;
using ShipWorks.Installer.ViewModels;

namespace ShipWorks.Installer.Views
{
    /// <summary>
    /// Interaction logic for CancelConfirmationDialog.xaml
    /// </summary>
    public partial class CancelConfirmationDialog : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CancelConfirmationDialog()
        {
            InitializeComponent();
            var vm = this.DataContext as CancelConfirmationDialogViewModel;
            this.Owner = vm.Owner;
        }

        /// <summary>
        /// Event handler for the dialogs closed event
        /// </summary>
        private void ExitSetupDlg_Closed(object sender, System.EventArgs e)
        {
            var vm = this.DataContext as CancelConfirmationDialogViewModel;
            vm.SetClosing(false);
        }

        /// <summary>
        /// Event handler for the dialogs closing event
        /// </summary>
        private void ExitSetupDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = this.DataContext as CancelConfirmationDialogViewModel;
            if (!vm.Loading)
            {
                this.DialogResult = vm.DialogResult;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
