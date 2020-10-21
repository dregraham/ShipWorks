using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class InstallDatabaseViewModel : InstallerViewModelBase
    {
        public InstallDatabaseViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.InstallDatabaseVisibility = Visibility.Visible;
            mainViewModel.InstallationFinishedVisibility = Visibility.Visible;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
