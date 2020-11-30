using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
using ShipWorks.Installer.Logging;
using ShipWorks.Installer.Models;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Views;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// Main view model
    /// </summary>
    [Obfuscation]
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService<NavigationPageType> navigationService;
        private readonly ILog log;
        private EFontAwesomeIcon eulaIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon installPathIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon loginIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon installationIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon systemCheckIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon locationConfigIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon installShipworksIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon installDatabaseIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon upgradeShipWorksIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon warningIcon = EFontAwesomeIcon.None;
        private EFontAwesomeIcon useShipWorksIcon = EFontAwesomeIcon.None;
        private InstallSettings installSettings;
        private NavBarState navBarState;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel(INavigationService<NavigationPageType> navigationService,
            IInnoSetupService innoSetupService,
            Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(MainViewModel));
            log.Info("Installer loaded");
            this.navigationService = navigationService;
            InstallSettings = new InstallSettings();
            NavBarState = NavBarState.Initial;
            HelpCommand = new RelayCommand(OpenHelpPage);
            OpenLogFolderCommand = new RelayCommand(OpenLogFolder);
            innoSetupService.DownloadInstaller(InstallSettings);
        }

        /// <summary>
        /// Command for help button click
        /// </summary>
        public ICommand HelpCommand { get; }

        /// <summary>
        /// Command for opening log folder in Windows Explorer
        /// </summary>
        public ICommand OpenLogFolderCommand { get; }

        /// <summary>
        /// The current install settings
        /// </summary>
        public InstallSettings InstallSettings
        {
            get => installSettings;
            set => Set(ref installSettings, value);
        }

        /// <summary>
        /// RAM meets requirements
        /// </summary>
        public Visibility FailedRequirements
        {
            get
            {
                return InstallSettings.CheckSystemResult == null ||
                       InstallSettings.CheckSystemResult.AllRequirementsMet
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        /// <summary>
        /// Whether this is a new installation or an upgrade
        /// </summary>
        public NavBarState NavBarState
        {
            get => navBarState;
            set => Set(ref navBarState, value);
        }

        /// <summary>
        /// Whether or not the window is in the process of closing
        /// </summary>
        public bool IsClosing { get; set; }

        /// <summary>
        /// Exposes the navigation service so we can bind to its CurrentPageKey
        /// </summary>
        public INavigationService<NavigationPageType> NavigationService => navigationService;

        /// <summary>
        /// Eula icon
        /// </summary>
        public EFontAwesomeIcon EulaIcon
        {
            get => eulaIcon;
            set => Set(ref eulaIcon, value);
        }

        /// <summary>
        /// Install Path icon
        /// </summary>
        public EFontAwesomeIcon InstallPathIcon
        {
            get => installPathIcon;
            set => Set(ref installPathIcon, value);
        }

        /// <summary>
        /// Login icon
        /// </summary>
        public EFontAwesomeIcon LoginIcon
        {
            get => loginIcon;
            set => Set(ref loginIcon, value);
        }

        /// <summary>
        /// Installation icon
        /// </summary>
        public EFontAwesomeIcon InstallationIcon
        {
            get => installationIcon;

            set => Set(ref installationIcon, value);
        }

        /// <summary>
        /// System check icon
        /// </summary>
        public EFontAwesomeIcon SystemCheckIcon
        {
            get => systemCheckIcon;
            set => Set(ref systemCheckIcon, value);
        }

        /// <summary>
        /// Location Config icon
        /// </summary>
        public EFontAwesomeIcon LocationConfigIcon
        {
            get => locationConfigIcon;

            set => Set(ref locationConfigIcon, value);
        }

        /// <summary>
        /// Install ShipWorks icon
        /// </summary>
        public EFontAwesomeIcon InstallShipworksIcon
        {
            get => installShipworksIcon;
            set => Set(ref installShipworksIcon, value);
        }

        /// <summary>
        /// Install Database icon
        /// </summary>
        public EFontAwesomeIcon InstallDatabaseIcon
        {
            get => installDatabaseIcon;
            set => Set(ref installDatabaseIcon, value);
        }

        /// <summary>
        /// Upgrade ShipWorks icon
        /// </summary>
        public EFontAwesomeIcon UpgradeShipWorksIcon
        {
            get => upgradeShipWorksIcon;
            set => Set(ref upgradeShipWorksIcon, value);
        }

        /// <summary>
        /// Warning icon
        /// </summary>
        public EFontAwesomeIcon WarningIcon
        {
            get => warningIcon;
            set => Set(ref warningIcon, value);
        }

        /// <summary>
        /// Use ShipWorks icon
        /// </summary>
        public EFontAwesomeIcon UseShipWorksIcon
        {
            get => useShipWorksIcon;
            set => Set(ref useShipWorksIcon, value);
        }

        /// <summary>
        /// Command handler for opening the help page
        /// </summary>
        private void OpenHelpPage()
        {
            log.Info("User clicked Contact Us");
            Telemetry.Telemetry.TrackButtonClick("ContactUs");
            ProcessExtensions.StartWebProcess("https://support.shipworks.com/hc/en-us/requests/new");
        }

        /// <summary>
        /// Command handler for opening the help page
        /// </summary>
        private void OpenLogFolder()
        {
            ProcessExtensions.OpenFolder(Logger.LogFolder);
        }

        /// <summary>
        /// Shows a confirmation dialog before closing the window
        /// </summary>
        public bool Close(bool needsWindowClose)
        {
            IsClosing = needsWindowClose;

            if ((installSettings.Error == InstallError.None &&
                navigationService.CurrentPageKey == NavigationPageType.UseShipWorks) ||
                (!installSettings.NeedsRollback &&
                navigationService.CurrentPageKey == NavigationPageType.Warning))
            {
                return true;
            }

            var dlg = new CancelConfirmationDialog(navigationService);
            return dlg.ShowDialog() ?? false;
        }
    }
}
