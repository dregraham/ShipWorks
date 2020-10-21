using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class UpgradeShipWorksViewModel : InstallerViewModelBase
    {
        public UpgradeShipWorksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.UpgradeShipWorksVisibility = Visibility.Visible;
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
