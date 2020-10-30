using System;
using System.Reflection;
using FontAwesome5;
using log4net;
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
        private string error;
        private readonly IHubService hubService;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            IHubService hubService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.LocationConfig)
        {
            this.hubService = hubService;
            log = logFactory(typeof(LoginViewModel));
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
        /// The error message
        /// </summary>
        public string Error
        {
            get => error;
            set => Set(ref error, value);
        }

        /// <summary>
        /// Login to the hub
        /// </summary>
        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Error = "Please enter a username and password";
                return;
            }

            try
            {
                Error = null;
                await hubService.Login(mainViewModel.InstallSettings, Username, Password);

                // If the customer's trial has ended, send them to the warnings page
                if (mainViewModel.InstallSettings.Token.RecurlyTrialEndDate < DateTime.UtcNow)
                {
                    log.Error($"The customer's trial has expired. End date was {mainViewModel.InstallSettings.Token.RecurlyTrialEndDate} UTC.");
                    mainViewModel.InstallSettings.Error = InstallError.Unknown;
                    mainViewModel.LoginIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                    navigationService.NavigateTo(NavigationPageType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return;
            }
            mainViewModel.LoginIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Command handler for the NextCommand
        /// Call Login as a separate async function to prevent blocking the UI
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
