using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model for warehouse settings
    /// </summary>
    [Component]
    public class WarehouseSettingsViewModel : ViewModelBase, IWarehouseSettingsViewModel
    {
        private readonly IWarehouseSettings warehouseSettingsView;
        private readonly IWarehouseListViewModel warehouseList;
        private readonly IWarehouseList warehouseListRequest;
        private readonly IWarehouseAssociation warehouseAssociation;
        private readonly IConfigurationData configurationData;
        private string warehouseName;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsViewModel(
            IWarehouseSettings warehouseSettingsView,
            IWarehouseListViewModel warehouseList,
            IWarehouseList warehouseListRequest,
            IWarehouseAssociation warehouseAssociation,
            IConfigurationData configurationData)
        {
            this.warehouseSettingsView = warehouseSettingsView;
            this.warehouseList = warehouseList;
            this.warehouseListRequest = warehouseListRequest;
            this.warehouseAssociation = warehouseAssociation;
            this.configurationData = configurationData;
            SelectWarehouse = new RelayCommand(OnSelectWarehouse);

            warehouseSettingsView.ViewModel = this;

            var config = configurationData.FetchReadOnly();
            warehouseName = string.IsNullOrEmpty(config.WarehouseID) ? "No warehouse selected" : config.WarehouseName;
        }

        /// <summary>
        /// Select warehouse command
        /// </summary>
        [Obfuscation]
        public ICommand SelectWarehouse { get; }

        /// <summary>
        /// Name of the warehouse associated with this instance of ShipWorks
        /// </summary>
        [Obfuscation]
        public string WarehouseName
        {
            get => warehouseName;
            set => Set(ref warehouseName, value);
        }

        /// <summary>
        /// Get the control associated with the view model
        /// </summary>
        public Control Control => warehouseSettingsView.Control;

        /// <summary>
        /// Save configuration data
        /// </summary>
        public void Save() { }

        /// <summary>
        /// Handle the select warehouse commandorder
        /// </summary>
        private void OnSelectWarehouse()
        {
            var results = warehouseListRequest.GetList();
            var warehouses = results.Value.warehouses.Select(x => new WarehouseViewModel(x));

            WarehouseViewModel warehouse = warehouseList.ChooseWarehouse(warehouses);
            if (warehouse != null)
            {
                var associationResponse = warehouseAssociation.Associate(warehouse.Id);
                if (associationResponse.Success)
                {
                    configurationData.UpdateConfiguration(x =>
                    {
                        x.WarehouseID = warehouse.Id;
                        x.WarehouseName = warehouse.Name;
                    });

                    WarehouseName = warehouse.Name;
                }
            }
        }
    }
}
