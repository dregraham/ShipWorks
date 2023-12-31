﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Editions;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Client for interacting with Odbc Stores and the hub
    /// </summary>
    [Component]
    public class OdbcStoreClient : IOdbcStoreClient
    {
        private readonly ILicenseService licenseService;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcStoreClient(ILicenseService licenseService, IWarehouseRequestClient warehouseRequestClient)
        {
            this.licenseService = licenseService;
            this.warehouseRequestClient = warehouseRequestClient;
        }

        /// <summary>
        /// Get odbc stores for the customer
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
                        if (storeType == (int) StoreTypeCode.Odbc)
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

        /// <summary>
        /// Get the given odbc store
        /// </summary>
        public async Task<GenericResult<OdbcStore>> GetStore(Guid warehouseStoreId, Store baseStore)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request = new RestRequest(WarehouseEndpoints.GetOdbcStore(warehouseStoreId.ToString("D")), Method.GET);

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Get Odbc Store")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        return GenericResult.FromError<OdbcStore>(response.Message);
                    }
                    else
                    {
                        OdbcStore result = JsonConvert.DeserializeObject<OdbcStore>(response.Value.Content);
                        result.Name = baseStore.Name;
                        result.StoreType = baseStore.StoreType;
                        result.UniqueIdentifier = baseStore.UniqueIdentifier;

                        // Dynamodb does not let you store empty strings so the string values that
                        // are empty get stored as null, when deserializing we need to fix the null
                        // values and make them empty strings again
                        result.ImportMap = result.ImportMap ?? string.Empty;
                        result.UploadMap = result.UploadMap ?? string.Empty;
                        result.ImportColumnSource = result.ImportColumnSource ?? string.Empty;
                        result.UploadColumnSource = result.UploadColumnSource ?? string.Empty;

                        return GenericResult.FromSuccess<OdbcStore>(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<OdbcStore>(ex);
            }

            return GenericResult.FromError<OdbcStore>("Not a warehouse user.");
        }
    }
}
