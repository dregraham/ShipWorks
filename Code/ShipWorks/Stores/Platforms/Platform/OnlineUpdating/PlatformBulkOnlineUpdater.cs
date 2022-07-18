using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
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
    /// Currently, only supported by Amazon. When other stores are supported, move the KeyedComponent 
    /// for the working storetype to here.
    /// </remarks>
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Amazon)]
    public class PlatformBulkOnlineUpdater : PlatformOnlineUpdater, IPlatformOnlineUpdater
    {
        private readonly IHubPlatformClient platformWebClient;
        private const string bulkShipNotifyEndpoint = "v-beta/order_sources/{0}/notify_shipped/bulk";

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformBulkOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient,
            IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory, IHubPlatformClient platformWebClient) :
            base(orderManager, shippingManager, sqlAdapterFactory, createWarehouseOrderClient, storeSpecificOnlineUpdaterFactory)
        {
            this.platformWebClient = platformWebClient;
        }

        /// <summary>
        /// Upload shipments to Platform (bulk)
        /// </summary
        protected override async Task UploadShipmentsToPlatform(List<ShipmentEntity> shipments, StoreEntity store, IWarehouseOrderClient client)
        {
            try
            {
                var updates = shipments.Select(async s => await GetPlatformBulkOnlineUpdateItem(s)).Select(r => r.Result).Where(s => s != null);
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
        private async Task<PlatformBulkOnlineUpdateItem> GetPlatformBulkOnlineUpdateItem(ShipmentEntity shipment)
        {
            await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
            var update = new PlatformBulkOnlineUpdateItem
            {
                SalesOrderId = shipment.Order.ChannelOrderID,
                TrackingNumber = shipment.TrackingNumber,
                CarrierCode = GetCarrierName(shipment),
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