using System;
using System.Reflection;
using FontAwesome5;
using log4net;
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
        public EulaViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallPath, logFactory(typeof(EulaViewModel)))
        {
        }

        /// <summary>
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.EulaIcon = EFontAwesomeIcon.Regular_CheckCircle;
            mainViewModel.CurrentPage = NextPage;
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
