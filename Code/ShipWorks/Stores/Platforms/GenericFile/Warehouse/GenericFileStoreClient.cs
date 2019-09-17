using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Editions;

namespace ShipWorks.Stores.Platforms.GenericFile.Warehouse
{
    class GenericFileStoreClient : IGenericFileStoreClient
    {
        private ILicenseService licenseService;
        private IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileStoreClient(ILicenseService licenseService, IWarehouseRequestClient warehouseRequestClient)
        {
            this.licenseService = licenseService;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get the given Generic File store
        /// </summary>
        public async Task<GenericResult<GenericFileStore>> GetStore(Guid warehouseStoreId, Store baseStore)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.GetGenericFileStore(warehouseStoreId.ToString("D")), Method.GET);

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Get Generic File Store")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return GenericResult.FromError<GenericFileStore>(response.Message);
                    }
                    else
                    {
                        GenericFileStore result = JsonConvert.DeserializeObject<GenericFileStore>(response.Value.Content);
                        result.Name = baseStore.Name;
                        result.StoreType = baseStore.StoreType;
                        result.UniqueIdentifier = baseStore.UniqueIdentifier;

                        result.ImportMap = result.ImportMap ?? string.Empty;

                        return GenericResult.FromSuccess<GenericFileStore>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<GenericFileStore>(ex);
            }

            return GenericResult.FromError<GenericFileStore>("Not a warehouse user.");
        }

        /// <summary>
        /// Get Generic File stores for the customer
        /// </summary>
        public async Task<GenericResult<Dictionary<Guid, Store>>> GetStores()
        {
            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.Stores, Method.GET);

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Get Stores")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return GenericResult.FromError<Dictionary<Guid, Store>>(response.Message);
                    }

                    Dictionary<Guid, Store> result = new Dictionary<Guid, Store>();
                    JObject storesResponse = JObject.Parse(response.Value.Content);

                    foreach (JToken item in storesResponse.SelectToken("stores"))
                    {
                        int storeType = Convert.ToInt32(item["details"]["type"]);
                        if (storeType == (int) StoreTypeCode.GenericFile)
                        {
                            Guid storeId = Guid.Parse(item["id"].ToString());
                            Store store = new Store
                            {
                                UniqueIdentifier = item["details"]["uniqueIdentifier"].ToString(),
                                Name = item["details"]["name"].ToString(),
                                StoreType = storeType
                            };

                            result.Add(storeId, store);
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<Dictionary<Guid, Store>>(ex);
            }

            return GenericResult.FromError<Dictionary<Guid, Store>>("Not a warehouse user.");
        }
    }
}
