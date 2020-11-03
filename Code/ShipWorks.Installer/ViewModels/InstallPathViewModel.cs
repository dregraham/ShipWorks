using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using log4net;
using Ookii.Dialogs.Wpf;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the installation path selection page
    /// </summary>
    [Obfuscation]
    public class InstallPathViewModel : InstallerViewModelBase
    {
        private string installPath;
        private ISystemCheckService systemCheckService;
        private readonly IRegistryService registryService;
        private readonly ILog log;
        private string error;
        private bool createShortcut;

        /// <summary>
        /// Constructor
        /// </summary>
        public InstallPathViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            ISystemCheckService systemCheckService,
            IRegistryService registryService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UpgradeShipWorks)
        {
            log = logFactory(typeof(InstallPathViewModel));
            log.Info($"Starting InstallPathViewModel");

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

            log.Info($"NextExecute: InstallPath: {InstallPath}");
            log.Info($"NextExecute: CreateShortcut: {CreateShortcut}");
            log.Info($"NextExecute: NextPage: {NextPage}");

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
                var pathWithBackslashes = value.Replace('/', '\\');
                value = pathWithBackslashes.EndsWith('\\') ? pathWithBackslashes : pathWithBackslashes + "\\";
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
            log.Info("Opening install path file browser");
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = InstallPath;
            dialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                log.Info($"User selected path: {dialog.SelectedPath}");
                InstallPath = dialog.SelectedPath;
            }
            ValidatePath();
        }

        /// <summary>
        /// Validates that the provided path meets the minimum requirements
        /// </summary>
        private bool ValidatePath()
        {
            log.Info($"Validating path {InstallPath}");
            string driveLetter = Path.GetPathRoot(InstallPath);
            if (!systemCheckService.DriveMeetsRequirements(driveLetter))
            {
                Error = $"Drive {driveLetter} does not meet the minimum space requirement of 20GB.";
                return false;
            }

            try
            {
                bool alreadyExists = Directory.Exists(InstallPath);

                // Use this to validate the install path
                // If the folder already exists, CreateDirectory doesn't do anything
                Directory.CreateDirectory(InstallPath);

                if (!alreadyExists)
                {
                    Directory.Delete(InstallPath);
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException)
            {
                Error = "The path entered is invalid.";
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            Error = null;
            return true;
        }

        /// <summary>
        /// Determines if the provided path already contains a SW installation
        /// </summary>
        private bool PathContainsShipWorks()
        {
            log.Info($"Checking path {InstallPath} for existing ShipWorks installation");
            if (Directory.Exists(InstallPath))
            {
                log.Info("Found existing ShipWorks installation");
                return Directory.GetFiles(InstallPath).Any(f => f == Path.Combine(InstallPath, "ShipWorks.exe"));
            }

            log.Info("Could not find existing ShipWorks installation");
            return false;
        }
    }
}
