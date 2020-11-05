using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using FontAwesome5;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the Use ShipWorks page
    /// </summary>
    [Obfuscation]
    public class UseShipWorksViewModel : InstallerViewModelBase
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public UseShipWorksViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks, logFactory(typeof(UseShipWorksViewModel)))
        {
            mainViewModel.InstallSettings.NeedsRollback = false;
            log = logFactory(typeof(UseShipWorksViewModel));
            log.Info("UseShipWorksViewModel starting.");
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.UseShipWorksIcon = EFontAwesomeIcon.Regular_CheckCircle;

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = Path.Combine(mainViewModel.InstallSettings.InstallPath, "ShipWorks.exe"),
            };

            log.Info($"UseShipWorksViewModel.NextExecute Starting {start.FileName}.");
            Process.Start(start);
        }

        /// <summary>
        /// Determines if the NextCommand can execute
        /// </summary>
        protected override bool NextCanExecute()
        {
            return mainViewModel.InstallSettings.Error == InstallError.None;
        }
    }
}
