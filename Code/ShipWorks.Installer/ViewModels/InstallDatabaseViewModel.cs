using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    [Obfuscation]
    public class InstallDatabaseViewModel : InstallerViewModelBase
    {
        public InstallDatabaseViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.InstallDatabaseIcon = EFontAwesomeIcon.Regular_CheckCircle;
            mainViewModel.InstallationIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
