using FontAwesome5;
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
            mainViewModel.UpgradeShipWorksIcon = EFontAwesomeIcon.Regular_CheckCircle;
        }

        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
