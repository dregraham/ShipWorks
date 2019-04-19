using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

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
        private readonly IWarehouseRemoteLoginWithToken remoteLoginWithToken;
        private readonly IWarehouseList warehouseListRequest;
        private string warehouseName;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsViewModel(
            IWarehouseSettings warehouseSettingsView, 
            IWarehouseListViewModel warehouseList, 
            IWarehouseRemoteLoginWithToken remoteLoginWithToken,
            IWarehouseList warehouseListRequest)
        {
            this.warehouseSettingsView = warehouseSettingsView;
            this.warehouseList = warehouseList;
            this.remoteLoginWithToken = remoteLoginWithToken;
            this.warehouseListRequest = warehouseListRequest;
            SelectWarehouse = new RelayCommand(OnSelectWarehouse);

            warehouseSettingsView.ViewModel = this;
            warehouseName = "No warehouse selected";
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
            var tokenResponse = remoteLoginWithToken.RemoteLoginWithToken();
            if (tokenResponse.Success)
            {
                var results = warehouseListRequest.GetList(tokenResponse.Value);
                var warehouses = results.Value.warehouses.Select(x => new WarehouseViewModel(x));

                IWarehouseViewModel warehouse = warehouseList.ChooseWarehouse(warehouses);
                if (warehouse != null)
                {
                    WarehouseName = warehouse.Name;
                }
            }
        }
    }
}
