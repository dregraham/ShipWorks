using System;
using System.IO;
using System.Linq;
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
        private ISystemCheckService systemCheckService;
        private string error;

        public InstallPathViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService, ISystemCheckService systemCheckService) :
            base(mainViewModel, navigationService, NavigationPageType.UpgradeShipWorks)
        {
            BrowseCommand = new RelayCommand(Browse);
            InstallPath = "C:\\Program Files\\ShipWorks";
            this.systemCheckService = systemCheckService;
            ValidatePath();
        }

        /// <summary>
        /// Go to next page
        /// </summary>
        protected override void NextExecute()
        {
            if (!PathContainsShipWorks())
            {
                mainViewModel.NavBarState = Enums.NavBarState.NewInstall;
                NextPage = NavigationPageType.Login;
            }
            else
            {
                mainViewModel.NavBarState = Enums.NavBarState.Upgrade;
            }
            mainViewModel.InstallPathIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
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
        /// The error to display
        /// </summary>
        public string Error
        {
            get => error;
            set => Set(ref error, value);
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
            ValidatePath();
        }

        /// <summary>
        /// Validates that the provided path meets the minimum requirements
        /// </summary>
        private void ValidatePath()
        {
            string driveLetter = InstallPath.Substring(0, 3);
            if (!systemCheckService.DriveMeetsRequirements(driveLetter))
            {
                Error = $"Drive {driveLetter} does not meet the minimum space requirement of 20GB.";
            }
            else
            {
                Error = null;
            }
        }

        /// <summary>
        /// Determines if the provided path already contains a SW installation
        /// </summary>
        /// <returns></returns>
        private bool PathContainsShipWorks()
        {
            if (Directory.Exists(InstallPath))
            {
                return Directory.GetFiles(InstallPath).Any(f => f == Path.Combine(InstallPath, "ShipWorks.exe"));
            }

            return false;
        }
    }
}
