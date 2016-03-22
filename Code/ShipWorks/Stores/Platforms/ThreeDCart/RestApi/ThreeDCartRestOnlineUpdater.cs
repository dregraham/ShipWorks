using System;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestOnlineUpdater
    {
        private readonly ILog log;
        private readonly IThreeDCartRestWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestOnlineUpdater"/> class.
        /// </summary>
        public ThreeDCartRestOnlineUpdater(ThreeDCartStoreEntity store)
            : this(LogManager.GetLogger(typeof (ThreeDCartRestOnlineUpdater)), new ThreeDCartRestWebClient(store))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestOnlineUpdater"/> class.
        /// </summary>
        public ThreeDCartRestOnlineUpdater(ILog log, IThreeDCartRestWebClient webClient)
        {
            this.log = log;
            this.webClient = webClient;
        }

        /// <summary>
        /// Changes the status of an order
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusID)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();
            UpdateOrderStatus(orderID, statusID, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Changes the status of an order
        /// </summary>
        public void UpdateOrderStatus(long orderID, int statusID, UnitOfWork2 unitOfWork)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);
            if (order != null)
            {
                if (order.IsManual)
                {
                    log.InfoFormat($"Not uploading order status since order {order.OrderID} is manual.");
                    return;
                }

                webClient.UpdateOrderStatus(order.OrderNumber, statusID);

                // Update the local database with the new status
                OrderEntity basePrototype = new OrderEntity(orderID)
                {
                    IsNew = false,
                    OnlineStatusCode = statusID,
                    OnlineStatus = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus) statusID)
                };

                unitOfWork.AddForSave(basePrototype);
            }
            else
            {
                log.WarnFormat($"Unable to update online status for order {orderID}: cannot find order");
            }
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(OrderEntity order)
        {
            // upload tracking number for the most recent processed, not voided shipment
            ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);
            if (shipment == null)
            {
                // log that there was no shipment, and return
                log.DebugFormat("There was no shipment found for order Id: {0}", order.OrderID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.WarnFormat("Not updating status of shipment {0} as it has gone away.", shipmentID);
                return;
            }

            UpdateShipmentDetails(shipment);
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        private void UpdateShipmentDetails(ShipmentEntity shipmentEntity)
        {
            OrderEntity order = shipmentEntity.Order;
            if (order.IsManual)
            {
                log.WarnFormat("Not updating order {0} since it is manual.", shipmentEntity.Order.OrderNumberComplete);
                return;
            }

            // Fetch the order items
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            ThreeDCartOrderItemEntity threeDCartOrderItem = order.OrderItems?.FirstOrDefault(oi => oi is ThreeDCartOrderItemEntity) as ThreeDCartOrderItemEntity;

            // Get the 3D Cart shipment id from the first 3D Cart order item
            if (threeDCartOrderItem == null)
            {
                throw new ThreeDCartException("No items were found on the order. ShipWorks cannot upload tracking information without items from 3D Cart.");
            }

            ThreeDCartShipment shipment = new ThreeDCartShipment()
            {
                ShipmentID = threeDCartOrderItem.ThreeDCartShipmentID,
                ShipmentOrderStatus = (int) Enums.ThreeDCartOrderStatus.Shipped,
                ShipmentMethodID = GetShipmentMethodID(shipmentEntity),
                ShipmentMethodName = "",
                ShipmentPhone = shipmentEntity.ShipPhone,
                ShipmentFirstName = shipmentEntity.ShipFirstName,
                ShipmentLastName = shipmentEntity.ShipLastName,
                ShipmentAddress = shipmentEntity.ShipStreet1,
                ShipmentAddress2 = shipmentEntity.ShipStreet2,
                ShipmentCity = shipmentEntity.ShipCity,
                ShipmentState = shipmentEntity.ShipStateProvCode,
                ShipmentZipCode = shipmentEntity.ShipPostalCode,
                ShipmentCountry = shipmentEntity.ShipCountryCode,
                ShipmentCompany = shipmentEntity.ShipCompany,
                ShipmentEmail = shipmentEntity.ShipEmail,
                ShipmentLastUpdate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                ShipmentShippedDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                ShipmentTrackingCode = shipmentEntity.TrackingNumber,
                ShipmentWeight = shipmentEntity.TotalWeight
            };

            webClient.UploadShipmentDetails(order.OrderNumber, shipment);
        }

        /// <summary>
        /// Gets the shipment method id.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns></returns>
        private int GetShipmentMethodID(ShipmentEntity shipmentEntity)
        {
            throw new NotImplementedException();
        }
    }
}