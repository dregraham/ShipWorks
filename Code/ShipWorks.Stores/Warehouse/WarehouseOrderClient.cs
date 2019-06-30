using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Communication;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Client for retrieving orders from the ShipWorks Warehouse app
    /// </summary>
    [Component]
    public class WarehouseOrderClient : IWarehouseOrderClient
    {
        private readonly WarehouseRequestClient warehouseRequestClient;
        private readonly ILicenseService licenseService;
        private readonly ShipmentDtoFactory shipmentDtoFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseOrderClient(WarehouseRequestClient warehouseRequestClient, ILicenseService licenseService,
                                    ShipmentDtoFactory shipmentDtoFactory, Func<Type, ILog> logFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.licenseService = licenseService;
            this.shipmentDtoFactory = shipmentDtoFactory;
            log = logFactory(typeof(WarehouseOrderClient));
        }

        /// <summary>
        /// Get orders for the given warehouse ID from the ShipWorks Warehouse app
        /// </summary>
        public async Task<IEnumerable<WarehouseOrder>> GetOrders(string warehouseID, string warehouseStoreID, long mostRecentSequence, StoreTypeCode storeType, Guid batchId)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.Orders(warehouseID), Method.GET);

                request.AddQueryParameter("storeId", warehouseStoreID);
                request.AddQueryParameter("lastSequence", mostRecentSequence.ToString());
                request.AddQueryParameter("batchId", batchId.ToString());

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Get Orders")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    throw new DownloadException(response.Message, response.Exception);
                }

                IEnumerable<WarehouseOrder> orders = JsonConvert.DeserializeObject<IEnumerable<WarehouseOrder>>(
                    response.Value.Content,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy
                            {
                                OverrideSpecifiedNames = false
                            }
                        },
                    });

                return orders;
            }
            catch (Exception ex) when (ex.GetType() != typeof(DownloadException))
            {
                throw new DownloadException($"An error occurred downloading orders for warehouse ID {warehouseID}", ex);
            }
        }

        /// <summary>
        /// Send a shipment to the hub
        /// </summary>
        public async Task UploadShipment(ShipmentEntity shipmentEntity, Guid hubOrderID, string tangoShipmentID)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel =
                    licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request =
                        new RestRequest(WarehouseEndpoints.ShipOrder(hubOrderID.ToString("N")), Method.PUT);

                    Shipment shipment = shipmentDtoFactory.CreateHubShipment(shipmentEntity, tangoShipmentID);
                    request.AddJsonBody(JsonConvert.SerializeObject(shipment));

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Ship Order")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        log.Error($"Failed to upload shipment {shipmentEntity.ShipmentID} to hub. {response.Message}",
                                  response.Exception);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Failed to upload shipment {shipmentEntity.ShipmentID} to hub.", ex);
            }
        }
    }
}
