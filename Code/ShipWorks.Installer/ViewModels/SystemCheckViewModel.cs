using System.Windows;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    public class SystemCheckViewModel : InstallerViewModelBase
    {
        public SystemCheckViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.Eula)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.SystemCheckVisibility = Visibility.Visible;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
