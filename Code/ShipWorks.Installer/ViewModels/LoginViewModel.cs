using System;
using System.Reflection;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
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
        private bool loggingIn = false;
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
            NoAccountCommand = new RelayCommand(() => ProcessExtension.StartWebProcess("https://registration.shipworks.com/WebRegistration/#!&p=create-account"));
            ForgotPasswordCommand = new RelayCommand(() => ProcessExtension.StartWebProcess("https://www.interapptive.com/account/forgotpassword.php"));
        }

        /// <summary>
        /// Opens a web browser when the no account link is clicked
        /// </summary>
        public ICommand NoAccountCommand { get; }

        /// <summary>
        /// Opens a web browser when the forgot password link is clicked
        /// </summary>
        public ICommand ForgotPasswordCommand { get; }

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
                Mouse.OverrideCursor = Cursors.Wait;
                loggingIn = true;

                // Needs ConfigureAwait(true) in order to set the mouse cursor in the finally block
                await hubService.Login(mainViewModel.InstallSettings, Username, Password).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return;
            }
            finally
            {
                loggingIn = false;
                Mouse.OverrideCursor = Cursors.Arrow;
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
            return !loggingIn;
        }
    }
}
