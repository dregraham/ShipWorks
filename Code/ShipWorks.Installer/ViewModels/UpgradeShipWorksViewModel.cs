using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the Upgrade ShipWorks page
    /// </summary>
    [Obfuscation]
    public class UpgradeShipWorksViewModel : InstallerViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpgradeShipWorksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService, 
               IInnoSetupService innoSetupService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
            innoSetupService.InstallShipWorks(mainViewModel.InstallSettings);
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.UpgradeShipWorksIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if the NextCommand can execute
        /// </summary>
        protected override bool NextCanExecute()
        {
            return false;
        }
    }
}
