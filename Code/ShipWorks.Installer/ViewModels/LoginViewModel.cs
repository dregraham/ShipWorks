using System;
using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for the login page
    /// </summary>
    [Obfuscation]
    public class LoginViewModel : InstallerViewModelBase
    {
        private string username;
        private string password;
        private readonly IHubService hubService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            IHubService hubService) :
            base(mainViewModel, navigationService, NavigationPageType.LocationConfig)
        {
            this.hubService = hubService;
        }

        /// <summary>
        /// The Hub username
        /// </summary>
        public string Username
        {
            get => username;
            set => Set(ref username, value);
        }

        /// <summary>
        /// The Hub password
        /// </summary>
        public string Password
        {
            get => password;
            set => Set(ref password, value);
        }

        /// <summary>
        /// Login to the hub
        /// </summary>
        private async void Login()
        {
            try
            {
                await hubService.Login(mainViewModel.InstallSettings, Username, Password);
            }
            catch (Exception)
            {
                // TODO: Set error message
                return;
            }
            mainViewModel.LoginIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected override void NextExecute() => Login();

        /// <summary>
        /// Determines if the NextCommand can execute
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
