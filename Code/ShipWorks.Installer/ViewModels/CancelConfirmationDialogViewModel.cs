using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Installer.Views;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the CancelConfirmationDialog
    /// </summary>
    public class CancelConfirmationDialogViewModel : ViewModelBase
    {
        MainViewModel mainViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public CancelConfirmationDialogViewModel(MainViewModel mainViewModel, MainWindow mainWindow)
        {
            this.mainViewModel = mainViewModel;
            Owner = mainWindow;
            NoCommand = new RelayCommand<Window>((Window dlg) => dlg.Close());
            YesCommand = new RelayCommand<Window>(ConfirmClose);
            DialogResult = false;
        }

        /// <summary>
        /// Command for the Yes button
        /// </summary>
        public ICommand YesCommand { get; }

        /// <summary>
        /// Command for the No button
        /// </summary>
        public ICommand NoCommand { get; }

        /// <summary>
        /// The result of the dialog
        /// </summary>
        public bool? DialogResult { get; private set; }

        /// <summary>
        /// The window that owns this dialog
        /// </summary>
        public MainWindow Owner { get; private set; }

        /// <summary>
        /// Sets the IsClosing property of the main view model
        /// </summary>
        public void SetClosing(bool value) => mainViewModel.IsClosing = value;

        /// <summary>
        /// Confirms the close, does any rollbacks that are needed and closes the window
        /// </summary>
        private void ConfirmClose(Window dlg)
        {
            if (!mainViewModel.IsClosing)
            {
                mainViewModel.IsClosing = true;
                Owner.Close();
            }
            DialogResult = true;
            dlg.Close();
        }
    }
}
