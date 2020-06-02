using System;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    public class DefaultWarehouseCreator : IDefaultWarehouseCreator
    {
        private readonly ILicenseService licenseService;
        private readonly IConfigurationData configurationData;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;

        public DefaultWarehouseCreator(ILicenseService licenseService, IConfigurationData configurationData,
                                       IWarehouseSettingsApi warehouseSettingsApi)
        {
            this.licenseService = licenseService;
            this.configurationData = configurationData;
            this.warehouseSettingsApi = warehouseSettingsApi;
        }

        public Task<Result> CreateIfNeeded(IStoreEntity store)
        {
            throw new NotImplementedException();
        }
    }
}
