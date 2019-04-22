using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private readonly IConfigurationData configurationData;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;
        private readonly IMessageHelper messageHelper;
        private string warehouseName;
        private bool associatedWithWarehouse;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsViewModel(
            IWarehouseSettings warehouseSettingsView,
            IWarehouseListViewModel warehouseList,
            IWarehouseSettingsApi warehouseSettingsApi,
            IConfigurationData configurationData,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.warehouseSettingsApi = warehouseSettingsApi;
            this.warehouseSettingsView = warehouseSettingsView;
            this.warehouseList = warehouseList;
            this.configurationData = configurationData;
            SelectWarehouse = new RelayCommand(OnSelectWarehouse);

            warehouseSettingsView.ViewModel = this;

            var config = configurationData.FetchReadOnly();
            if (string.IsNullOrEmpty(config.WarehouseID))
            {
                WarehouseName = "No warehouse selected";
            }
            else
            {
                WarehouseName = config.WarehouseName;
                AssociatedWithWarehouse = true;
            }
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
        /// Is this ShipWorks database already associated with a warehouse
        /// </summary>
        [Obfuscation]
        public bool AssociatedWithWarehouse
        {
            get => associatedWithWarehouse;
            set => Set(ref associatedWithWarehouse, value);
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
        /// Handle the select warehouse command
        /// </summary>
        private async void OnSelectWarehouse()
        {
            var results = await warehouseSettingsApi.GetAllWarehouses().ConfigureAwait(true);
            var warehouses = results.Value.warehouses.Select(x => new WarehouseViewModel(x));

            WarehouseViewModel warehouse = warehouseList.ChooseWarehouse(warehouses);
            if (warehouse != null)
            {
                var associationResponse = await warehouseSettingsApi.Associate(warehouse.Id).ConfigureAwait(true);
                if (associationResponse.Success)
                {
                    configurationData.UpdateConfiguration(x =>
                    {
                        x.WarehouseID = warehouse.Id;
                        x.WarehouseName = warehouse.Name;
                    });

                    WarehouseName = warehouse.Name;
                    AssociatedWithWarehouse = true;
                }
                else
                {
                    messageHelper.ShowError("Could not associate this ShipWorks database with the warehouse.");
                }
            }
        }
    }
}
