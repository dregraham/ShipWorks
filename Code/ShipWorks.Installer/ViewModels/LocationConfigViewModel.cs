using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FontAwesome5;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationConfigViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            IHubService hubService) :
            base(mainViewModel, navigationService, NavigationPageType.InstallShipworks)
        {
            WarehouseList.Add(SelectedWarehouse);
            this.hubService = hubService;
            GetWarehouseList();
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
        public async void GetWarehouseList()
        {
            var response = await hubService.GetWarehouseList(mainViewModel.InstallSettings.Token);
            WarehouseList = new ObservableCollection<Warehouse>(response.warehouses);
            SelectedWarehouse = WarehouseList.FirstOrDefault() ?? SelectedWarehouse;
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
            mainViewModel.LocationConfigIcon = EFontAwesomeIcon.Regular_CheckCircle;
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
