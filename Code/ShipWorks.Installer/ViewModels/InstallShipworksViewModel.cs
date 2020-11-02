using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the Install ShipWorks page
    /// </summary>
    [Obfuscation]
    public class InstallShipworksViewModel : InstallerViewModelBase
    {
        private readonly IInnoSetupService innoSetupService;

        /// <summary>
        /// Constructor
        /// </summary>
        public InstallShipworksViewModel(MainViewModel mainViewModel, 
            INavigationService<NavigationPageType> navigationService, IInnoSetupService innoSetupService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallDatabase)
        {
            this.innoSetupService = innoSetupService;
            innoSetupService.InstallShipWorks(mainViewModel.InstallSettings);
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.InstallShipworksIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if the NextCommand can execute
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
