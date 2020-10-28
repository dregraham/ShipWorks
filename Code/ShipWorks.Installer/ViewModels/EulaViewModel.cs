using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    [Obfuscation]
    public class EulaViewModel : InstallerViewModelBase
    {
        /// <summary>
        /// View Model for the Eula page
        /// </summary>
        public EulaViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallPath)
        {
        }

        /// <summary>
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.EulaIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if this page is complete and we can move on
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
