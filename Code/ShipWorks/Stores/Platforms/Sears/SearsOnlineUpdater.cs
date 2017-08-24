using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Online
    /// </summary>
    public class SearsOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SearsOnlineUpdater));

        /// <summary>
        /// Upload the shipment details for the given shipment ID
        /// </summary>
        public async Task UploadShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);

            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", shipmentID);
                return;
            }

            await UploadShipmentDetails(shipment).ConfigureAwait(false);
        }

        /// <summary>
        /// Upload the shipment details for the given shipment
        /// </summary>
        public async Task UploadShipmentDetails(ShipmentEntity shipment)
        {
            SearsOrderEntity order = shipment.Order as SearsOrderEntity;
            if (order == null)
            {
                log.Error("shipment not associated with order in SearsOnlineUpdater");
                throw new SearsException("Shipment not associated with order");
            }

            if (order.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for manual order {0}", order.OrderNumberComplete);
                return;
            }

            SearsStoreEntity storeEntity = StoreManager.GetRelatedStore(order.OrderID) as SearsStoreEntity;
            if (storeEntity == null)
            {
                log.InfoFormat("Could not find the sears store for order {0}", order.OrderNumberComplete);
                return;
            }

            SearsWebClient webClient = new SearsWebClient(storeEntity);

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IShippingManager shippingManager = scope.Resolve<IShippingManager>();
                ShipmentEntity loadedShipment = await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);

                ISqlAdapterFactory sqlAdapterFactory = scope.Resolve<ISqlAdapterFactory>();
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    ISearsCombineOrderSearchProvider searchProvider = scope.Resolve<ISearsCombineOrderSearchProvider>();
                    IEnumerable<SearsOrderDetail> orderDetails = await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

                    foreach (SearsOrderDetail orderDetail in orderDetails)
                    {
                        List<SearsTracking> searsTrackingDetails = await GetSearsTracking(orderDetail, loadedShipment, sqlAdapter).ConfigureAwait(false);
                        IEnumerable<SearsTracking> poTrackingDetails = searsTrackingDetails.Where(st => st.PoNumber == orderDetail.PoNumber);

                        webClient.UploadShipmentDetails(orderDetail, poTrackingDetails);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the tracking info to send to Sears
        /// </summary>
        /// <remarks>
        /// We're not checking whether the order is manual because it's possible a real order was combined into a manual
        /// order. In that case, we'd still have legit items to upload. So just try to get SearsOrderItems for the
        /// order because manually created orders will only have generic OrderItems.</remarks>
        private async Task<List<SearsTracking>> GetSearsTracking(SearsOrderDetail orderDetail, ShipmentEntity shipment, ISqlAdapter sqlAdapter)
        {
            var orderItems = await FetchOrderItems(orderDetail.OrderID, orderDetail.PoNumber, sqlAdapter).ConfigureAwait(false);

            return orderItems.Where(x => !string.IsNullOrEmpty(x.ItemID))
                .Select(searsOrderItem => CreateSearsTracking(orderDetail, searsOrderItem, shipment))
                .ToList();
        }

        /// <summary>
        /// Fetch the order items for the given order
        /// </summary>
        private async Task<IEnumerable<ISearsOrderItemEntity>> FetchOrderItems(long orderID, string poNumber, ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();

            // Fetch the order items
            InnerOuterJoin from = factory.SearsOrderItem
                .LeftJoin(factory.SearsOrderSearch)
                .On(SearsOrderSearchFields.OriginalOrderID == SearsOrderItemFields.OriginalOrderID & 
                    SearsOrderSearchFields.OrderID == SearsOrderItemFields.OrderID);

            IPredicate poNumberPredicate = SearsOrderSearchFields.PoNumber.IsNull().Or(SearsOrderSearchFields.PoNumber == poNumber);

            EntityQuery<SearsOrderItemEntity> query = factory.Create<SearsOrderItemEntity>()
                .From(from)
                .Where(SearsOrderItemFields.OrderID == orderID)
                .AndWhere(poNumberPredicate);

            IEntityCollection2 items = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            return items.OfType<ISearsOrderItemEntity>();
        }

        /// <summary>
        /// Create a Sears tracking object
        /// </summary>
        private SearsTracking CreateSearsTracking(SearsOrderDetail orderDetail, ISearsOrderItemEntity item, ShipmentEntity shipment)
        {
            string carrier = SearsUtility.GetShipmentCarrierCode(shipment);
            string method = SearsUtility.GetShipmentServiceCode(shipment);

            return new SearsTracking
            {
                OrderID = orderDetail.OrderID,
                OrderDate = orderDetail.OrderDate,
                ShipDate = shipment.ShipDate,
                TrackingNumber = shipment.TrackingNumber,
                Carrier = carrier,
                Method = method,
                OriginalOrderID = item.OriginalOrderID,
                LineNumber = item.LineNumber,
                PoNumber = orderDetail.PoNumber,
                ItemID = item.ItemID,
                Quantity = item.Quantity
            }; ;
        }
    }
}
