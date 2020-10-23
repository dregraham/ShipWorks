using System.Windows;
using FontAwesome5;
using GalaSoft.MvvmLight;
using ShipWorks.Installer.Models;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// Main view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService<NavigationPageType> navigationService;
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
        private bool isFreshInstall;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel(INavigationService<NavigationPageType> navigationService)
        {
            this.navigationService = navigationService;
            InstallSettings = new InstallSettings();
            IsFreshInstall = false;
        }

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
                       (InstallSettings.CheckSystemResult.RamMeetsRequirement &&
                        InstallSettings.CheckSystemResult.CpuMeetsRequirement &&
                        InstallSettings.CheckSystemResult.HddMeetsRequirement &&
                        InstallSettings.CheckSystemResult.OsMeetsRequirement)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        /// <summary>
        /// Whether this is a new installation or an upgrade
        /// </summary>
        public bool IsFreshInstall
        {
            get => isFreshInstall;
            set => Set(ref isFreshInstall, value);
        }

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
    }
}
