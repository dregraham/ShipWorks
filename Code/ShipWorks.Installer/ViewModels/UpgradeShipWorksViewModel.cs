using System;
using System.Reflection;
using System.Threading.Tasks;
using FontAwesome5;
using log4net;
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
        private readonly IInnoSetupService innoSetupService;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpgradeShipWorksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService, 
               IInnoSetupService innoSetupService, Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks, logFactory(typeof(UpgradeShipWorksViewModel)))
        {
            this.innoSetupService = innoSetupService;
            log.Info("Upgrade ShipWorks screen displayed.");
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override async Task NextExecuteAsync()
        {
            mainViewModel.UpgradeShipWorksIcon = EFontAwesomeIcon.Regular_CheckCircle;

            log.Info("Starting Inno setup for upgrade.");
            await innoSetupService.InstallShipWorks(mainViewModel.InstallSettings).ConfigureAwait(true);
            
            log.Info("Finished Inno setup for upgrade.");

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
