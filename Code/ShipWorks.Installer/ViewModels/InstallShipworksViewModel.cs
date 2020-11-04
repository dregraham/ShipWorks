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
    /// View model for the Install ShipWorks page
    /// </summary>
    [Obfuscation]
    public class InstallShipWorksViewModel : InstallerViewModelBase
    {
        private readonly IInnoSetupService innoSetupService;

        /// <summary>
        /// Constructor
        /// </summary>
        public InstallShipWorksViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService, IInnoSetupService innoSetupService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallDatabase, logFactory(typeof(InstallShipWorksViewModel)))
        {
            this.innoSetupService = innoSetupService;

            log.Info("Upgrade ShipWorks screen displayed.");
            _ = NextExecuteAsync();
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override async Task NextExecuteAsync()
        {
            log.Info("Starting Inno setup for install.");
            await innoSetupService.InstallShipWorks(mainViewModel.InstallSettings).ConfigureAwait(true);
            log.Info("Finished Inno setup for install.");

            if (mainViewModel.InstallSettings.Error != InstallError.None)
            {
                mainViewModel.InstallationIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                NextPage = NavigationPageType.Warning;
            }
            else
            {
                mainViewModel.InstallShipworksIcon = EFontAwesomeIcon.Regular_CheckCircle;
            }

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
