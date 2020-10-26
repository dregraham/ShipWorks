using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using Ookii.Dialogs.Wpf;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    [Obfuscation]
    public class InstallPathViewModel : InstallerViewModelBase
    {
        private string installPath;
        private ISystemCheckService systemCheckService;
        private readonly IRegistryService registryService;
        private string error;
        private bool createShortcut;

        public InstallPathViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            ISystemCheckService systemCheckService,
            IRegistryService registryService) :
            base(mainViewModel, navigationService, NavigationPageType.UpgradeShipWorks)
        {
            BrowseCommand = new RelayCommand(Browse);
            ValidatePathCommand = new RelayCommand(() => ValidatePath());
            this.systemCheckService = systemCheckService;
            this.registryService = registryService;
            InstallPath = registryService.GetInstallPath();
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
            mainViewModel.InstallSettings.InstallPath = InstallPath;
            mainViewModel.InstallSettings.CreateShortcut = CreateShortcut;
            navigationService.NavigateTo(NextPage);
        }


        /// <summary>
        /// Can go to next page
        /// </summary>
        protected override bool NextCanExecute() => ValidatePath();

        /// <summary>
        /// Command for opening a file dialog
        /// </summary>
        public ICommand BrowseCommand { get; }

        /// <summary>
        /// Command for validating the install path
        /// </summary>
        public ICommand ValidatePathCommand { get; }

        /// <summary>
        /// The path to install ShipWorks to
        /// </summary>
        public string InstallPath
        {
            get => installPath;
            set
            {
                value = value.EndsWith('\\') ? value : value + "\\";
                Set(ref installPath, value);
            }
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
        /// Whether or not to create a desktop shortcut
        /// </summary>
        public bool CreateShortcut
        {
            get => createShortcut;
            set => Set(ref createShortcut, value);
        }

        /// <summary>
        /// Opens a browser dialog and sets InstallPath
        /// </summary>
        private void Browse()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = InstallPath;
            dialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                InstallPath = dialog.SelectedPath;
            }
            ValidatePath();
        }

        /// <summary>
        /// Validates that the provided path meets the minimum requirements
        /// </summary>
        private bool ValidatePath()
        {
            string driveLetter = Path.GetPathRoot(InstallPath);
            if (!systemCheckService.DriveMeetsRequirements(driveLetter))
            {
                Error = $"Drive {driveLetter} does not meet the minimum space requirement of 20GB.";
                return false;
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Determines if the provided path already contains a SW installation
        /// </summary>
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
