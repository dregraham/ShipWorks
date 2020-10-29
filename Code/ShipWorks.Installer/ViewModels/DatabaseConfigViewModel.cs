using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using log4net;
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
        private string selectedText;
        private string username;
        private string password;
        private bool nextEnabled;
        private EFontAwesomeIcon connectionIcon;
        private ISqlServerLookupService sqlLookup;
        private ILog log;

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
                Set(ref serverInstance, value);
                ListDatabases();
            }
        }

        /// <summary>
        /// The currently selected database
        /// </summary>
        public SqlSessionConfiguration SelectedDatabase
        {
            get => selectedDatabase;
            set
            {
                Set(ref selectedDatabase, value);
                Username = value.Username;
                Password = value.Password;
                TestConnection();
            }
        }

        /// <summary>
        /// The text to display above the database grid
        /// </summary>
        public string SelectedText
        {
            get => selectedText;
            set => Set(ref selectedText, value);
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

        /// <summary>
        /// The password displayed in the test section
        /// </summary>
        public string Password
        {
            get => password;
            set
            {
                Set(ref password, value);
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
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if this page is complete and we can move on
        /// </summary>
        protected override bool NextCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Lists out all of the databases found on the provided ServerInstance
        /// </summary>
        private async void ListDatabases()
        {
            SelectedText = "Searching for databases...";
            Databases = new List<SqlSessionConfiguration>
            {
                new SqlSessionConfiguration
                {
                    DatabaseName = "Searching for databases...."
                }
            };
            try
            {
                Databases = await sqlLookup.GetDatabases(ServerInstance ?? string.Empty);
                SelectedDatabase = Databases.FirstOrDefault();
                NextEnabled = true;
            }
            catch (Exception ex)
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                SelectedText = $"An error attempting to connect to '{ServerInstance}'";
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
                SelectedText = $"Connected to '{SelectedDatabase.DatabaseName}' using the '{account}' account";
                NextEnabled = true;
            }
            else
            {
                ConnectionIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
                SelectedText = $"Failed to connect to '{SelectedDatabase.DatabaseName}' using the '{account}' account";
                NextEnabled = false;
            }

        }
    }
}
