using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.ApplicationCore.Settings.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Creates a default warehouse for the user if needed
    /// </summary>
    [Component]
    public class DefaultWarehouseCreator : IDefaultWarehouseCreator
    {
        private readonly ILicenseService licenseService;
        private readonly IConfigurationData configurationData;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;
        private const string DefaultWarehouseName = "Default Warehouse";

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultWarehouseCreator(ILicenseService licenseService, IConfigurationData configurationData,
                                       IWarehouseSettingsApi warehouseSettingsApi)
        {
            this.licenseService = licenseService;
            this.configurationData = configurationData;
            this.warehouseSettingsApi = warehouseSettingsApi;
        }

        /// <summary>
        /// Creates a default warehouse in the hub and links it to the database
        /// </summary>
        public async Task<Result> Create(StoreEntity store)
        {
            Result needsWarehouseResult = await NeedsDefaultWarehouse();
            if (needsWarehouseResult.Failure)
            {
                return needsWarehouseResult;
            }

            PersonAdapter storeAddress = store.Address;
            Details warehouseDetails = new Details
            {
                Name = DefaultWarehouseName,
                Street = storeAddress.StreetAll,
                City = storeAddress.City,
                State = storeAddress.StateProvCode,
                Zip = storeAddress.PostalCode
            };

            GenericResult<string> createWarehouseResult = await warehouseSettingsApi.Create(warehouseDetails);
            if (createWarehouseResult.Failure)
            {
                return Result.FromError("Failed to create default warehouse in the hub");
            }

            Result linkWarehouseResult = await warehouseSettingsApi.Link(createWarehouseResult.Value);
            if (linkWarehouseResult.Failure)
            {
                return Result.FromError("Failed to link default warehouse to this database");
            }

            configurationData.UpdateConfiguration(x =>
            {
                x.WarehouseID = createWarehouseResult.Value;
                x.WarehouseName = DefaultWarehouseName;
            });

            return Result.FromSuccess();
        }

        /// <summary>
        /// Check if a default warehouse needs to be created
        /// </summary>
        public async Task<Result> NeedsDefaultWarehouse()
        {
            if (!licenseService.IsHub)
            {
                return Result.FromError("User does not have access to hub");
            }

            IConfigurationEntity configurationEntity = configurationData.FetchReadOnly();
            if (string.IsNullOrWhiteSpace(configurationEntity.WarehouseID))
            {
                return Result.FromError("Customer already has warehouse linked to this database");
            }

            GenericResult<WarehouseListDto> getWarehousesResult = await warehouseSettingsApi.GetAllWarehouses();
            if (getWarehousesResult.Failure)
            {
                return Result.FromError("Failed to retrieve warehouses from hub");
            }

            if (!getWarehousesResult.Value.warehouses.None())
            {
                return Result.FromError("Customer already has warehouses in the hub");
            }

            return Result.FromSuccess();
        }
    }
}
