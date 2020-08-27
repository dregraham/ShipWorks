using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Client for interacting with the warehouse in regard to stores
    /// </summary>
    [Component]
    public class WarehouseStoreClient : IWarehouseStoreClient
    {
        private readonly ILicenseService licenseService;
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly IIndex<StoreTypeCode, IStoreDtoFactory> storeDtoFactories;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseStoreClient(ILicenseService licenseService, IWarehouseRequestClient warehouseRequestClient,
                                    IIndex<StoreTypeCode, IStoreDtoFactory> storeDtoFactories, IConfigurationData configurationData)
        {
            this.licenseService = licenseService;
            this.warehouseRequestClient = warehouseRequestClient;
            this.storeDtoFactories = storeDtoFactories;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Update the stores credentials
        /// </summary>
        public async Task<Result> UpdateStoreCredentials(StoreEntity store)
        {
            try
            {
                if (licenseService.IsHub)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.UpdateStoreCredentials(store.WarehouseStoreID?.ToString("D")), Method.POST);
                    request.JsonSerializer = new RestSharpJsonNetSerializer(new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy
                            {
                                OverrideSpecifiedNames = false
                            }
                        },
                    });
                    request.RequestFormat = DataFormat.Json;

                    Store storeDto = await GetStoreDtoFactory(store).Create(store).ConfigureAwait(false);
                    request.AddJsonBody(storeDto);

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Upload Store")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return Result.FromError(response.Message);
                    }
                }

                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Upload the store to warehouse mode
        /// </summary>
        public async Task<Result> UploadStoreToWarehouse(StoreEntity store, bool isNew)
        {
            try
            {
                if (licenseService.IsHub)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.Stores, Method.POST);
                    request.JsonSerializer = new RestSharpJsonNetSerializer(new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy
                            {
                                OverrideSpecifiedNames = false
                            }
                        },
                    });
                    request.RequestFormat = DataFormat.Json;

                    Store storeDto = await GetStoreDtoFactory(store).Create(store).ConfigureAwait(false);
                    if (!isNew)
                    {
                        storeDto.MigrationWarehouse = configurationData.FetchReadOnly().WarehouseID;
                    }
                    request.AddJsonBody(storeDto);

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Upload Store")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return Result.FromError(response.Message);
                    }

                    if (Guid.TryParse(JObject.Parse(response.Value.Content)["id"]?.ToString(), out Guid warehouseStoreId))
                    {
                        store.WarehouseStoreID = warehouseStoreId;
                    }
                    else
                    {
                        return Result.FromError($"Invalid Response: {response.Value.Content}");
                    }
                }

                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Get the DtoFactory for the given store
        /// </summary>
        private IStoreDtoFactory GetStoreDtoFactory(IStoreEntity store) =>
            storeDtoFactories.TryGetValue(store.StoreTypeCode, out IStoreDtoFactory storeDtoFactory) ?
                storeDtoFactory :
                throw new NotSupportedException($"The StoreType {EnumHelper.GetDescription(store.StoreTypeCode)} is not supported for ShipWorks Warehouse mode.");
    }
}
