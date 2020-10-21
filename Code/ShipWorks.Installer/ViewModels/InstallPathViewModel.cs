using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class InstallPathViewModel : InstallerViewModelBase
    {
        public InstallPathViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.Login)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.InstallPathFinishedVisibility = Visibility.Visible;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
