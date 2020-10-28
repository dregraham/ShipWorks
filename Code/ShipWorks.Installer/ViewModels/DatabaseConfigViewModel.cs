using System.Collections.Generic;
using System.Reflection;
using FontAwesome5;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Models;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the database config page
    /// </summary>
    [Obfuscation]
    public class DatabaseConfigViewModel : InstallerViewModelBase
    {
        private List<DatabaseInfo> databases;
        private List<string> networkLocations;
        private string selectedNetworkLocation;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseConfigViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks)
        {
            Databases = new List<DatabaseInfo>
            {
                new DatabaseInfo
                {
                    Name = "Test DB",
                    Status = "Out of Date",
                    LastActivity = "Test Guy, on 06/12/20",
                    LatestOrder = "8 from 06/12/20"
                },
                new DatabaseInfo
                {
                    Name = "Test DB Two",
                    Status = "Ready",
                    LastActivity = "Test Guy, on 06/12/20",
                    LatestOrder = "8 from 06/12/20"
                },
                new DatabaseInfo
                {
                    Name = "Test DB Three",
                    Status = "(Active)",
                    LastActivity = "Test Guy, on 06/12/20",
                    LatestOrder = "8 from 06/12/20"
                }
            };

            NetworkLocations = new List<string>
            {
                "CHART-PC\\SHIPWORKS",
                "CHART-PC\\SHIPWORKS1",
                "CHART-PC\\SHIPWORKS2",
                "CHART-PC\\SHIPWORKS3",

            };

            SelectedNetworkLocation = "CHART-PC\\SHIPWORKS";
        }

        /// <summary>
        /// The list of databases to display
        /// </summary>
        public List<DatabaseInfo> Databases
        {
            get => databases;
            set => Set(ref databases, value);
        }

        /// <summary>
        /// The list of network locations to display
        /// </summary>
        public List<string> NetworkLocations
        {
            get => networkLocations;
            set => Set(ref networkLocations, value);
        }

        /// <summary>
        /// The selected network location
        /// </summary>
        public string SelectedNetworkLocation
        {
            get => selectedNetworkLocation;
            set => Set(ref selectedNetworkLocation, value);
        }

        /// <summary>
        /// Trigger navigation to the next wizard page
        /// </summary>
        protected override void NextExecute()
        {
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if this page is complete and we can move on
        /// </summary>
        /// <returns></returns>
        protected override bool NextCanExecute()
        {
            return true;
        }
    }
}
