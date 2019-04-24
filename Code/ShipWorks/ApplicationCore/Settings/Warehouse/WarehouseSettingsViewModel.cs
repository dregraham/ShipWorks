using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Users;

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
        private bool canLinkWarehouse;
        private readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsViewModel(
            IWarehouseSettings warehouseSettingsView,
            IWarehouseListViewModel warehouseList,
            IWarehouseSettingsApi warehouseSettingsApi,
            IConfigurationData configurationData,
            IUserSession userSession,
            IMessageHelper messageHelper)
        {
            this.userSession = userSession;
            this.messageHelper = messageHelper;
            this.warehouseSettingsApi = warehouseSettingsApi;
            this.warehouseSettingsView = warehouseSettingsView;
            this.warehouseList = warehouseList;
            this.configurationData = configurationData;
            SelectWarehouse = new RelayCommand(() => OnSelectWarehouse().Forget());

            warehouseSettingsView.ViewModel = this;

            var config = configurationData.FetchReadOnly();
            if (string.IsNullOrEmpty(config.WarehouseID))
            {
                WarehouseName = "No warehouse selected";
                CanLinkWarehouse = userSession.User.IsAdmin;
            }
            else
            {
                WarehouseName = config.WarehouseName;
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
        public bool CanLinkWarehouse
        {
            get => canLinkWarehouse;
            set => Set(ref canLinkWarehouse, value);
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
        private async Task OnSelectWarehouse()
        {
            WarehouseViewModel warehouse = warehouseList.ChooseWarehouse();
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
                    CanLinkWarehouse = false;
                }
                else
                {
                    messageHelper.ShowError("Could not associate this ShipWorks database with the warehouse.");
                }
            }
        }
    }
}
