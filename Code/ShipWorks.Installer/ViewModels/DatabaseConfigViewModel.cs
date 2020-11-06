using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Sql;
using ShipWorks.Installer.Utilities;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the database config page
    /// </summary>
    [Obfuscation]
    public class DatabaseConfigViewModel : InstallerViewModelBase
    {
        private IEnumerable<SqlSessionConfiguration> databases;
        private string serverInstance;
        private SqlSessionConfiguration selectedDatabase;
        private string connectionStatusText;
        private string username;
        private string password;
        private bool nextEnabled;
        private int selectedDatabaseIndex;
        private bool showCredentialInput;
        private EFontAwesomeIcon connectionIcon;
        private readonly ISqlServerLookupService sqlLookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseConfigViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            ISqlServerLookupService sqlLookup,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks, logFactory(typeof(DatabaseConfigViewModel)))
        {
            this.sqlLookup = sqlLookup;
            HelpCommand = new RelayCommand(() => ProcessExtensions.StartWebProcess("https://support.shipworks.com/hc/en-us/articles/360022462812"));
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            ConnectCommand = new AsyncCommand(async () =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                Mouse.OverrideCursor = Cursors.Wait;
                _ = Task.Run(() => ListDatabases());
            });
            username = string.Empty;
            password = string.Empty;
            showCredentialInput = false;
            NextEnabled = false;
        }

        /// <summary>
        /// Command to open a web browser to the db help article
        /// </summary>
        public ICommand HelpCommand { get; }

        /// <summary>
        /// Command for connecting to a Sql server instance
        /// </summary>
        public ICommand ConnectCommand { get; }

        /// <summary>
        /// The list of databases to display
        /// </summary>
        public IEnumerable<SqlSessionConfiguration> Databases
        {
            get => databases;
            set => Set(ref databases, value);
        }

        /// <summary>
        /// The database server to connect to
        /// </summary>
        public string ServerInstance
        {
            get => serverInstance;
            set => Set(ref serverInstance, value.ToUpperInvariant());
        }

        /// <summary>
        /// The index of the currently selected database
        /// </summary>
        public int SelectedDatabaseIndex
        {
            get => selectedDatabaseIndex;
            set
            {
                Set(ref selectedDatabaseIndex, value);
                if (value > -1)
                {
                    SelectedDatabase = Databases.ToArray()[value];
                }
            }
        }

        /// <summary>
        /// The currently selected database
        /// </summary>
        private SqlSessionConfiguration SelectedDatabase
        {
            get => selectedDatabase;
            set
            {
                selectedDatabase = value;
                Username = value.Username;
                Password = value.Password;
                _ = TestConnection();
            }
        }

        /// <summary>
        /// The text to display above the database grid
        /// </summary>
        public string ConnectionStatusText
        {
            get => connectionStatusText;
            set => Set(ref connectionStatusText, value);
        }

        /// <summary>
        /// The icon for if we can connect or not
        /// </summary>
        public EFontAwesomeIcon ConnectionIcon
        {
            get => connectionIcon;
            set => Set(ref connectionIcon, value);
        }

        /// <summary>
        /// The username displayed in the test section
        /// </summary>
        public string Username
        {
            get => username;
            set
            {
                Set(ref username, value);
                if (SelectedDatabase != null)
                {
                    SelectedDatabase.Username = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        SelectedDatabase.WindowsAuth = false;
                    }
                    else
                    {
                        SelectedDatabase.WindowsAuth = true;
                    }
                }
            }
        }

        /// <summary>
        /// The password displayed in the test section
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                Set(ref password, value);
                if (SelectedDatabase != null)
                {
                    SelectedDatabase.Password = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        SelectedDatabase.WindowsAuth = false;
                    }
                    else
                    {
                        SelectedDatabase.WindowsAuth = true;
                    }
                }
            }
        }

        /// <summary>
        /// Whether or not the user can continue
        /// </summary>
        public bool NextEnabled
        {
            get => nextEnabled;
            set => Set(ref nextEnabled, value);
        }

        /// <summary>
        /// Whether or not we need to display inputs for credentials
        /// </summary>
        public bool ShowCredentialInput
        {
            get => showCredentialInput;
            set => Set(ref showCredentialInput, value);
        }

        /// <summary>
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.InstallSettings.ConnectionString = SelectedDatabase.GetConnectionString();
            log.Info(mainViewModel.InstallSettings.ConnectionString);
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
            mainViewModel.CurrentPage = NextPage;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if this page is complete and we can move on.
        /// In this instance returning the actual value is fighting the buttons
        /// enabled property. So just return true here and we'll handle this by 
        /// enabling the button our selves
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Command handler for the back command
        /// </summary>
        protected override void BackExecute()
        {
            base.BackExecute();
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.None;
        }

        /// <summary>
        /// Lists out all of the databases found on the provided ServerInstance
        /// </summary>
        private async Task ListDatabases()
        {
            ConnectionStatusText = "Searching for databases...";
            ConnectionIcon = EFontAwesomeIcon.None;
            Databases = null;
            try
            {
                Databases = await sqlLookup.GetDatabases(ServerInstance ?? string.Empty, Username, Password).ConfigureAwait(true);
                NextEnabled = false;
                ShowCredentialInput = false;
                ConnectionStatusText = null;
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                    ConnectionStatusText = $"An error occurred attempting to connect to '{ServerInstance}'";
                    NextEnabled = false;
                    ShowCredentialInput = true;
                });
                log.Error(ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
            }
        }

        /// <summary>
        /// Tests a connection to a db
        /// </summary>
        private async Task TestConnection()
        {
            string account = SelectedDatabase.WindowsAuth ? "Windows Auth" : $"the '{SelectedDatabase.Username}' account";
            if (await sqlLookup.TestConnection(SelectedDatabase))
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_CheckCircle;
                ConnectionStatusText = $"Connected to '{SelectedDatabase.DatabaseName}' using {account}";
                NextEnabled = true;
            }
            else
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                ConnectionStatusText = $"Failed to connect to '{SelectedDatabase.DatabaseName}' using {account}";
                NextEnabled = false;
                ShowCredentialInput = true;
            }
        }
    }
}
