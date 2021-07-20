using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Settings.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Manual
{
    /// <summary>
    /// Activity for creating a default manual store
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class CreateDefaultStoreActivity : IInitializeForCurrentSession
    {
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;
        private readonly IConfigurationData configurationData;
        private readonly ILicenseService licenseService;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateDefaultStoreActivity(IStoreTypeManager storeTypeManager, IWarehouseSettingsApi warehouseSettingsApi, 
            IConfigurationData configurationData, ILicenseService licenseService, ISqlAdapterFactory sqlAdapterFactory, IStoreManager storeManager)
        {
            this.storeTypeManager = storeTypeManager;
            this.warehouseSettingsApi = warehouseSettingsApi;
            this.configurationData = configurationData;
            this.licenseService = licenseService;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Create the default store
        /// </summary>
        public async Task Create()
        {
            IConfigurationEntity configuration = configurationData.FetchReadOnly();

            if (storeManager.GetDatabaseStoreCount() == 0 && licenseService.IsHub && !string.IsNullOrEmpty(configuration.WarehouseID))
            {
                StoreType storeType = storeTypeManager.GetType(StoreTypeCode.Manual);
                StoreEntity store = storeType.CreateStoreInstance();
                store.StartSetup();

                var warehouseResult = await warehouseSettingsApi.GetAllWarehouses().ConfigureAwait(false);

                var warehouse = warehouseResult.Value.warehouses.SingleOrDefault(x => x.id == configuration.WarehouseID);

                if (warehouse != null)
                {
                    PersonAdapter address = store.Address;

                    store.StoreName = "My Test Store";
                    licenseService.GetLicense(store).Activate(store);

                    address.UnparsedName = warehouse.details.Name;
                    address.Company = "";
                    address.City = warehouse.details.City;
                    address.Street1 = warehouse.details.Street;
                    address.Street2 = "";
                    address.Street3 = "";
                    address.StateProvCode = warehouse.details.State;
                    address.PostalCode = warehouse.details.Zip;
                    address.Phone = "";
                    address.Fax = "";
                    address.Email = "";
                    address.Website = "";

                    // Save the store
                    await storeManager.SaveStoreAsync(store).ConfigureAwait(false);
                    store.CompleteSetup();

                    var sqlAdapter = sqlAdapterFactory.Create();

                    // Create the default presets
                    CreateDefaultStatusPreset(store, StatusPresetTarget.Order, sqlAdapter);
                    CreateDefaultStatusPreset(store, StatusPresetTarget.OrderItem, sqlAdapter);

                    // Create the default filters
                    StoreFilterRepository storeFilterRepository = new StoreFilterRepository(store);
                    storeFilterRepository.Save(true);

                    storeManager.SaveStore(store);
                }
            }
        }

        /// <summary>
        /// Create a default status preset for the default store
        /// </summary>
        private void CreateDefaultStatusPreset(StoreEntity store, StatusPresetTarget presetTarget, ISqlAdapter sqlAdapter)
        {
            StatusPresetEntity preset = new StatusPresetEntity();
            preset.StoreID = store.StoreID;
            preset.StatusTarget = (int) presetTarget;
            preset.StatusText = "";
            preset.IsDefault = true;

            sqlAdapter.SaveEntity(preset);
        }

        /// <summary>
        /// Initializes the object
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Create().Wait();
        }
    }
}