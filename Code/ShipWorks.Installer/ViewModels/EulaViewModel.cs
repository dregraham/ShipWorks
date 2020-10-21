using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class EulaViewModel : InstallerViewModelBase
    {
        public EulaViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) : 
            base(mainViewModel, navigationService, NavigationPageType.InstallPath)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.EulaFinishedVisibility = Visibility.Visible;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
