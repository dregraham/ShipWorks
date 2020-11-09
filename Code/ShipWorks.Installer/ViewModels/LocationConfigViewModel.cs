using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome5;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Api.DTO;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
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
        private bool finishedLoading = false;
        private bool isCheckBoxEnabled;
        private Warehouse selectedWarehouse;
        private ObservableCollection<Warehouse> warehouseList = new ObservableCollection<Warehouse>();
        private readonly IHubService hubService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationConfigViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            IHubService hubService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks, logFactory(typeof(LocationConfigViewModel)))
        {
            OpenWebsiteCommand = new RelayCommand(() => ProcessExtensions.StartWebProcess("https://shipworks.zendesk.com/hc/en-us/articles/360022647251"));
            this.hubService = hubService;
            navigationService.Navigated += OnNavigated;
            _ = GetWarehouseList();
        }

        /// <summary>
        /// Command to open a website
        /// </summary>
        public ICommand OpenWebsiteCommand { get; }

        /// <summary>
        /// The list of warehouses
        /// </summary>
        public ObservableCollection<Warehouse> WarehouseList
        {
            get => warehouseList;
            set => Set(ref warehouseList, value);
        }

        /// <summary>
        /// The currently selected warehouse;
        /// </summary>
        public Warehouse SelectedWarehouse
        {
            get => selectedWarehouse;
            set
            {
                Set(ref selectedWarehouse, value);

                if (value?.ID != null)
                {
                    OwnDbChecked = false;
                }
            }
        }

        /// <summary>
        /// Flag to check if we are still loading the warehouse list
        /// </summary>
        public bool FinishedLoading
        {
            get => finishedLoading;
            set => Set(ref finishedLoading, value);
        }

        /// <summary>
        /// Get the list of warehouses from the Hub
        /// </summary>
        public async Task GetWarehouseList()
        {
            FinishedLoading = false;
            SelectedWarehouse = new Warehouse { Details = new Details { Name = "Loading Warehouses..." } };
            WarehouseList = new ObservableCollection<Warehouse> { SelectedWarehouse };

            var warehouses = new List<Warehouse>(){
                new Warehouse
                {
                    Details = new Details
                    {
                        Name = "None"
                    }
                }
            };

            try
            {
                var response = await hubService.GetWarehouseList(mainViewModel.InstallSettings.Token).ConfigureAwait(false);
                warehouses.AddRange(response.warehouses);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                // Update synchronously on the UI thread to prevent the WarehouseList assignment
                // from resetting SelectedWarehouse to null after we set it
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WarehouseList = new ObservableCollection<Warehouse>(warehouses);
                    SelectedWarehouse = WarehouseList.FirstOrDefault();
                    FinishedLoading = true;
                });
            }
        }

        /// <summary>
        /// Event handler when navigating to this page
        /// </summary>
        private void OnNavigated(object sender, NavigatedEventArgs e)
        {
            if (e.NavigatedPage == NavigationPageType.LocationConfig.ToString())
            {
                _ = GetWarehouseList();
            }
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
            else
            {
                mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
                mainViewModel.CurrentPage = NextPage;
            }
            mainViewModel.InstallSettings.Warehouse = SelectedWarehouse;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Determines if the NextCommand can execute
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
            mainViewModel.CurrentPage = NavigationPageType.Login;
        }
    }
}
