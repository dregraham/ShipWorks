using System.Reflection;
using FontAwesome5;
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
        public InstallDatabaseViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks)
        {
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
            return true;
        }
    }
}
