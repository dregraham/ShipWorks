using FontAwesome5;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _loadedCommand;
        private INavigationService<NavigationPageType> _navigationService;
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
        private SystemCheckResult checkSystemResult;

        public MainViewModel(INavigationService<NavigationPageType> navigationService)
        {
            _navigationService = navigationService;
        }

        public SystemCheckResult CheckSystemResult
        {
            get { return checkSystemResult; }

            set
            {
                checkSystemResult = value;
                RaisePropertyChanged(nameof(CheckSystemResult));
            }
        }

        public EFontAwesomeIcon EulaIcon
        {
            get => eulaIcon;
            set => Set(ref eulaIcon, value);
        }

        public EFontAwesomeIcon InstallPathIcon
        {
            get => installPathIcon;
            set => Set(ref installPathIcon, value);
        }

        public EFontAwesomeIcon LoginIcon
        {
            get => loginIcon;
            set => Set(ref loginIcon, value);
        }

        public EFontAwesomeIcon InstallationIcon
        {
            get => installationIcon;

            set => Set(ref installationIcon, value);
        }

        public EFontAwesomeIcon SystemCheckIcon
        {
            get => systemCheckIcon;
            set => Set(ref systemCheckIcon, value);
        }

        public EFontAwesomeIcon LocationConfigIcon
        {
            get => locationConfigIcon;

            set => Set(ref locationConfigIcon, value);
        }

        public EFontAwesomeIcon InstallShipworksIcon
        {
            get => installShipworksIcon;
            set => Set(ref installShipworksIcon, value);
        }

        public EFontAwesomeIcon InstallDatabaseIcon
        {
            get => installDatabaseIcon;
            set => Set(ref installDatabaseIcon, value);
        }

        public EFontAwesomeIcon UpgradeShipWorksIcon
        {
            get => upgradeShipWorksIcon;
            set => Set(ref upgradeShipWorksIcon, value);
        }

        public EFontAwesomeIcon WarningIcon
        {
            get => warningIcon;
            set => Set(ref warningIcon, value);
        }

        public EFontAwesomeIcon UseShipWorksIcon
        {
            get => useShipWorksIcon;
            set => Set(ref useShipWorksIcon, value);
        }
    }
}
