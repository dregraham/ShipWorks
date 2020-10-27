using System.Reflection;
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
        /// Constructo
        /// </summary>
        /// <param name="mainViewModel"></param>
        /// <param name="navigationService"></param>
        public WarningViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
            Warning = "There was a problem installing ShipWorks.";
        }

        public string Warning
        {
            get => warning;
            set => Set(ref warning, value);
        }
    }
}
