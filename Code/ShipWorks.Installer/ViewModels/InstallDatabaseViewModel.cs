using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
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
            log.Info("Beginning database installation");
            Task.Run(async () =>
            {
                exitCode = await shipWorksCommandLineService.AutoInstallShipWorks(mainViewModel.InstallSettings);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (exitCode == 0)
                    {
                        mainViewModel.InstallDatabaseIcon = EFontAwesomeIcon.Regular_CheckCircle;
                        mainViewModel.CurrentPage = NavigationPageType.UseShipWorks;
                        navigationService.NavigateTo(NextPage);
                        return;
                    }

                    mainViewModel.InstallDatabaseIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                    mainViewModel.UseShipWorksIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                    mainViewModel.CurrentPage = NavigationPageType.UseShipWorks;
                    mainViewModel.InstallSettings.Error = InstallError.Database;
                    mainViewModel.InstallSettings.NeedsRollback = false;
                    navigationService.NavigateTo(NavigationPageType.Warning);
                });
            });
        }
    }
}
