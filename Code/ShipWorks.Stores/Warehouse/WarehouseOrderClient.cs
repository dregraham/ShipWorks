using System;
using System.Collections.Generic;
using System.Linq;
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
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating.DTO;
using ShipWorks.Warehouse.Orders;
using ShipWorks.Warehouse.Orders.DTO;

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
        public WarehouseOrderClient(
            IWarehouseRequestClient warehouseRequestClient,
            ILicenseService licenseService,
            ShipmentDtoFactory shipmentDtoFactory,
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
        public async Task<GenericResult<IEnumerable<WarehouseUploadOrderResponse>>> UploadOrders(IEnumerable<OrderEntity> orders, IStoreEntity store, bool assignBatch)
        {
            try
            {
                EditionRestrictionLevel restrictionLevel =
                    licenseService.CheckRestriction(EditionFeature.Warehouse, null);
                if (restrictionLevel == EditionRestrictionLevel.None)
                {
                    return await uploadOrderRequestCreator().Submit(orders, store, assignBatch);
                }
                else
                {
                    log.Error(RestrictedErrorMessage);
                    return GenericResult.FromError<IEnumerable<WarehouseUploadOrderResponse>>(RestrictedErrorMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Failed to upload order to hub. First Order ID was {orders.FirstOrDefault()?.OrderID.ToString() ?? "???"}", ex);
                return GenericResult.FromError<IEnumerable<WarehouseUploadOrderResponse>>(ex);
            }
        }

        /// <summary>
        /// Upload shipment notification information to Hub. This will only work for orders from shipengine
        /// </summary>
        public async Task<Result> NotifyShipped(string salesOrderId, string trackingNumber, string carrier)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.NotifyShipped(salesOrderId), Method.POST);
                request.AddJsonBody(new ShipmentNotification(trackingNumber, carrier));
                request.JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault();

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Notify Shipped")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    log.Error($"Failed to notify shipment {salesOrderId} on the hub. {response.Message}",
                              response.Exception);
                    return Result.FromError(response.Exception);
                }

                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to notify shipment {salesOrderId} on the hub.", ex);
                return Result.FromError(ex);
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

        /// <summary>
        /// Reroute order items for the given warehouse ID from the ShipWorks Warehouse app
        /// </summary>
        public async Task<Result> RerouteOrderItems(Guid hubOrderID, ItemsToReroute itemsToReroute)
        {
            try
            {
                if (licenseService.IsHub)
                {
                    IRestRequest request =
                        new RestRequest(WarehouseEndpoints.RerouteOrderItems(hubOrderID.ToString("N")), Method.PUT);

                    request.AddJsonBody(JsonConvert.SerializeObject(itemsToReroute));

                    GenericResult<IRestResponse> response = await warehouseRequestClient
                        .MakeRequest(request, "Reroute Order items")
                        .ConfigureAwait(true);

                    if (response.Failure)
                    {
                        log.Error($"Failed to reroute order items for order {hubOrderID} to hub. {response.Message}",
                            response.Exception);
                        throw response.Exception;
                    }

                    return Result.FromSuccess();
                }

                string restrictedErrorMessage = "Attempted to reroute order items to hub for a non warehouse customer";
                log.Error(restrictedErrorMessage);
                throw new InvalidOperationException(restrictedErrorMessage);
            }
            catch (Exception ex)
            {
                log.Error($"Failed to reroute order items for order {hubOrderID} to hub.", ex);
                throw;
            }
        }

        /// <summary>
        /// Upload Amazon shipments
        /// </summary>
        public async Task<Result> UploadAmazonShipments(AmazonBulkUploadShipmentsRequest body)
        {
            try
            {
                IRestRequest request = new RestRequest(WarehouseEndpoints.UploadAmazonShipments, Method.POST);
                request.AddJsonBody(body);
                request.JsonSerializer = RestSharpJsonNetSerializer.CreateHubDefault();

                GenericResult<IRestResponse> response = await warehouseRequestClient
                    .MakeRequest(request, "Upload Amazon Shipments")
                    .ConfigureAwait(true);

                if (response.Failure)
                {
                    log.Error($"Failed to upload Amazon shipments. {response.Message}",
                              response.Exception);
                    return Result.FromError(response.Exception);
                }

                return Result.FromSuccess();
            }
            catch (Exception ex)
            {
                log.Error($"Failed to Amazon shipments: {ex.Message}", ex);
                return Result.FromError(ex);
            }
        }
    }
}
