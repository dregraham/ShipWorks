using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using FontAwesome5;
using log4net;
using ShipWorks.Installer.Api.DTO;
using ShipWorks.Installer.Enums;
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
        private Warehouse selectedWarehouse = new Warehouse { Details = new Details { Name = "Loading Warehouses..." } };
        private ObservableCollection<Warehouse> warehouseList = new ObservableCollection<Warehouse>();
        private readonly IHubService hubService;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationConfigViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            IHubService hubService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks, logFactory(typeof(LocationConfigViewModel)))
        {
            WarehouseList.Add(SelectedWarehouse);
            this.hubService = hubService;
            _ = GetWarehouseList();
        }

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
            set => Set(ref selectedWarehouse, value);
        }

        /// <summary>
        /// Get the list of warehouses from the Hub
        /// </summary>
        public async Task GetWarehouseList()
        {
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
                });
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
    }
}
