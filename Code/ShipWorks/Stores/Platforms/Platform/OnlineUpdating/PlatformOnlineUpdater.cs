using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Platform
    /// </summary>
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Api)]
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.BrightpearlHub)]
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.WalmartHub)]
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.ChannelAdvisorHub)]
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.VolusionHub)]
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.GrouponHub)]
    public class PlatformOnlineUpdater : IPlatformOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PlatformOnlineUpdater));
        private readonly IOrderManager orderManager;
        protected readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<IWarehouseOrderClient> createWarehouseOrderClient;
        private readonly IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient,
            IIndex<StoreTypeCode, IOnlineUpdater> storeSpecificOnlineUpdaterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.createWarehouseOrderClient = createWarehouseOrderClient;
            this.storeSpecificOnlineUpdaterFactory = storeSpecificOnlineUpdaterFactory;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Newer stores use the swat id to upload Platform shipments
        /// </summary>
        protected virtual bool UseSwatId => false;

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public async Task UploadOrderShipmentDetails(StoreEntity store, IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            var orders = await LoadOrders(orderKeys).ConfigureAwait(false);

            foreach (var order in orders)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID, false).ConfigureAwait(false);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", order.OrderID);
                }
                else
                {
                    shipment.Order = order;
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(store, shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Load the orders for the given keys
        /// </summary>
        private async Task<IEnumerable<OrderEntity>> LoadOrders(IEnumerable<long> orderKeys)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await orderManager.LoadOrdersAsync(orderKeys, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(StoreEntity store, IEnumerable<long> shipmentKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long shipmentID in shipmentKeys)
            {
                var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
                ShipmentEntity shipment = shipmentAdapter?.Shipment;

                if (shipment == null)
                {
                    log.InfoFormat("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID);
                }
                else if (!shipment.Processed)
                {
                    log.InfoFormat($"Not uploading non-processed shipment with ID {shipmentID}");
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(store, shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(StoreEntity store, List<ShipmentEntity> shipments)
        {
            var nonPlatformShipments = shipments.Where(x => x.Order.ChannelOrderID.IsNullOrWhiteSpace());

            if (nonPlatformShipments.Any())
            {
                if (storeSpecificOnlineUpdaterFactory.TryGetValue(store.StoreTypeCode, out var uploader))
                {
                    await uploader.UploadShipmentDetails(store, nonPlatformShipments.ToList());
                }
                else
                {
                    throw new PlatformStoreException($"Could not find store-specific uploader for type code {store.StoreTypeCode}");
                }
            }

            var client = createWarehouseOrderClient();

            await UploadShipmentsToPlatform(shipments.Where(x => !x.Order.ChannelOrderID.IsNullOrWhiteSpace()).ToList(), store, client).ConfigureAwait(false);
        }

        /// <summary>
        /// Upload shipments to Platform (one at a time)
        /// </summary
        protected virtual async Task UploadShipmentsToPlatform(List<ShipmentEntity> shipments, StoreEntity store, IWarehouseOrderClient client)
        {
            foreach (var shipment in shipments.Where(x => !x.Order.ChannelOrderID.IsNullOrWhiteSpace()))
            {
                await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
                string carrier = GetCarrierName(shipment);
                var result = await client.NotifyShipped(shipment.Order.ChannelOrderID, shipment.TrackingNumber, carrier, UseSwatId).ConfigureAwait(false);
                result.OnFailure(ex => throw new PlatformStoreException($"Error uploading shipment details: {ex.Message}", ex));
            }
        }

        /// <summary>
        /// Gets the carrier name that is allowed in shipengine
        /// </summary>
        protected string GetCarrierName(ShipmentEntity shipment)
        {
            CarrierDescription otherDesc = null;
            ShipmentTypeCode shipmentTypeCode = shipment.ShipmentTypeCode;

            if (shipmentTypeCode == ShipmentTypeCode.Other)
            {
                otherDesc = shippingManager.GetOtherCarrierDescription(shipment);
            }

            string sfpName = string.Empty;
            if (shipmentTypeCode == ShipmentTypeCode.AmazonSFP)
            {
                sfpName = shipment.AmazonSFP.CarrierName.ToUpperInvariant();
            }

            switch (true)
            {
                case true when otherDesc?.IsDHL ?? false:
                case true when ShipmentTypeManager.ShipmentTypeCodeSupportsDhl(shipmentTypeCode) &&
                        ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service):
                    return "dhl_ecommerce";

                case true when shipmentTypeCode == ShipmentTypeCode.Endicia:
                case true when shipmentTypeCode == ShipmentTypeCode.Express1Endicia:
                    return "endicia";

                case true when ShipmentTypeManager.IsPostal(shipmentTypeCode):
                case true when otherDesc?.IsUSPS ?? false:
                case true when sfpName.Equals("USPS", StringComparison.OrdinalIgnoreCase):
                case true when sfpName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase):
                    return "stamps_com";

                case true when ShipmentTypeManager.IsFedEx(shipmentTypeCode):
                case true when otherDesc?.IsFedEx ?? false:
                case true when sfpName.Equals("FEDEX", StringComparison.OrdinalIgnoreCase):
                    return "fedex";

                case true when ShipmentTypeManager.IsUps(shipmentTypeCode):
                case true when otherDesc?.IsUPS ?? false:
                case true when sfpName.Equals("UPS", StringComparison.OrdinalIgnoreCase):
                    return "ups";

                case true when shipmentTypeCode == ShipmentTypeCode.DhlExpress:
                    return "dhl_express";

                case true when shipmentTypeCode == ShipmentTypeCode.DhlEcommerce:
                    return "dhl_global_mail";

                case true when shipmentTypeCode == ShipmentTypeCode.OnTrac:
                case true when sfpName.Equals("ONTRAC", StringComparison.OrdinalIgnoreCase):
                    return "ontrac";

                case true when shipmentTypeCode == ShipmentTypeCode.AmazonSWA:
                    return "amazon_shipping";

                case true when shipmentTypeCode == ShipmentTypeCode.Asendia:
                    return "asendia";

                default:
                    return "other";
            }
        }
    }
}