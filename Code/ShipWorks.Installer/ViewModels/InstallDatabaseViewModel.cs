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
    /// View Model for the Install Database page
    /// </summary>
    [Obfuscation]
    public class InstallDatabaseViewModel : InstallerViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InstallDatabaseViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService,
            IShipWorksCommandLineService shipWorksCommandLineService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks, logFactory(typeof(InstallDatabaseViewModel)))
        {

            var exitCode = 0;

            Task.Run(async () =>
            {
                exitCode = await shipWorksCommandLineService.AutoInstallShipWorks(mainViewModel.InstallSettings);

                if (exitCode == 0)
                {
                    navigationService.NavigateTo(NextPage);
                    return;
                }

                mainViewModel.InstallDatabaseIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                navigationService.NavigateTo(NavigationPageType.Warning);
            });
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.InstallDatabaseIcon = EFontAwesomeIcon.Regular_CheckCircle;
            mainViewModel.InstallationIcon = EFontAwesomeIcon.Regular_CheckCircle;
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
