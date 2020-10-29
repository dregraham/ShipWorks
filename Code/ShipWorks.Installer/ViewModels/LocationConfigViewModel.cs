using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the warehouse selection page
    /// </summary>
    [Obfuscation]
    public class LocationConfigViewModel : InstallerViewModelBase
    {
        private bool ownDbChecked;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationConfigViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks)
        {
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        public bool OwnDbChecked
        {
            get => ownDbChecked;
            set => Set(ref ownDbChecked, value);
        }

        /// <summary>
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            if (OwnDbChecked)
            {
                NextPage = NavigationPageType.DatabaseConfig;
            }
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
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
