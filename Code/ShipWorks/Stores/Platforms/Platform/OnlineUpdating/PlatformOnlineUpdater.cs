using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
    [KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Etsy)]
	[KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Shopify)]
	public class PlatformOnlineUpdater : IPlatformOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PlatformOnlineUpdater));
        private readonly IOrderManager orderManager;
        protected readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<IWarehouseOrderClient> createWarehouseOrderClient;
        private readonly IIndex<StoreTypeCode, IOnlineUpdater> legacyStoreSpecificOnlineUpdaterFactory;
        private readonly IIndex<StoreTypeCode, IPlatformOnlineUpdaterBehavior> platformOnlineUpdateBehavior;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient,
            IIndex<StoreTypeCode, IOnlineUpdater> legacyStoreSpecificOnlineUpdaterFactory,
            IIndex<StoreTypeCode, IPlatformOnlineUpdaterBehavior> platformOnlineUpdateBehavior)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.createWarehouseOrderClient = createWarehouseOrderClient;
            this.legacyStoreSpecificOnlineUpdaterFactory = legacyStoreSpecificOnlineUpdaterFactory;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.platformOnlineUpdateBehavior = platformOnlineUpdateBehavior;
        }

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
                if (legacyStoreSpecificOnlineUpdaterFactory.TryGetValue(store.StoreTypeCode, out var uploader))
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
            var behavior = GetPlatformOnlineUpdaterBehavior(store);

            foreach (var shipment in shipments.Where(x => !x.Order.ChannelOrderID.IsNullOrWhiteSpace()))
            {
				await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);

                List<SalesOrderItem> salesOrderItems = null;
                if (behavior.IncludeSalesOrderItems)
                {
                    salesOrderItems = shipment.Order.OrderItems.Select(x => new SalesOrderItem
                    {
                        SalesOrderItemId = x.StoreOrderItemID,
                        Quantity = (int) x.Quantity
                    }).ToList();
                }

                var shopifyStore = store as IShopifyStoreEntity;

                bool? notifyBuyer = shopifyStore?.ShopifyNotifyCustomer;

                var trackingUrl = behavior.GetTrackingUrl(shipment);
                var carrierName = behavior.GetCarrierName(shippingManager, shipment);
                var result = await client.NotifyShipped(shipment.Order.ChannelOrderID, shipment.TrackingNumber, trackingUrl, carrierName, behavior.UseSwatId, salesOrderItems, notifyBuyer).ConfigureAwait(false);
                result.OnFailure(ex => throw new PlatformStoreException($"Error uploading shipment details: {ex.Message}", ex));


                if (behavior.SetOrderStatusesOnShipmentNotify)
				{
					UnitOfWork2 unitOfWork = new ManagedConnectionUnitOfWork2();
					behavior.SetOrderStatuses(shipment.Order, unitOfWork);

					using (SqlAdapter adapter = new SqlAdapter(true))
					{
						unitOfWork.Commit(adapter);
						adapter.Commit();
					}
				}
            }
        }

        protected IPlatformOnlineUpdaterBehavior GetPlatformOnlineUpdaterBehavior(StoreEntity store)
        {
            if (!platformOnlineUpdateBehavior.TryGetValue(store.StoreTypeCode, out var behavior))
            {
                behavior = new DefaultPlatformOnlineUpdaterBehavior();
            }

            return behavior;
        }
    }
}