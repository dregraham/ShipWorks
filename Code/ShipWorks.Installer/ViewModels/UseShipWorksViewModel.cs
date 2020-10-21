using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class UseShipWorksViewModel : InstallerViewModelBase
    {
        public UseShipWorksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.UseShipWorksVisibility = Visibility.Visible;
        }

        protected override bool NextCanExecute()
        {
            return false;
        }
    }
}
