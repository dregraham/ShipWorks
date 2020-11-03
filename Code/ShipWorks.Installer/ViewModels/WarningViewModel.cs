using System;
using System.Reflection;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the warning view
    /// </summary>
    [Obfuscation]
    public class WarningViewModel : InstallerViewModelBase
    {
        private string warning;
        /// <summary>
        /// Constructor
        /// </summary>
        public WarningViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks, logFactory(typeof(WarningViewModel)))
        {
            Warning = "There was a problem installing ShipWorks.";
        }

        /// <summary>
        /// The warning message to show
        /// </summary>
        public string Warning
        {
            get => warning;
            set => Set(ref warning, value);
        }
    }
}
