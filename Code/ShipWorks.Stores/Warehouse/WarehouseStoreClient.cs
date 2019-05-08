using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;

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
        public WarehouseStoreClient(ILicenseService licenseService, WarehouseRequestClient warehouseRequestClient, StoreDtoFactory storeDtoFactory)
        {
            this.licenseService = licenseService;
            this.warehouseRequestClient = warehouseRequestClient;
            this.storeDtoFactory = storeDtoFactory;
        }

        /// <summary>
        /// Upload the store to warehouse mode
        /// </summary>
        public async Task<Result> UploadStoreToWarehouse(StoreEntity store)
        {
            if (!WarehouseStoreTypes.IsSupported(store.StoreTypeCode))
            {
                return Result.FromSuccess();
            }

            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.Stores(store.WarehouseStoreID?.ToString("D")), Method.POST);
                    request.JsonSerializer = new RestSharpJsonNetSerializer();
                    request.RequestFormat = DataFormat.Json;

                    Store storeDto = await storeDtoFactory.Create(store).ConfigureAwait(false);
                    request.AddJsonBody(storeDto);

                    GenericResult<IRestResponse> response = await warehouseRequestClient.MakeRequest(request, "Upload Store")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return Result.FromError(response.Message);
                    }

                    if(Guid.TryParse(JObject.Parse(response.Value.Content)["id"]?.ToString(), out Guid warehouseStoreId))
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
