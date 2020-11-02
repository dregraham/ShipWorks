using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Sql;

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
        private EFontAwesomeIcon connectionIcon;
        private readonly ISqlServerLookupService sqlLookup;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseConfigViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            ISqlServerLookupService sqlLookup,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks)
        {
            this.sqlLookup = sqlLookup;
            this.log = logFactory(typeof(DatabaseConfigViewModel));
            TestCommand = new RelayCommand(TestConnection);
            HelpCommand = new RelayCommand(() => ProcessExtension.StartWebProcess("https://support.shipworks.com/hc/en-us/articles/360022462812"));
            ConnectCommand = new RelayCommand(ListDatabases);
            username = string.Empty;
            password = string.Empty;
            NextEnabled = false;
        }

        /// <summary>
        /// Command to test a db connection
        /// </summary>
        public ICommand TestCommand { get; }

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
        /// The list of network locations to display
        /// </summary>
        public string ServerInstance
        {
            get => serverInstance;
            set
            {
                Set(ref serverInstance, value.ToUpperInvariant());
                ListDatabases();
            }
        }

        /// <summary>
        /// The index of the currently selected databse
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
                TestConnection();
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
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.InstallSettings.ConnectionString = SelectedDatabase.GetConnectionString();
            log.Info(mainViewModel.InstallSettings.ConnectionString);
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if this page is complete and we can move on
        /// </summary>
        protected override bool NextCanExecute()
        {
            return NextEnabled;
        }

        /// <summary>
        /// Lists out all of the databases found on the provided ServerInstance
        /// </summary>
        private async void ListDatabases()
        {
            ConnectionStatusText = "Searching for databases...";
            Databases = new List<SqlSessionConfiguration>
            {
                new SqlSessionConfiguration
                {
                    DatabaseName = "Searching for databases...."
                }
            };
            try
            {
                Databases = await sqlLookup.GetDatabases(ServerInstance ?? string.Empty, Username, Password);
                SelectedDatabaseIndex = 0;
                NextEnabled = true;
            }
            catch (Exception ex)
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                ConnectionStatusText = $"An error occured attempting to connect to '{ServerInstance}'";
                NextEnabled = false;
                log.Error(ex.Message);
            }
        }

        /// <summary>
        /// Tests a connection to a db
        /// </summary>
        private async void TestConnection()
        {
            string account = SelectedDatabase.WindowsAuth ? "Windows Auth" : SelectedDatabase.Username;
            if (await sqlLookup.TestConnection(SelectedDatabase))
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_CheckCircle;
                ConnectionStatusText = $"Connected to '{SelectedDatabase.DatabaseName}' using the '{account}' account";
                NextEnabled = true;
            }
            else
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                ConnectionStatusText = $"Failed to connect to '{SelectedDatabase.DatabaseName}' using the '{account}' account";
                NextEnabled = false;
            }
        }
    }
}
