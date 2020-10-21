using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class InstallShipworksViewModel : InstallerViewModelBase
    {
        public InstallShipworksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallDatabase)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.InstallShipworksVisibility = Visibility.Visible;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
