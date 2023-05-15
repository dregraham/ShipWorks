using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating.DTO;
using ShipWorks.Stores.Platforms.Platform;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating
{
    /// <summary>
    /// Bulk Upload shipment details to Amazon SP
    /// </summary>
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Amazon)]
    public class AmazonBulkOnlineUpdater : PlatformOnlineUpdater, IPlatformOnlineUpdater
    {
        private readonly IAmazonOrderSearchProvider orderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonBulkOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient,
            IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory, IAmazonOrderSearchProvider orderSearchProvider,
            IIndex<StoreTypeCode, IPlatformOnlineUpdaterBehavior> platformOnlineUpdateBehavior) :
            base(orderManager, shippingManager, sqlAdapterFactory, createWarehouseOrderClient, storeSpecificOnlineUpdaterFactory, platformOnlineUpdateBehavior)
        {
            this.orderSearchProvider = orderSearchProvider;
        }

        /// <summary>
        /// Upload shipments to Platform (bulk)
        /// </summary
        protected override async Task UploadShipmentsToPlatform(List<ShipmentEntity> shipments, StoreEntity store, IWarehouseOrderClient client)
        {
            try
            {
                var typedStore = (AmazonStoreEntity) store;

                var requestShipments = await Task.WhenAll(shipments.Select(async s => await GetShipmentRequests(s).ConfigureAwait(false))).ConfigureAwait(false);
                var request = new AmazonBulkUploadShipmentsRequest
                {
                    MarketplaceId = typedStore.MarketplaceID,
                    OrderSourceId = typedStore.OrderSourceID,
                    Shipments = requestShipments.SelectMany(x => x).ToList(),
                };

                var result = await client.UploadAmazonShipments(request).ConfigureAwait(false);

                if (result.Failure)
                {
                    throw result.Exception;
                }
            }
            catch (Exception ex)
            {
                throw new PlatformStoreException($"Error uploading shipment details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create a Shipment Notification item to add to the bulk
        /// </summary>
        private async Task<IEnumerable<AmazonUploadShipment>> GetShipmentRequests(ShipmentEntity shipment)
        {
            await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);

            // Get the service used and strip out any non-ascii characters
            string serviceUsed = shippingManager.GetOverriddenServiceUsed(shipment);
            serviceUsed = Regex.Replace(serviceUsed, @"[^\u001F-\u007F]", string.Empty);

            DateTime shipDate = shipment.ShipDate.ToLocalTime();

            // shipdate can't be before the order was placed
            if (shipDate < shipment.Order.OrderDate)
            {
                // set it 10 minutes after it was placed
                shipDate = shipment.Order.OrderDate.AddMinutes(10);
            }

            // shipment can't be in the future
            if (shipDate > DateTime.Now)
            {
                shipDate = DateTime.Now;
            }

            (string carrierName, string carrierCode, string trackingNumber) = AmazonUtility.GetCarrierInfoAndTrackingNumber(shipment);

            var order = (AmazonOrderEntity) shipment.Order;

            var amazonOrderIds = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            var requests = new List<AmazonUploadShipment>();

            foreach (var orderId in amazonOrderIds)
            {
                requests.Add(new AmazonUploadShipment
                {
                    AmazonOrderId = orderId,
                    CarrierCode = carrierCode,
                    CarrierName = carrierName,
                    Service = serviceUsed,
                    ShipDate = shipDate,
                    TrackingNumber = trackingNumber,
                });
            }

            return requests;
        }
    }
}