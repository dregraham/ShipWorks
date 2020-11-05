using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Views;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the CancelConfirmationDialog
    /// </summary>
    public class CancelConfirmationDialogViewModel : ViewModelBase
    {
        MainViewModel mainViewModel;
        private readonly IInnoSetupService innoSetupService;
        private readonly ILog log;
        private bool loading;

        /// <summary>
        /// Constructor
        /// </summary>
        public CancelConfirmationDialogViewModel(MainViewModel mainViewModel,
            MainWindow mainWindow,
            IInnoSetupService innoSetupService,
            Func<Type, ILog> logFactory)
        {
            this.mainViewModel = mainViewModel;
            Owner = mainWindow;
            this.innoSetupService = innoSetupService;
            NoCommand = new RelayCommand<Window>((Window dlg) => dlg.Close());
            YesCommand = new RelayCommand<Window>(ConfirmClose);
            DialogResult = false;
            log = logFactory(typeof(CancelConfirmationDialogViewModel));
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
        /// Whether or not we are performing a rollback
        /// </summary>
        public bool Loading
        {
            get => loading;
            set => Set(ref loading, value);
        }

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
        private async void ConfirmClose(Window dlg)
        {
            if (mainViewModel.InstallSettings.NeedsRollback)
            {
                Loading = true;
                try
                {
                    await innoSetupService.RunUninstaller().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.Error("Error rolling back changes", ex);
                }
                finally
                {
                    Loading = false;
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!mainViewModel.IsClosing)
                {
                    mainViewModel.IsClosing = true;
                    Owner.Close();
                }
                DialogResult = true;
                dlg.Close();
            });
        }
    }
}
