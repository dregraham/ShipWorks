using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Odbc.Warehouse;
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
        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly ILicenseService licenseService;
        private readonly ShipmentDtoFactory shipmentDtoFactory;
        private readonly Func<IUploadOrdersRequest> uploadOrderRequestCreator;
        private readonly ILog log;

        public const string RestrictedErrorMessage = "Attempted to upload order to hub for a non warehouse customer";

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public WarehouseOrderClient(
            IWarehouseRequestClient warehouseRequestClient,
            ILicenseService licenseService,
            ShipmentDtoFactory shipmentDtoFactory,
            IWarehouseOrderDtoFactory warehouseOrderDtoFactory,
            Func<IUploadOrdersRequest> uploadOrderRequestCreator,
            Func<Type, ILog> logFactory)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.licenseService = licenseService;
            this.shipmentDtoFactory = shipmentDtoFactory;
            this.uploadOrderRequestCreator = uploadOrderRequestCreator;
            log = logFactory(typeof(WarehouseOrderClient));
        }

        /// <summary>
        /// Get orders for the given warehouse ID from the ShipWorks Warehouse app
        /// </summary>
        public async Task<WarehouseGetOrdersResponse> GetOrders(string warehouseID, string warehouseStoreID,
            long mostRecentSequence, StoreTypeCode storeType, Guid batchId)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.Orders(warehouseID), Method.GET)
                    .AddHeader("Version", "2")
                    .AddQueryParameter("storeId", warehouseStoreID)
                    .AddQueryParameter("lastSequence", mostRecentSequence.ToString())
                    .AddQueryParameter("batchId", batchId.ToString());

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Get Orders")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    throw new DownloadException(response.Message, response.Exception);
                }

                var orderResponse = JsonConvert.DeserializeObject<WarehouseGetOrdersResponse>(
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

                return orderResponse;
            }
            catch (Exception ex) when (ex.GetType() != typeof(DownloadException))
            {
                throw new DownloadException($"An error occurred downloading orders for warehouse ID {warehouseID}", ex);
            }
        }

        /// <summary>
        /// Upload a order to the hub
        /// </summary>
        public async Task<GenericResult<WarehouseUploadOrderResponses>> UploadOrders(IEnumerable<OrderEntity> orders, IStoreEntity store)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel =
                    licenseService.CheckRestriction(EditionFeature.Warehouse, null);
                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    return await uploadOrderRequestCreator().Submit(orders, store);
                }
                else
                {
                    log.Error(RestrictedErrorMessage);
                    return GenericResult.FromError<WarehouseUploadOrderResponses>(RestrictedErrorMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Failed to upload order to hub. First Order ID was {orders.FirstOrDefault()?.OrderID.ToString() ?? "???"}", ex);
                return GenericResult.FromError<WarehouseUploadOrderResponses>(ex);
            }
        }

        /// <summary>
        /// Send a shipment to the hub
        /// </summary>
        public async Task<Result> UploadShipment(ShipmentEntity shipmentEntity, Guid hubOrderID, string tangoShipmentID)
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
                        return Result.FromError(response.Exception);
                    }

                    return Result.FromSuccess();
                }

                string restrictedErrorMessage = "Attempted to upload shipment to hub for a non warehouse customer";
                log.Error(restrictedErrorMessage);
                return Result.FromError(restrictedErrorMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to upload shipment {shipmentEntity.ShipmentID} to hub.", ex);
                return Result.FromError(ex);
            }
        }

        /// <summary>
        /// Send void to the hub
        /// </summary>
        public async Task<Result> UploadVoid(long shipmentID, Guid hubOrderID, string tangoShipmentID)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel =
                    licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    IRestRequest request =
                        new RestRequest(WarehouseEndpoints.VoidShipment(hubOrderID.ToString("N")), Method.PUT);

                    VoidShipment voidShipment = shipmentDtoFactory.CreateVoidShipment(shipmentID, tangoShipmentID);
                    request.AddJsonBody(JsonConvert.SerializeObject(voidShipment));

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Void Order")
                        .ConfigureAwait(false);

                    if (response.Failure)
                    {
                        log.Error($"Failed to upload shipment {shipmentID} to hub. {response.Message}",
                                  response.Exception);
                        return Result.FromError(response.Exception);
                    }

                    return Result.FromSuccess();
                }

                string restrictedErrorMessage = "Attempted to upload voided shipment to hub for a non warehouse customer";
                log.Error(restrictedErrorMessage);
                return Result.FromError(restrictedErrorMessage);

            }
            catch (Exception ex)
            {
                log.Error($"Failed to upload shipment {shipmentID} to hub.", ex);
                return Result.FromError(ex);
            }
        }
    }
}
