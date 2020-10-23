using System;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using Ookii.Dialogs.Wpf;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class InstallPathViewModel : InstallerViewModelBase
    {
        private string installPath;

        public InstallPathViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.Login)
        {
            BrowseCommand = new RelayCommand(Browse);
        }

        /// <summary>
        /// Go to next page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.IsFreshInstall = true;
            mainViewModel.InstallPathIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
            InstallPath = "C:\\Program Files\\ShipWorks";
        }


        /// <summary>
        /// Can go to next page
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Command for opening a file dialog
        /// </summary>
        public ICommand BrowseCommand { get; }

        /// <summary>
        /// The path to install ShipWorks to
        /// </summary>
        public string InstallPath
        {
            get => installPath;
            set => Set(ref installPath, value);
        }

        /// <summary>
        /// Opens a browser dialog and sets InstallPath
        /// </summary>
        private void Browse()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.ProgramFiles;
            dialog.ShowDialog();
            InstallPath = dialog.SelectedPath;
        }

        private void ValidatePath()
        {

        }
    }
}
