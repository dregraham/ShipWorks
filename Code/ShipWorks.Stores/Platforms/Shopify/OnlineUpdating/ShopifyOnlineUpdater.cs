using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
    /// <summary>
    /// Updates Shopify order status/shipments
    /// </summary>
    [Component]
    public class ShopifyOnlineUpdater : IShopifyOnlineUpdater
    {
        private readonly ILog log;
        private readonly IShopifyOrderSearchProvider orderSearchProvider;
        private readonly Func<ShopifyStoreEntity, IProgressReporter, IShopifyWebClient> createWebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyOnlineUpdater(IShopifyOrderSearchProvider orderSearchProvider,
            Func<ShopifyStoreEntity, IProgressReporter, IShopifyWebClient> createWebClient,
            Func<Type, ILog> createLogger)
        {
            this.createWebClient = createWebClient;
            this.orderSearchProvider = orderSearchProvider;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public async Task UpdateOnlineStatus(ShopifyStoreEntity store, ShopifyOrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "order is required.");
            }

            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            UnitOfWork2 unitOfWork = new UnitOfWork2();
            await UpdateOnlineStatus(store, shipment, unitOfWork).ConfigureAwait(false);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateOnlineStatus(ShopifyStoreEntity store, long shipmentID, UnitOfWork2 unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork", "unitOfWork is required");
            }

            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            await UpdateOnlineStatus(store, shipment, unitOfWork).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        private async Task UpdateOnlineStatus(ShopifyStoreEntity store, ShipmentEntity shipment, UnitOfWork2 unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork", "unitOfWork is required");
            }

            if (shipment.Order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipment.Order.OrderNumberComplete);
                return;
            }

            ShopifyOrderEntity order = (ShopifyOrderEntity) shipment.Order;

            await UploadOrderShipmentDetails(store, shipment, order).ConfigureAwait(false);

            order.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled;
            order.OnlineStatus = EnumHelper.GetDescription(ShopifyStatus.Shipped);

            unitOfWork.AddForSave(order);
        }

        /// <summary>
        /// Update the online status of the given orders
        /// </summary>
        private async Task UploadOrderShipmentDetails(ShopifyStoreEntity store, ShipmentEntity shipment, IShopifyOrderEntity order)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Order.IsManual && shipment.Order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.InfoFormat("Not uploading shipment details for OrderID {0} since it is manual.", shipment.Order.OrderID);
                return;
            }

            string carrier = GetTrackingCompany(shipment);
            string trackingNumber = shipment.TrackingNumber;

            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            shipmentType.LoadShipmentData(shipment, true);

            string carrierTrackingUrl = shipmentType.GetCarrierTrackingUrl(shipment);

            // Check the order's online status to see if it's Fulfilled.  If it is, don't try to re-ship it...it will throw an error.
            if ((ShopifyFulfillmentStatus) order.FulfillmentStatusCode == ShopifyFulfillmentStatus.Fulfilled)
            {
                log.WarnFormat("Not updating shipment status of Order {0} since it is already 'Fulfilled'", order.OrderNumberComplete);
                return;
            }

            var orderSearchEntities = await orderSearchProvider.GetOrderIdentifiers(shipment.Order).ConfigureAwait(false);
            var webClient = createWebClient(store, null);

            orderSearchEntities
                .Select(x => PerformUpload(webClient, x.ShopifyOrderID, carrier, trackingNumber, carrierTrackingUrl))
                .ThrowIfNotEmpty((msg, ex) => new ShopifyException(msg, ex));
        }

        /// <summary>
        /// Perform the upload
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="trackingNumber"></param>
        /// <param name="carrierTrackingUrl"></param>
        /// <param name="webClient"></param>
        /// <param name="orderSearchEntity"></param>
        private IResult PerformUpload(IShopifyWebClient webClient, long shopifyOrderID, string carrier, string trackingNumber, string carrierTrackingUrl)
        {
            try
            {
                webClient.UploadOrderShipmentDetails(shopifyOrderID, trackingNumber, carrier, carrierTrackingUrl);
            }
            catch (ShopifyAlreadyUploadedException ex)
            {
                log.Warn(ex.Message);
            }
            catch (ShopifyException ex)
            {
                return Result.FromError(ex);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Gets the tracking company for uploading tracking to Shopify
        /// </summary>
        private static string GetTrackingCompany(ShipmentEntity shipment)
        {
            if (ShipmentTypeManager.IsPostal(shipment.ShipmentTypeCode))
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
                if (ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service))
                {
                    return "DHL eCommerce";
                }
            }

            return ShippingManager.GetCarrierName((ShipmentTypeCode) shipment.ShipmentType);
        }
    }
}
