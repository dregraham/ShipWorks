using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    [Obfuscation]
    public class InstallShipworksViewModel : InstallerViewModelBase
    {
        public InstallShipworksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallDatabase)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.InstallShipworksIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
