using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    [Obfuscation]
    public class WarningViewModel : InstallerViewModelBase
    {
        public WarningViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
        }

        protected override void NextExecute()
        {
            mainViewModel.WarningIcon = EFontAwesomeIcon.Regular_CheckCircle;
        }

        protected override bool NextCanExecute()
        {
            return false;
        }
    }
}
