using System;
using System.Threading.Tasks;
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
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly StoreDtoFactory storeDtoFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseStoreClient(ILicenseService licenseService, WarehouseRequestClient warehouseRequestClient,
                                    StoreDtoFactory storeDtoFactory)
        {
            this.licenseService = licenseService;
            this.warehouseRequestClient = warehouseRequestClient;
            this.storeDtoFactory = storeDtoFactory;
        }

        /// <summary>
        /// Update the stores credentials
        /// </summary>
        public async Task<Result> UpdateStoreCredentials(StoreEntity store)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
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

                    Store storeDto = await storeDtoFactory.Create(store).ConfigureAwait(false);
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
        public async Task<Result> UploadStoreToWarehouse(StoreEntity store)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
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

                    Store storeDto = await storeDtoFactory.Create(store).ConfigureAwait(false);
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
    }
}
