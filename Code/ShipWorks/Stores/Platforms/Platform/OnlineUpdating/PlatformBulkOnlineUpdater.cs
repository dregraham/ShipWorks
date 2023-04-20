using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Bulk Upload shipment details to Platform
    /// </summary>
    /// <remarks>
    /// When other stores are supported, move the KeyedComponent for the working storetype to here.
    /// </remarks>
    public class PlatformBulkOnlineUpdater : PlatformOnlineUpdater, IPlatformOnlineUpdater
    {
        private readonly IHubPlatformClient platformWebClient;
        private static readonly ILog log = LogManager.GetLogger(typeof(PlatformBulkOnlineUpdater));
        private const string bulkShipNotifyEndpoint = "v-beta/order_sources/{0}/notify_shipped/bulk";

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformBulkOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient,
            IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory, IHubPlatformClient platformWebClient,
            IIndex<StoreTypeCode, IPlatformOnlineUpdaterBehavior> platformOnlineUpdateBehavior) :
            base(orderManager, shippingManager, sqlAdapterFactory, createWarehouseOrderClient, storeSpecificOnlineUpdaterFactory, platformOnlineUpdateBehavior)
        {
            this.platformWebClient = platformWebClient;
        }

        /// <summary>
        /// Upload shipments to Platform (bulk)
        /// </summary
        protected override async Task UploadShipmentsToPlatform(List<ShipmentEntity> shipments, StoreEntity store, IWarehouseOrderClient client)
        {
            var behavior = GetPlatformOnlineUpdaterBehavior(store);
            try
            {
                var updates = shipments
                    .Select(async s => await GetPlatformBulkOnlineUpdateItem(s, behavior))
                    .Select(r => r.Result)
                    .Where(s => s != null)
                    .ToList();

                if (!updates.Any())
                {
                    log.WarnFormat("Not uploading bulk shipments to platform for store {0}, no valid shipments found.", store.StoreID);
                    return;
                }

                var request = new NotifyMarketplaceShippedRequest
                {
                    NotifyMarketplaceShippedRequests = updates
                };

                await platformWebClient.CallViaPassthrough(request, string.Format(bulkShipNotifyEndpoint, store.OrderSourceID), HttpMethod.Post, "UploadShipments").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new PlatformStoreException($"Error uploading shipment details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create a Shipment Notification item to add to the bulk
        /// </summary>
        private async Task<PlatformBulkOnlineUpdateItem> GetPlatformBulkOnlineUpdateItem(ShipmentEntity shipment, IPlatformOnlineUpdaterBehavior behavior)
        {
            await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
            var update = new PlatformBulkOnlineUpdateItem
            {
                SalesOrderId = shipment.Order.ChannelOrderID,
                TrackingNumber = shipment.TrackingNumber,
                CarrierCode = behavior.GetCarrierName(shippingManager, shipment),
                ShipFrom = new ShipFrom
                {
                    Name = shipment.OriginUnparsedName,
                    Phone = shipment.OriginPhone,
                    CompanyName = shipment.OriginCompany,
                    AddressLine1 = shipment.OriginStreet1,
                    AddressLine2 = shipment.OriginStreet2,
                    AddressLine3 = shipment.OriginStreet3,
                    CityLocality = shipment.OriginCity,
                    StateProvince = shipment.OriginStateProvCode,
                    PostalCode = shipment.OriginPostalCode,
                    CountryCode = shipment.AdjustedOriginCountryCode(),
                }
            };
            return update;
        }
    }
}