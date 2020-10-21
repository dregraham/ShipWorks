using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _loadedCommand;
        private INavigationService<NavigationPageType> _navigationService;
        private Visibility eulaFinishedVisibility = Visibility.Hidden;
        private Visibility installPathFinishedVisibility = Visibility.Hidden;
        private Visibility loginFinishedVisibility = Visibility.Hidden;
        private Visibility installationFinishedVisibility = Visibility.Hidden;
        private Visibility systemCheckVisibility = Visibility.Hidden;
        private Visibility locationConfigVisibility = Visibility.Hidden;
        private Visibility installShipworksVisibility = Visibility.Hidden;
        private Visibility installDatabaseVisibility = Visibility.Hidden;
        private Visibility upgradeShipWorksVisibility = Visibility.Hidden;
        private Visibility warningVisibility = Visibility.Hidden;
        private Visibility useShipWorksVisibility = Visibility.Hidden;

        public MainViewModel(INavigationService<NavigationPageType> navigationService)
        {
            _navigationService = navigationService;
        }

        public Visibility EulaFinishedVisibility
        {
            get { return eulaFinishedVisibility; }

            set
            {
                eulaFinishedVisibility = value;
                RaisePropertyChanged(nameof(EulaFinishedVisibility));
            }
        }

        public Visibility InstallPathFinishedVisibility
        {
            get { return installPathFinishedVisibility; }

            set
            {
                installPathFinishedVisibility = value;
                RaisePropertyChanged(nameof(InstallPathFinishedVisibility));
            }
        }

        public Visibility LoginFinishedVisibility
        {
            get { return loginFinishedVisibility; }

            set
            {
                loginFinishedVisibility = value;
                RaisePropertyChanged(nameof(LoginFinishedVisibility));
            }
        }

        public Visibility InstallationFinishedVisibility
        {
            get { return installationFinishedVisibility; }

            set
            {
                installationFinishedVisibility = value;
                RaisePropertyChanged(nameof(InstallationFinishedVisibility));
            }
        }

        public Visibility SystemCheckVisibility
        {
            get { return systemCheckVisibility; }

            set
            {
                systemCheckVisibility = value;
                RaisePropertyChanged(nameof(SystemCheckVisibility));
            }
        }

        public Visibility LocationConfigVisibility
        {
            get { return locationConfigVisibility; }

            set
            {
                locationConfigVisibility = value;
                RaisePropertyChanged(nameof(LocationConfigVisibility));
            }
        }

        public Visibility InstallShipworksVisibility
        {
            get { return installShipworksVisibility; }

            set
            {
                installShipworksVisibility = value;
                RaisePropertyChanged(nameof(InstallShipworksVisibility));
            }
        }

        public Visibility InstallDatabaseVisibility
        {
            get { return installDatabaseVisibility; }

            set
            {
                installDatabaseVisibility = value;
                RaisePropertyChanged(nameof(InstallDatabaseVisibility));
            }
        }

        public Visibility UpgradeShipWorksVisibility
        {
            get { return upgradeShipWorksVisibility; }

            set
            {
                upgradeShipWorksVisibility = value;
                RaisePropertyChanged(nameof(UpgradeShipWorksVisibility));
            }
        }

        public Visibility WarningVisibility
        {
            get { return warningVisibility; }

            set
            {
                warningVisibility = value;
                RaisePropertyChanged(nameof(WarningVisibility));
            }
        }

        public Visibility UseShipWorksVisibility
        {
            get { return useShipWorksVisibility; }

            set
            {
                useShipWorksVisibility = value;
                RaisePropertyChanged(nameof(UseShipWorksVisibility));
            }
        }
    }
}
